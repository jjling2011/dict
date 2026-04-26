using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dict
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 从托盘图标打开和直接运行速度差不多
            // NotifyIcon nicon = CreateNotifyIcon();
            // Application.Run();

            Application.Run(new WebWindow());

            // nicon.Visible = false;
        }

        private static NotifyIcon CreateNotifyIcon()
        {
            Form form = null;
            var formLock = new object();

            void ShowForm()
            {
                lock (formLock)
                {
                    if (form == null)
                    {
                        form = new WebWindow();
                        form.FormClosed += (o2, evt2) => form = null;
                        form.Show();
                    }
                }
                form.Activate();
            }
            var menu = new ContextMenuStrip();
            menu.Items.Add("显示主窗口", null, (s, e) => ShowForm());
            menu.Items.Add(new ToolStripSeparator()); // 添加分隔线
            menu.Items.Add("退出程序", null, (s, e) => Application.Exit());
            var nicon = new NotifyIcon
            {
                // 工具箱图标 by Elegantthemes on <a href="https://icon-icons.com/zh/authors/103-elegantthemes">Icon-Icons.com</a>
                Icon = Properties.Resources.toolsbox,
                Text = "Toolsbox v0.1",
                ContextMenuStrip = menu,
                Visible = true,
            };
            nicon.MouseClick += (o, evt) =>
            {
                if (evt.Button == MouseButtons.Left)
                {
                    ShowForm();
                }
            };
            ShowForm();
            return nicon;
        }
    }
}
