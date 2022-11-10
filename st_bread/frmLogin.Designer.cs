namespace st_bread
{
    partial class frmLogin
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.pic_Cancel = new System.Windows.Forms.PictureBox();
            this.pic_login = new System.Windows.Forms.PictureBox();
            this.lbl_Version = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_login)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_Cancel
            // 
            this.pic_Cancel.BackColor = System.Drawing.Color.Transparent;
            this.pic_Cancel.Image = global::st_bread.Properties.Resources.btn_cancel02;
            this.pic_Cancel.Location = new System.Drawing.Point(580, 245);
            this.pic_Cancel.Name = "pic_Cancel";
            this.pic_Cancel.Size = new System.Drawing.Size(309, 120);
            this.pic_Cancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Cancel.TabIndex = 5;
            this.pic_Cancel.TabStop = false;
            this.pic_Cancel.Tag = "CANCEL";
            this.pic_Cancel.Click += new System.EventHandler(this.pic_Cancel_Click);
            // 
            // pic_login
            // 
            this.pic_login.BackColor = System.Drawing.Color.Transparent;
            this.pic_login.Image = global::st_bread.Properties.Resources.btn_login;
            this.pic_login.Location = new System.Drawing.Point(152, 245);
            this.pic_login.Name = "pic_login";
            this.pic_login.Size = new System.Drawing.Size(309, 120);
            this.pic_login.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_login.TabIndex = 4;
            this.pic_login.TabStop = false;
            this.pic_login.Tag = "LOGIN";
            this.pic_login.Click += new System.EventHandler(this.pic_login_Click);
            // 
            // lbl_Version
            // 
            this.lbl_Version.AutoSize = true;
            this.lbl_Version.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Version.Font = new System.Drawing.Font("나눔고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Version.ForeColor = System.Drawing.Color.White;
            this.lbl_Version.Location = new System.Drawing.Point(826, 17);
            this.lbl_Version.Name = "lbl_Version";
            this.lbl_Version.Size = new System.Drawing.Size(50, 17);
            this.lbl_Version.TabIndex = 6;
            this.lbl_Version.Text = "label1";
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::st_bread.Properties.Resources.bg_bg;
            this.ClientSize = new System.Drawing.Size(1024, 478);
            this.Controls.Add(this.lbl_Version);
            this.Controls.Add(this.pic_Cancel);
            this.Controls.Add(this.pic_login);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLogin";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_login)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_Cancel;
        private System.Windows.Forms.PictureBox pic_login;
        private System.Windows.Forms.Label lbl_Version;

    }
}

