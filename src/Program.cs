using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Windows.Forms;

namespace AndroidKeyGen
{
    public static class Program
    {
        public static string Wd { get; set; }

        [STAThread]
        private static void Main()
        {
            var main = Assembly.GetExecutingAssembly();
            Wd = Path.GetDirectoryName(main.Location);
            Image logo = null;
            foreach (var resName in main.GetManifestResourceNames())
            {
                var res = main.GetManifestResourceStream(resName);
                if (resName.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
                {
                    using (res)
                    {
                        using GZipStream gzDll = new(res, CompressionMode.Decompress);
                        using MemoryStream ms = new();
                        gzDll.CopyTo(ms);
                        AppDomain.CurrentDomain.AssemblyResolve += (_, __) => Assembly.Load(ms.GetBuffer());
                    }
                }
                else if (resName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    logo = Image.FromStream(res);
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AndroidKeyGenForm().GetForm(logo));
        }
    }
}