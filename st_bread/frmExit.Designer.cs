namespace st_bread
{
    partial class frmExit
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
            this.lbl_Message = new System.Windows.Forms.Label();
            this.pan_index = new System.Windows.Forms.Panel();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_WinRestart = new System.Windows.Forms.Button();
            this.btn_WinExit = new System.Windows.Forms.Button();
            this.btn_ProgExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_Message
            // 
            this.lbl_Message.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_Message.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 20.25F, System.Drawing.FontStyle.Bold);
            this.lbl_Message.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbl_Message.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lbl_Message.Location = new System.Drawing.Point(0, 529);
            this.lbl_Message.Name = "lbl_Message";
            this.lbl_Message.Size = new System.Drawing.Size(630, 57);
            this.lbl_Message.TabIndex = 28;
            this.lbl_Message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pan_index
            // 
            this.pan_index.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.pan_index.Location = new System.Drawing.Point(494, 45);
            this.pan_index.Margin = new System.Windows.Forms.Padding(0);
            this.pan_index.Name = "pan_index";
            this.pan_index.Size = new System.Drawing.Size(12, 82);
            this.pan_index.TabIndex = 27;
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 20.25F, System.Drawing.FontStyle.Bold);
            this.btn_Cancel.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_Cancel.Image = global::st_bread.Properties.Resources.go_home_4;
            this.btn_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_Cancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_Cancel.Location = new System.Drawing.Point(167, 336);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(0);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(295, 82);
            this.btn_Cancel.TabIndex = 32;
            this.btn_Cancel.Text = "ESC-취소";
            this.btn_Cancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_WinRestart
            // 
            this.btn_WinRestart.FlatAppearance.BorderSize = 0;
            this.btn_WinRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_WinRestart.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 20.25F, System.Drawing.FontStyle.Bold);
            this.btn_WinRestart.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_WinRestart.Image = global::st_bread.Properties.Resources.view_refresh_3;
            this.btn_WinRestart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_WinRestart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_WinRestart.Location = new System.Drawing.Point(167, 239);
            this.btn_WinRestart.Margin = new System.Windows.Forms.Padding(0);
            this.btn_WinRestart.Name = "btn_WinRestart";
            this.btn_WinRestart.Size = new System.Drawing.Size(295, 82);
            this.btn_WinRestart.TabIndex = 31;
            this.btn_WinRestart.Text = "3-시스템 재시작";
            this.btn_WinRestart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_WinRestart.UseVisualStyleBackColor = true;
            this.btn_WinRestart.Click += new System.EventHandler(this.btn_WinRestart_Click);
            // 
            // btn_WinExit
            // 
            this.btn_WinExit.FlatAppearance.BorderSize = 0;
            this.btn_WinExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_WinExit.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 20.25F, System.Drawing.FontStyle.Bold);
            this.btn_WinExit.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_WinExit.Image = global::st_bread.Properties.Resources.system_shutdown_6;
            this.btn_WinExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_WinExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_WinExit.Location = new System.Drawing.Point(167, 142);
            this.btn_WinExit.Margin = new System.Windows.Forms.Padding(0);
            this.btn_WinExit.Name = "btn_WinExit";
            this.btn_WinExit.Size = new System.Drawing.Size(295, 82);
            this.btn_WinExit.TabIndex = 30;
            this.btn_WinExit.Text = "2-시스템 종료";
            this.btn_WinExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_WinExit.UseVisualStyleBackColor = true;
            this.btn_WinExit.Click += new System.EventHandler(this.btn_WinExit_Click);
            // 
            // btn_ProgExit
            // 
            this.btn_ProgExit.FlatAppearance.BorderSize = 0;
            this.btn_ProgExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_ProgExit.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 20.25F, System.Drawing.FontStyle.Bold);
            this.btn_ProgExit.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_ProgExit.Image = global::st_bread.Properties.Resources.view_restore;
            this.btn_ProgExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_ProgExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn_ProgExit.Location = new System.Drawing.Point(167, 45);
            this.btn_ProgExit.Margin = new System.Windows.Forms.Padding(0);
            this.btn_ProgExit.Name = "btn_ProgExit";
            this.btn_ProgExit.Size = new System.Drawing.Size(295, 82);
            this.btn_ProgExit.TabIndex = 29;
            this.btn_ProgExit.Text = "1-프로그램종료";
            this.btn_ProgExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btn_ProgExit.UseVisualStyleBackColor = true;
            this.btn_ProgExit.Click += new System.EventHandler(this.btn_ProgExit_Click);
            // 
            // frmExit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(49)))), ((int)(((byte)(60)))));
            this.ClientSize = new System.Drawing.Size(630, 586);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_WinRestart);
            this.Controls.Add(this.btn_WinExit);
            this.Controls.Add(this.btn_ProgExit);
            this.Controls.Add(this.lbl_Message);
            this.Controls.Add(this.pan_index);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmExit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmExit";
            this.Load += new System.EventHandler(this.frmExit_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmExit_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Message;
        private System.Windows.Forms.Panel pan_index;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_WinRestart;
        private System.Windows.Forms.Button btn_WinExit;
        private System.Windows.Forms.Button btn_ProgExit;
    }
}