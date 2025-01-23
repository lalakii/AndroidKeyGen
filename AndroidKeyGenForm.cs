using Org.BouncyCastle.Bcpg;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using static AndroidKeyGen.Program;

namespace AndroidKeyGen
{
    internal class AndroidKeyGenForm : LaUIForm.ILaUI
    {
        private readonly LaUIForm frm;

        private Button bt1;

        private ComboBox cb1;

        private ComboBox cb2;

        private Label lb1;

        private Label lb2;

        private Label lb3;

        private Label lb4;

        private Label lb5;

        private Label lb6;

        private DateTimePicker picker;

        private TextBox tb1;

        private TextBox tb2;

        public AndroidKeyGenForm()
        {
            frm = new LaUIForm(this)
            {
                WindowTitle = "Android KeyGen V1.4",
                ColorPrimary = Color.FromArgb(255, 164, 197, 57),
                BorderColor = Color.DarkGray,
                BorderVisible = true,
                ActiveControl = bt1,
            };
        }

        public Form GetForm()
        {
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
            lb1 = new() { Top = 21, Left = left, Text = "算法/Algorithm：", AutoSize = true };
            lb2 = new() { Top = lb1.Top + lb1.Height + 10, Left = left, AutoSize = true, Text = "有效期/Validity period：" };
            lb3 = new() { Top = lb2.Top + lb2.Height + 10, Left = left, AutoSize = true, Text = "名称/Name：" };
            lb4 = new() { Top = lb3.Top + lb3.Height + 10, Left = left, AutoSize = true, Text = "密码/Password：" };
            lb5 = new() { Top = lb1.Top + lb1.Height + 10, AutoSize = true, Text = "年/Years" };
            lb6 = new() { Top = lb4.Top + lb4.Height + 10, Left = left, AutoSize = true, Text = "起始日期/Start date：" };
            picker = new() { Format = DateTimePickerFormat.Custom, CustomFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.UniversalSortableDateTimePattern };
            content.Controls.AddRange([picker, lb1, lb2, lb3, lb4, lb5, lb6]);
            cb1 = new()
            {
                Left = lb1.Left + lb1.Width + 65,
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
            bt1 = new() { Text = "生成/Generate", AutoSize = true, TextAlign = ContentAlignment.MiddleCenter, Width = 167, Height = 42 };
            content.Controls.AddRange([cb1, cb2, tb1, tb2, bt1]);
        }

        public void OnLayout(Panel content)
        {
            cb1.Top = ((lb1.Height - cb1.Height) / 2) + lb1.Top;
            cb2.Top = ((lb2.Height - cb2.Height) / 2) + lb2.Top;
            tb1.Top = ((lb3.Height - tb1.Height) / 2) + lb3.Top;
            tb2.Top = ((lb3.Height - tb2.Height) / 2) + lb4.Top;
            lb5.Left = cb2.Left + cb2.Width;
            picker.Top = ((lb6.Height - picker.Height) / 2) + lb6.Top;
            picker.Left = tb1.Left;
            picker.Width = tb1.Width + lb5.Width;
            bt1.Top = picker.Top + picker.Height + 5;
            content.Width = lb5.Width + lb5.Left + 14;
            content.Height = bt1.Top + bt1.Height + 10;
            bt1.Left = (content.Width - bt1.Width) / 2;
            bt1.Click += Bt1_Click;
        }

        private void Bt1_Click(object sender, EventArgs e)
        {
            Enum.TryParse(cb1.SelectedItem.ToString(), out AlgorithmType getType);
            int.TryParse(cb2.SelectedItem.ToString(), out int getYear);
            var getAlias = tb1.Text;
            var getPwd = tb2.Text;
            if (string.IsNullOrEmpty(getAlias) || string.IsNullOrEmpty(getPwd))
            {
                MessageBox.Show("名称或密码不能为空", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var errCode = JksUtils.CreateKeyStore(getType, Math.Abs(getYear), getAlias.Replace("=", "-"), getPwd, picker.Value);
            var msg = "创建失败！";
            var icon = MessageBoxIcon.Error;
            if (errCode == 0)
            {
                icon = MessageBoxIcon.Information;
                msg = string.Format("Android签名证书生成成功\r\n\r\n算法：{0}\r\n有效期：{1}年\r\n名称：{2}\r\n密码：{3}\r\n起始日期：{4}\r\n\n证书文件存储在程序运行路径: {5}", getType, getYear, getAlias, getPwd, picker.Value, Program.wd);
            }

            MessageBox.Show(msg, null, MessageBoxButtons.OK, icon);
        }
    }
}