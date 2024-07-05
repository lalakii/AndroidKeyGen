namespace AndroidKeyGen
{
    partial class AndroidKeyGen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.keygen = new System.Windows.Forms.Button();
            this.type = new System.Windows.Forms.ComboBox();
            this.alias = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.year = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pwd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.git = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.git)).BeginInit();
            this.SuspendLayout();
            // 
            // keygen
            // 
            this.keygen.Location = new System.Drawing.Point(190, 219);
            this.keygen.Name = "keygen";
            this.keygen.Size = new System.Drawing.Size(144, 39);
            this.keygen.TabIndex = 0;
            this.keygen.Text = "创建/Generate";
            this.keygen.UseVisualStyleBackColor = true;
            this.keygen.Click += new System.EventHandler(this.Keygen_Click);
            // 
            // type
            // 
            this.type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.type.FormattingEnabled = true;
            this.type.Items.AddRange(new object[] {
            "RSA",
            "EC",
            "DSA"});
            this.type.Location = new System.Drawing.Point(229, 29);
            this.type.Name = "type";
            this.type.Size = new System.Drawing.Size(180, 23);
            this.type.TabIndex = 1;
            // 
            // alias
            // 
            this.alias.Location = new System.Drawing.Point(229, 115);
            this.alias.Name = "alias";
            this.alias.Size = new System.Drawing.Size(180, 25);
            this.alias.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "名称/Name：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "算法/Algorithm：";
            // 
            // year
            // 
            this.year.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.year.FormattingEnabled = true;
            this.year.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "20",
            "30",
            "40",
            "50",
            "60",
            "70",
            "80"});
            this.year.Location = new System.Drawing.Point(229, 72);
            this.year.Name = "year";
            this.year.Size = new System.Drawing.Size(180, 23);
            this.year.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(195, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "有效期/Validity period：";
            // 
            // pwd
            // 
            this.pwd.Location = new System.Drawing.Point(229, 160);
            this.pwd.Name = "pwd";
            this.pwd.Size = new System.Drawing.Size(180, 25);
            this.pwd.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "密码/Password：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(415, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "年/Years";
            // 
            // git
            // 
            this.git.Cursor = System.Windows.Forms.Cursors.Hand;
            this.git.Image = global::AndroidKeyGen.Properties.Resources.github_mark;
            this.git.Location = new System.Drawing.Point(433, 208);
            this.git.Name = "git";
            this.git.Size = new System.Drawing.Size(52, 50);
            this.git.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.git.TabIndex = 10;
            this.git.TabStop = false;
            this.git.Click += new System.EventHandler(this.Git_Click);
            // 
            // AndroidKeyGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Thistle;
            this.ClientSize = new System.Drawing.Size(524, 284);
            this.Controls.Add(this.git);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.year);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.type);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.alias);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.keygen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AndroidKeyGen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AndroidKeyGen v1.2";
            this.SizeChanged += new System.EventHandler(this.AndroidKeyGen_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.git)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button keygen;
        private System.Windows.Forms.ComboBox type;
        private System.Windows.Forms.TextBox alias;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox year;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox pwd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox git;
    }
}