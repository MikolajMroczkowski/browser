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
            browser = new ChromiumWebBrowser("https://google.pl");
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
                EmulateUrl(e.Address);
            }
            else
            {
                EmulateUrl(ignoreUrl);
                ignoreUrl = "";
            }
        }
        private void EmulateUrl(string url)
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
            if (e.ErrorText == "ERR_ABORTED")
            {
                browser.Back();
            }
            else
            {
                using (StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + @"\pages\err.html"))
                {
                    String content = sr.ReadToEnd();
                    content = content.Replace("ERR", e.ErrorText);
                    content = content.Replace("URL", e.FailedUrl);
                    ignoreUrl = e.ErrorText;
                    browser.LoadHtml(content);
                }
            }
        }

        private void LoadUrl(string url)
        {
            browser.Load(url);
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadUrl(txtUrl.Text);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (browser.CanGoBack)
            {
                browser.Back();
            }
        }

        private void btnForwoard_Click(object sender, EventArgs e)
        {
            if (browser.CanGoForward)
            {
                browser.Forward();
            }
        }
        private void txtUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadUrl(txtUrl.Text);
            }
        }
    }
}
