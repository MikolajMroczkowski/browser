using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cef
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        ChromiumWebBrowser browser;
        string ignoreUrl = "";
        private void frmMain_Load(object sender, EventArgs e)
        {
            browser = new ChromiumWebBrowser("dshfkje");
            browser.Dock= DockStyle.Fill;
            this.panelRender.Controls.Add(browser);
            browser.LoadError += Browser_LoadError;
            browser.TitleChanged += Browser_TitleChanged;
            browser.AddressChanged += Browser_AddressChanged;
            
        }

        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (ignoreUrl=="")
            {
                loadUrl(e.Address);
            }
            else
            {
                loadUrl(ignoreUrl);
                ignoreUrl = "";
            }
        }
        private void loadUrl(string url)
        {
            txtUrl.Invoke(new Action(delegate ()
            {
                txtUrl.Text = url;
            }));
        }
        private void Browser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            this.Invoke(new Action(delegate ()
            {
                this.Text = e.Title;
            }
            ));
        }

        private void Browser_LoadError(object sender, CefSharp.LoadErrorEventArgs e)
        {
            using(StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory+@"\pages\err.html"))
            {
                String content = sr.ReadToEnd();
                content = content.Replace("ERR", e.ErrorText);
                content = content.Replace("URL", e.FailedUrl);
                ignoreUrl = e.ErrorText;
                browser.LoadHtml(content);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            browser.Load(txtUrl.Text);
        }
    }
}
