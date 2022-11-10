namespace st_bread
{
    partial class frmMessage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMessage));
            this.lbl_Message = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_Message3 = new System.Windows.Forms.Label();
            this.lbl_Message2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_Check = new System.Windows.Forms.PictureBox();
            this.pic_Exit = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Check)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Exit)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_Message
            // 
            this.lbl_Message.AutoSize = true;
            this.lbl_Message.Font = new System.Drawing.Font("나눔고딕", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Message.Location = new System.Drawing.Point(493, 106);
            this.lbl_Message.Name = "lbl_Message";
            this.lbl_Message.Size = new System.Drawing.Size(515, 55);
            this.lbl_Message.TabIndex = 0;
            this.lbl_Message.Text = "주문이 완료 되었습니다.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(72, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 43);
            this.label3.TabIndex = 8;
            this.label3.Text = "안내";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lbl_Message3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lbl_Message2);
            this.panel1.Controls.Add(this.btn_Check);
            this.panel1.Controls.Add(this.lbl_Message);
            this.panel1.Location = new System.Drawing.Point(11, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1057, 520);
            this.panel1.TabIndex = 9;
            // 
            // lbl_Message3
            // 
            this.lbl_Message3.AutoSize = true;
            this.lbl_Message3.Font = new System.Drawing.Font("나눔고딕", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Message3.Location = new System.Drawing.Point(493, 28);
            this.lbl_Message3.Name = "lbl_Message3";
            this.lbl_Message3.Size = new System.Drawing.Size(515, 55);
            this.lbl_Message3.TabIndex = 9;
            this.lbl_Message3.Text = "주문이 완료 되었습니다.";
            // 
            // lbl_Message2
            // 
            this.lbl_Message2.AutoSize = true;
            this.lbl_Message2.Font = new System.Drawing.Font("나눔고딕", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Message2.Location = new System.Drawing.Point(493, 184);
            this.lbl_Message2.Name = "lbl_Message2";
            this.lbl_Message2.Size = new System.Drawing.Size(515, 55);
            this.lbl_Message2.TabIndex = 7;
            this.lbl_Message2.Text = "주문이 완료 되었습니다.";
            // 
            // pictureBox2
            // 
            //this.pictureBox2.Image = global::st_bread.Properties.Resources._5874_info_sign_clip_art;
            this.pictureBox2.Location = new System.Drawing.Point(11, 7);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(55, 48);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            //this.pictureBox1.Image = global::st_bread.Properties.Resources.sorry;
            this.pictureBox1.Location = new System.Drawing.Point(15, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(346, 319);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // btn_Check
            // 
            this.btn_Check.Image = global::st_bread.Properties.Resources.btn_check;
            this.btn_Check.Location = new System.Drawing.Point(324, 367);
            this.btn_Check.Name = "btn_Check";
            this.btn_Check.Size = new System.Drawing.Size(385, 120);
            this.btn_Check.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Check.TabIndex = 6;
            this.btn_Check.TabStop = false;
            this.btn_Check.Click += new System.EventHandler(this.btn_Check_Click);
            // 
            // pic_Exit
            // 
            this.pic_Exit.Image = global::st_bread.Properties.Resources.btn_close1;
            this.pic_Exit.Location = new System.Drawing.Point(1021, 7);
            this.pic_Exit.Name = "pic_Exit";
            this.pic_Exit.Size = new System.Drawing.Size(47, 45);
            this.pic_Exit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Exit.TabIndex = 7;
            this.pic_Exit.TabStop = false;
            this.pic_Exit.Click += new System.EventHandler(this.pic_Exit_Click);
            // 
            // frmMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1080, 597);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pic_Exit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "2";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Check)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Exit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pic_Exit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox btn_Check;
        public System.Windows.Forms.Label lbl_Message;
        public System.Windows.Forms.Label lbl_Message2;
        public System.Windows.Forms.Label lbl_Message3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}