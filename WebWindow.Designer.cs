namespace Dict
{
    partial class WebWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebWindow));
            this.webView = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnDictEn = new System.Windows.Forms.Button();
            this.btnDictCn = new System.Windows.Forms.Button();
            this.btnListCalc = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.webView)).BeginInit();
            this.SuspendLayout();
            // 
            // webView
            // 
            this.webView.AllowExternalDrop = true;
            this.webView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webView.CreationProperties = null;
            this.webView.DefaultBackgroundColor = System.Drawing.Color.White;
            this.webView.Location = new System.Drawing.Point(12, 41);
            this.webView.Name = "webView";
            this.webView.Size = new System.Drawing.Size(449, 563);
            this.webView.TabIndex = 0;
            this.webView.ZoomFactor = 1D;
            this.webView.WebMessageReceived += new System.EventHandler<Microsoft.Web.WebView2.Core.CoreWebView2WebMessageReceivedEventArgs>(this.WebView_WebMessageReceived);
            // 
            // btnHome
            // 
            this.btnHome.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHome.Location = new System.Drawing.Point(386, 12);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(75, 23);
            this.btnHome.TabIndex = 2;
            this.btnHome.Text = "返回主页";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.BtnHome_Click);
            // 
            // btnDictEn
            // 
            this.btnDictEn.Location = new System.Drawing.Point(12, 12);
            this.btnDictEn.Name = "btnDictEn";
            this.btnDictEn.Size = new System.Drawing.Size(75, 23);
            this.btnDictEn.TabIndex = 3;
            this.btnDictEn.Text = "英文词典";
            this.btnDictEn.UseVisualStyleBackColor = true;
            this.btnDictEn.Click += new System.EventHandler(this.btnDictEn_Click);
            // 
            // btnDictCn
            // 
            this.btnDictCn.Location = new System.Drawing.Point(93, 12);
            this.btnDictCn.Name = "btnDictCn";
            this.btnDictCn.Size = new System.Drawing.Size(75, 23);
            this.btnDictCn.TabIndex = 3;
            this.btnDictCn.Text = "中文词典";
            this.btnDictCn.UseVisualStyleBackColor = true;
            this.btnDictCn.Click += new System.EventHandler(this.btnDictCn_Click);
            // 
            // btnListCalc
            // 
            this.btnListCalc.Location = new System.Drawing.Point(174, 12);
            this.btnListCalc.Name = "btnListCalc";
            this.btnListCalc.Size = new System.Drawing.Size(75, 23);
            this.btnListCalc.TabIndex = 3;
            this.btnListCalc.Text = "列表计算器";
            this.btnListCalc.UseVisualStyleBackColor = true;
            this.btnListCalc.Click += new System.EventHandler(this.btnListCalc_Click);
            // 
            // WebWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 616);
            this.Controls.Add(this.btnListCalc);
            this.Controls.Add(this.btnDictCn);
            this.Controls.Add(this.btnDictEn);
            this.Controls.Add(this.btnHome);
            this.Controls.Add(this.webView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WebWindow";
            this.Text = "Dict v0.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WebWindow_FormClosing);
            this.Load += new System.EventHandler(this.WebWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.webView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Button btnDictEn;
        private System.Windows.Forms.Button btnDictCn;
        private System.Windows.Forms.Button btnListCalc;
    }
}

