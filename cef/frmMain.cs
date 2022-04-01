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
using System.Data.SQLite;
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
        string cs = $@"URI=file:{AppDomain.CurrentDomain.BaseDirectory}\browser.db";
        private void frmMain_Load(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(con);
            cmd.CommandText = "create table IF NOT EXISTS history (url text,title text,lastVisit INTEGER DEFAULT(datetime('now')));";
            cmd.ExecuteNonQuery();
            con.Close();
            browser = new ChromiumWebBrowser("browser://history");
            browser.Dock= DockStyle.Fill;
            this.panelRender.Controls.Add(browser);
            browser.LoadError += Browser_LoadError;
            browser.TitleChanged += Browser_TitleChanged;
            browser.AddressChanged += Browser_AddressChanged;
            browser.LoadingStateChanged += Browser_LoadingStateChanged;
        }
        
        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            string icon = "↻";
            if (e.IsLoading)
            {
                icon = "x";
            }
            btnReload.Invoke(new Action(delegate ()
            {
                btnReload.Text = icon;
            }));
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
            var con = new SQLiteConnection(cs);
            con.Open();
            var cmd = new SQLiteCommand(con);
            cmd.CommandText = "INSERT INTO history(title, url) VALUES(@title, @url)";
            cmd.Parameters.AddWithValue("@title", this.Text);
            cmd.Parameters.AddWithValue("@url", e.Address);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            con.Close();

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

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (browser.IsLoading)
            {
                browser.Stop();
            }
            else
            {
                browser.Reload();
            }
            
        }
    }
}
