using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Dict.utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dict
{
    public partial class WebWindow : Form
    {
        private readonly string dbConnString;
        private readonly string rootIndexPath;
        private readonly string startupPath;

        public WebWindow()
        {
            InitializeComponent();

            startupPath = Application.StartupPath;
            rootIndexPath = Path.Combine(startupPath, "www", "index.html");

            dbConnString = new SQLiteConnectionStringBuilder
            {
                DataSource = Path.Combine(startupPath, "dictdb", "dict.db"),
                Version = 3,
            }.ToString();
        }

        #region GUI
        private async void WebWindow_Load(object sender, EventArgs e)
        {
            // restore window position
            Point savedLoc = Properties.Settings.Default.MainLocation;
            Size savedSize = Properties.Settings.Default.MainSize;

            // 简单的有效性检查，防止 Empty 值导致的问题
            if (savedSize.Width > 0 && savedSize.Height > 0)
            {
                // 使用辅助类处理屏幕边界问题
                WindowPositionHelper.EnsureVisible(this, savedLoc, savedSize);
            }
            else
            {
                // 如果没有保存过数据，使用默认启动位置（通常是 CenterScreen 或 WindowsDefault）
                this.StartPosition = FormStartPosition.CenterScreen;
            }

            labelLoading.Left = (this.Width - labelLoading.Width) / 2;
            labelLoading.Top = (this.Height - labelLoading.Height) / 3;

            // init web view
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;
            webView.CoreWebView2.NavigationCompleted += (o, evt) =>
            {
                labelLoading.Visible = false;
                var localPath = webView.Source.LocalPath;
                if (localPath != Properties.Settings.Default.LastUrl)
                {
                    Properties.Settings.Default.LastUrl = localPath;
                    Properties.Settings.Default.Save();
                }
            };

            LoadLastUrl();
            // debug
            // Navigate(rootIndexPath);
        }

        private void LoadLastUrl()
        {
            try
            {
                var localPath = Properties.Settings.Default.LastUrl;
                if (File.Exists(localPath))
                {
                    var uri = new Uri(localPath).AbsoluteUri;
                    webView.CoreWebView2.Navigate(uri);
                    return;
                }
            }
            catch { }
            Navigate(rootIndexPath);
        }

        private void WebWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 只有当窗口状态不是最小化时才保存（防止程序崩溃或异常退出时保存了错误状态）
            // 但通常我们在 Closing 时，WindowState 还是关闭前的状态
            if (this.WindowState != FormWindowState.Minimized)
            {
                Properties.Settings.Default.MainLocation = this.Location;
                Properties.Settings.Default.MainSize = this.Size;
            }
            // 如果窗口是最小化关闭的，我们就不更新位置，保留上次正常的大小位置

            // 保存设置到配置文件
            Properties.Settings.Default.Save();
        }

        private void WebView_WebMessageReceived(
            object sender,
            Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs e
        )
        {
            string message = e.TryGetWebMessageAsString();
            try
            {
                var req = JObject.Parse(message);
                var resp = HandleWebRequest(req["tag"].ToString(), req["req"] as JObject);
                webView.CoreWebView2.PostWebMessageAsJson(resp);
            }
            catch (Exception ex)
            {
                Error($"{ex.GetType()}\n{ex.Message}");
            }
        }

        void Error(string message)
        {
            MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void Navigate(string localPath)
        {
            if (!File.Exists(localPath))
            {
                localPath = rootIndexPath;
            }
            string url = new Uri(localPath).AbsoluteUri;
            webView.CoreWebView2.Navigate(url);
        }

        #endregion

        #region handle web request
        string AddPercentSign(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return "%";
            }
            var sb = new StringBuilder("%");
            foreach (var c in keyword)
            {
                sb.Append(c);
                sb.Append("%");
            }
            return sb.ToString();
        }

        JObject AddOneRow(SQLiteDataReader reader, JArray rows)
        {
            var row = new JObject();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                row.Add(reader.GetName(i), JToken.FromObject(reader.GetValue(i)));
            }
            rows.Add(row);
            return row;
        }

        string HandleDictEnReq(JObject req)
        {
            var r = new JArray();
            var keyword = req["Keyword"].Value<string>();
            var isChn = req["IsChn"].Value<bool>();
            using (var conn = new SQLiteConnection(dbConnString))
            {
                conn.Open();
                var paramName = "@keyword";
                var paramValue = AddPercentSign(keyword);

                var searchColumn = isChn ? "translation" : "word";
                var query =
                    $"SELECT word, phonetic, exchange, translation FROM en WHERE {searchColumn} LIKE {paramName}";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue(paramName, paramValue);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AddOneRow(reader, r);
                        }
                    }
                }
            }
            return r.ToString();
        }

        string HandleDictCnReq(JObject req)
        {
            var r = new JArray();
            var keyword = req["Keyword"].Value<string>();

            using (var conn = new SQLiteConnection(dbConnString))
            {
                conn.Open();
                var paramName = "@keyword";
                var paramValue = AddPercentSign(keyword);

                if (req["Include-Word"].Value<bool>())
                {
                    var queryWord =
                        $"SELECT word, oldword, pinyin, radicals, strokes, explanation FROM word "
                        + $"WHERE INSTR({paramName}, word) > 0 ORDER BY INSTR({paramName}, word) ASC";
                    ExecSqlCommand(r, conn, queryWord, paramName, keyword, "word");
                }

                if (req["Include-Idiom"].Value<bool>())
                {
                    var queryIdom =
                        $"SELECT word, pinyin, derivation, explanation, example FROM idiom "
                        + $"WHERE word LIKE {paramName}";
                    ExecSqlCommand(r, conn, queryIdom, paramName, paramValue, "idiom");
                }

                if (req["Include-Xhy"].Value<bool>())
                {
                    var queryXhy =
                        $"SELECT riddle, answer FROM xiehouyu "
                        + $"WHERE (riddle LIKE {paramName} OR answer LIKE {paramName})";
                    ExecSqlCommand(r, conn, queryXhy, paramName, paramValue, "xhy");
                }
            }
            return r.ToString();
        }

        private void ExecSqlCommand(
            JArray r,
            SQLiteConnection conn,
            string query,
            string paramName,
            string paramValue,
            string tableName
        )
        {
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue(paramName, paramValue);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = AddOneRow(reader, r);
                        row["table"] = tableName;
                    }
                }
            }
        }

        string HandleWebRequest(string tag, JObject req)
        {
            switch (tag)
            {
                case "dict-en":
                    return HandleDictEnReq(req);
                default:
                    break;
            }
            return HandleDictCnReq(req);
        }
        #endregion
    }
}
