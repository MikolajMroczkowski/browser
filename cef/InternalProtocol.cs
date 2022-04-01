using System;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CefSharp;

namespace cef
{
    public class InternalProtocol:ResourceHandler
    {
        private string frontendFolderPath;
        string cs = $@"URI=file:{AppDomain.CurrentDomain.BaseDirectory}\browser.db";
        public InternalProtocol()
        {
            frontendFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./pages/");
        }
        public override CefReturnValue ProcessRequestAsync(IRequest request, ICallback callback)
        {
            var uri = new Uri(request.Url);
            var args = uri.ToString().Split('/');
            var mode = args[2];
            switch (mode)
            {
                case "history":
                    var elements = "";
                    try
                    {
                        var con = new SQLiteConnection(cs);
                        con.Open();
                        string stm = "SELECT * FROM history ORDER BY lastVisit DESC LIMIT 10000";
                        var cmd = new SQLiteCommand(stm, con);
                        SQLiteDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            if (rdr.GetString(0) != rdr.GetString(1)+"/")
                            {
                                elements += $"<p><a href=\"{rdr.GetString(0)}\" target=\"_blank\">{rdr.GetString(1)} ({rdr.GetString(2)})</a></p><br>";
                            }
                            
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    var frame = File.ReadAllText(frontendFolderPath+"history.html");
                    string content = frame.Replace("CONTENT", elements);
                    byte[] bytes = Encoding.ASCII.GetBytes(content);
                    Stream = new MemoryStream(bytes);
                    callback.Continue();
                    return CefReturnValue.Continue;
                    break;
                default:
                    return CefReturnValue.Continue;
                    break;
            }
            
            
        }
        
    }
    public class InternalProtocolFactory : ISchemeHandlerFactory
    {
        public const string SchemeName = "browser";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new InternalProtocol();
        }
    }
}