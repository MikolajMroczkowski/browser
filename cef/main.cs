using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using EasyTabs;

namespace cef
{
    public partial class main : TitleBarTabs
    {
        public main()
        {
            InitializeComponent();
            var settings = new CefSettings();

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = InternalProtocolFactory.SchemeName,
                SchemeHandlerFactory = new InternalProtocolFactory()
            });
            Cef.Initialize(settings);
            AeroPeekEnabled = true;
            TabRenderer = new ChromeTabRenderer(this);
        }

        private void main_Load(object sender, EventArgs e)
        {

        }
        public override TitleBarTab CreateTab()
        {
            return new TitleBarTab(this)
            {
                Content = new frmMain()
            };
        }
    }
}
