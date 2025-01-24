using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AndroidKeyGen
{
    internal class AndroidKeyGenForm : LaUIForm.ILaUI
    {
        private readonly LaUIForm frm;
        private readonly bool isCN = LaUIForm.IsChinese;
        private Button bt1;
        private ComboBox cb1;
        private ComboBox cb2;
        private Label lb1;
        private Label lb2;
        private Label lb3;
        private Label lb4;
        private Label lb5;
        private Label lb6;
        private Image logo;
        private PictureBox pb;
        private DateTimePicker picker;
        private TextBox tb1;

        private TextBox tb2;

        public AndroidKeyGenForm()
        {
            frm = new LaUIForm(this)
            {
                WindowTitle = "Android KeyGen V1.5",
                ColorPrimary = Color.FromArgb(255, 76, 175, 80),
                BorderColor = Color.DarkGray,
                BorderVisible = true,
                ActiveControl = bt1,
            };
        }

        public Form GetForm(Image logo)
        {
            this.logo = logo;
            return frm;
        }

        public void OnClose()
        {
            Environment.Exit(0);
        }

        public void OnHelp()
        {
            try
            {
                Process.Start("https://github.com/lalakii/AndroidKeyGen");
            }
            catch
            {
            }
        }

        public void OnInit(Panel content)
        {
            const int left = 12;
            lb1 = new() { Top = 18, Left = left, Text = isCN ? "算法:" : "Algorithm:", AutoSize = true };
            lb2 = new() { Top = lb1.Top + lb1.Height + 10, Left = left, AutoSize = true, Text = isCN ? "有效期:" : "Validity period:" };
            lb3 = new() { Top = lb2.Top + lb2.Height + 10, Left = left, AutoSize = true, Text = isCN ? "名称:" : "Name:" };
            lb4 = new() { Top = lb3.Top + lb3.Height + 10, Left = left, AutoSize = true, Text = isCN ? "密码:" : "Password:" };
            lb5 = new() { Top = lb1.Top + lb1.Height + 10, AutoSize = true, Text = isCN ? "年" : "Years" };
            lb6 = new() { Top = lb4.Top + lb4.Height + 10, Left = left, AutoSize = true, Text = isCN ? "起始日期:" : "Start date:" };
            picker = new() { Format = DateTimePickerFormat.Custom, CustomFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern };
            pb = new() { SizeMode = PictureBoxSizeMode.Zoom, Height = 50 };
            content.Controls.AddRange([pb, picker, lb1, lb2, lb3, lb4, lb5, lb6]);
            cb1 = new()
            {
                Left = isCN ? (lb1.Left + (lb2.Width * 3)) : (int)(lb1.Left + (lb2.Width * 1.5)),
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            cb2 = new()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Left = cb1.Left,
            };
            cb1.Items.AddRange(["RSA", "EC", "DSA"]);
            for (int i = 1; i < 101; i++)
            {
                cb2.Items.Add(i);
            }

            cb2.SelectedIndex = 0;
            cb1.SelectedIndex = 0;
            tb1 = new() { Left = cb1.Left, Width = cb1.Width };
            tb2 = new() { Left = cb1.Left, Width = cb1.Width, UseSystemPasswordChar = true };
            bt1 = new() { Text = isCN ? "生成" : "Generate", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Width = 137, Height = 28 };
            content.Controls.AddRange([cb1, cb2, tb1, tb2, bt1]);
        }

        public void OnLayout(Panel content)
        {
            cb1.Top = ((lb1.Height - cb1.Height) / 2) + lb1.Top;
            cb2.Top = ((lb2.Height - cb2.Height) / 2) + lb2.Top;
            tb1.Top = ((lb3.Height - tb1.Height) / 2) + lb3.Top;
            tb2.Top = ((lb3.Height - tb2.Height) / 2) + lb4.Top;
            lb5.Left = cb2.Left + cb2.Width + 4;
            picker.Top = ((lb6.Height - picker.Height) / 2) + lb6.Top;
            picker.Width = lb6.Width + tb1.Width;
            picker.Left = tb1.Left - (picker.Width - cb1.Width);
            bt1.Top = picker.Top + picker.Height + 12;
            content.Width = lb5.Width + lb5.Left + 12;
            content.Height = bt1.Top + bt1.Height + 17;
            bt1.Left = (content.Width - bt1.Width) / 2;
            bt1.Click += Bt1_Click;
            bt1.Font = new Font(picker.Font.FontFamily, bt1.Font.Size * 1.2f);
            pb.Image = logo;
            pb.Top = bt1.Top - pb.Height + bt1.Height;
        }

        private void Bt1_Click(object sender, EventArgs e)
        {
            Enum.TryParse(cb1.SelectedItem.ToString(), out CertUtils.AlgorithmType getType);
            int.TryParse(cb2.SelectedItem.ToString(), out int getYear);
            var getAlias = tb1.Text;
            var getPwd = tb2.Text;
            if (string.IsNullOrEmpty(getAlias) || string.IsNullOrEmpty(getPwd))
            {
                MessageBox.Show(isCN ? "名称或密码不能为空" : "Name or password cannot be empty", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CreateKeyStore(getType, Math.Abs(getYear), getAlias.Replace("=", "-"), getPwd, picker.Value) == 0)
            {
                const string cnMsg = "Android签名证书生成成功\r\n\r\n算法：{0}\r\n有效期：{1}年\r\n名称：{2}\r\n密码：{3}\r\n起始日期：{4}\r\n";
                const string enMsg = "Android Signature Certificate Generated Successfully\r\n\r\nAlgorithm: {0}\r\nValidity: {1}year\r\nName: {2}\r\nPassword: {3}\r\nStart Date: {4}\r\n";
                MessageBox.Show(string.Format(isCN ? cnMsg : enMsg, getType, getYear, getAlias, getPwd, picker.Value), string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private int CreateKeyStore(CertUtils.AlgorithmType type, int year, string alias, string pwd, DateTime date)
        {
            SaveFileDialog sd = new() { Filter = "KeyStore (.jks)|*.jks", FileName = $"{alias}" };
            if (sd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using var fs = File.Create(sd.FileName);
                    int ret = CertUtils.Create(fs, type, year, alias, pwd, date);
                    if (ret != 0)
                    {
                        const string enMsg = "Certificate creation failure!";
                        const string cnMsg = "证书创建失败!";
                        MessageBox.Show($"{(isCN ? cnMsg : enMsg)}({ret})", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    MessageBox.Show(isCN ? "路径无法写入，请尝试更改名称或在其他路径保存!" : "Path can't be written, try changing the filename or saving in another path!", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return -1;
        }
    }
}