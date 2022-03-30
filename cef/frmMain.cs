using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        private void frmMain_Load(object sender, EventArgs e)
        {
            var settings = new CefSettings();

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: @"C:\CEFSharpExample\webpage",
                    hostName: "cefsharp",
                    defaultPage: "index.html" // will default to index.html
                )
            });

            Cef.Initialize(settings);
            browser = new ChromiumWebBrowser("chrome://settings");
            browser.Dock= DockStyle.Fill;
            this.panelRender.Controls.Add(browser);
            browser.LoadError += Browser_LoadError;
            
        }

        private void Browser_LoadError(object sender, CefSharp.LoadErrorEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
