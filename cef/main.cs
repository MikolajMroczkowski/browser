using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyTabs;

namespace cef
{
    public partial class main : TitleBarTabs
    {
        public main()
        {
            InitializeComponent();
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
