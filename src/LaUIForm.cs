using System;
using System.Drawing;
using System.Reflection;
using System.Security.Principal;
using System.Windows.Forms;

namespace AndroidKeyGen
{
    [System.ComponentModel.DesignerCategory("")]
    public sealed class LaUIForm : Form
    {
        public static readonly bool IsChinese = System.Globalization.CultureInfo.InstalledUICulture.Name.IndexOf("zh-", StringComparison.OrdinalIgnoreCase) > -1;

        public LaUIForm(ILaUI events)
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.GhostWhite;
            StartPosition = FormStartPosition.CenterScreen;
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
            StartPosition = FormStartPosition.CenterScreen;
            Panel pn = new() { AutoSize = false, BackColor = Color.Transparent };
            Label bc = new() { Text = "×", AutoSize = true, BackColor = pn.BackColor, Font = new Font("Consolas", DefaultFont.Size * 1.88f) }, bm = new() { AutoSize = true, Text = "—", Font = new Font(bc.Font.Name, bc.Font.Size * 0.8f), BackColor = pn.BackColor }, bh = new() { AutoSize = true, Text = "?", BackColor = pn.BackColor, Font = bc.Font };
            Controls.AddRange([pn, bc, bm, bh]);
            bc.MouseEnter += (_, __) => bc.ForeColor = Color.IndianRed;
            bc.MouseLeave += (_, __) => bc.ForeColor = ForeColor;
            bc.Click += (_, __) =>
            {
                Hide();
                events.OnClose();
            };
            bm.MouseEnter += (_, __) => bm.ForeColor = ColorPrimary;
            bm.MouseLeave += (_, __) => bm.ForeColor = ForeColor;
            bm.Click += (_, __) => WindowState = FormWindowState.Minimized;
            bh.MouseEnter += (_, __) => bh.ForeColor = Color.DodgerBlue;
            bh.MouseLeave += (_, __) => bh.ForeColor = ForeColor;
            bh.Click += (_, __) => events.OnHelp();
            events.OnInit(pn);
            InitDefaultFont(this);
            Paint += (_, e) =>
            {
                Text = WindowTitle;
                var g = e.Graphics;
                g.DrawString(Text, new Font("Candara", 12f), new SolidBrush(ForeColor), 5, 7);
                using var p0 = new Pen(ColorPrimary, 7f);
                g.DrawLine(p0, Width, 0, 0, 0);
                if (BorderVisible)
                {
                    p0.Color = BorderColor;
                    p0.Width = 1f;
                    g.DrawRectangle(p0, 5, pn.Top + 5, Width - 13, Height - pn.Top - 11);
                }

                if (WindowState == FormWindowState.Maximized)
                {
                    WindowState = FormWindowState.Normal;
                }
            };
            Load += (_, _) =>
            {
                if (DarkMode)
                {
                    BackColor = Color.FromArgb(255, 30, 30, 30);
                    ForeColor = Color.White;
                }

                events.OnLayout(pn);
                pn.Top = bc.Height - 6;
                Width = pn.Width;
                Width += 2 * pn.Left;
                bc.Left = Width - bc.Width;
                bm.Left = bc.Left - bc.Width;
                bm.Top = Math.Abs(bc.Height - bm.Height) / 2;
                bh.Left = bm.Left - bc.Width - ((bc.Width - bm.Width) / 2);
                Height = pn.Top + pn.Height;
            };
        }

        public interface ILaUI
        {
            void OnClose();

            void OnHelp();

            void OnInit(Panel content);

            void OnLayout(Panel content);
        }

        public Color BorderColor { get; set; }

        public bool BorderVisible { get; set; }

        public Color ColorPrimary { get; set; }

        public bool DarkMode { get; set; }

        public string WindowTitle { get; set; }

        protected override CreateParams CreateParams
        {
            get
            {
                var p = base.CreateParams;
                p.Style &= 0x800000 | 0x20000;
                return p;
            }
        }

        public static bool IsAdministrator()
        {
            using var identity = WindowsIdentity.GetCurrent();
            return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x00A3)
            {
                return;
            }

            base.WndProc(ref m);
            if (m.Msg == 0x84)
            {
                m.Result = (IntPtr)0x2;
            }
        }

        private void InitDefaultFont(Control parent)
        {
            foreach (Control it in parent.Controls)
            {
                if (it.BackColor != Color.Transparent)
                {
                    it.Font = new Font("Segoe UI", it.Font.Size);
                }

                var cs = it.Controls;
                if (cs?.Count > 0)
                {
                    InitDefaultFont(it);
                }
            }
        }
    }
}