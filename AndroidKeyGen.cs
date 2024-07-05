using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static AndroidKeyGen.Program;

namespace AndroidKeyGen
{
    public partial class AndroidKeyGen : Form
    {
        public AndroidKeyGen()
        {
            InitializeComponent();
            Icon = SystemIcons.Application;
            type.SelectedIndex = 0;
            year.SelectedIndex = 0;
        }

        private void Keygen_Click(object sender, EventArgs e)
        {
            var jksUtil = new JksUtils();
            Enum.TryParse(type.SelectedItem.ToString(), out AlgorithmType getType);
            int.TryParse(year.SelectedItem.ToString(), out int getYear);
            var getAlias = alias.Text;
            var getPwd = pwd.Text;
            if (string.IsNullOrEmpty(getAlias) || string.IsNullOrEmpty(getPwd))
            {
                MessageBox.Show("名称或密码不能为空");
                return;
            }
            var errCode = jksUtil.CreateKeyStore(getType, Math.Abs(getYear), getAlias, getPwd);
            var msg = "创建失败！";
            if (errCode == 0)
            {
                msg = string.Format("Android签名证书创建成功\r\n\r\n算法：{0}\r\n有效期：{1}年\r\n名称：{2}\r\n密码：{3}\r\n\r\n证书文件存储在程序运行路径", getType, getYear, getAlias, getPwd);
            }
            MessageBox.Show(msg);
        }

        private void Git_Click(object sender, EventArgs e)
        {
#pragma warning disable S1075 // URIs should not be hardcoded
            _ = Process.Start("https://github.com/lalakii");
#pragma warning restore S1075 // URIs should not be hardcoded
        }

        private void AndroidKeyGen_SizeChanged(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Normal && WindowState != FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
        }
    }
}