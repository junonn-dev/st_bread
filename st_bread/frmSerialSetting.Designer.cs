namespace st_bread
{
    partial class frmSerialSetting
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Login = new System.Windows.Forms.Button();
            this.lst_Set = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_Cancel);
            this.panel1.Controls.Add(this.btn_Login);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 532);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 107);
            this.panel1.TabIndex = 26;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Cancel.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_Cancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Cancel.Location = new System.Drawing.Point(206, 0);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(199, 107);
            this.btn_Cancel.TabIndex = 4;
            this.btn_Cancel.Text = "나가기";
            this.btn_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Login
            // 
            this.btn_Login.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn_Login.FlatAppearance.BorderSize = 0;
            this.btn_Login.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Login.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Login.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_Login.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Login.Location = new System.Drawing.Point(0, 0);
            this.btn_Login.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(206, 107);
            this.btn_Login.TabIndex = 3;
            this.btn_Login.Text = "선 택";
            this.btn_Login.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // lst_Set
            // 
            this.lst_Set.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lst_Set.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lst_Set.Font = new System.Drawing.Font("나눔고딕", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lst_Set.FullRowSelect = true;
            this.lst_Set.GridLines = true;
            this.lst_Set.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lst_Set.Location = new System.Drawing.Point(0, 0);
            this.lst_Set.MultiSelect = false;
            this.lst_Set.Name = "lst_Set";
            this.lst_Set.Size = new System.Drawing.Size(405, 532);
            this.lst_Set.TabIndex = 27;
            this.lst_Set.UseCompatibleStateImageBehavior = false;
            this.lst_Set.View = System.Windows.Forms.View.Details;
            this.lst_Set.DoubleClick += new System.EventHandler(this.lst_Set_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 395;
            // 
            // frmSerialSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(49)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(405, 639);
            this.Controls.Add(this.lst_Set);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSerialSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmSerialSetting";
            this.Load += new System.EventHandler(this.frmSerialSetting_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.ListView lst_Set;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}