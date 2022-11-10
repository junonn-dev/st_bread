namespace st_bread
{
    partial class frmNFC
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
            this.lbl_Info = new System.Windows.Forms.Label();
            this.txt_NFC = new System.Windows.Forms.TextBox();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::st_bread.Properties.Resources.nfc_tag;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Location = new System.Drawing.Point(12, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(403, 438);
            this.panel1.TabIndex = 6;
            // 
            // lbl_Info
            // 
            this.lbl_Info.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_Info.Font = new System.Drawing.Font("나눔고딕", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Info.Location = new System.Drawing.Point(0, 0);
            this.lbl_Info.Name = "lbl_Info";
            this.lbl_Info.Size = new System.Drawing.Size(429, 88);
            this.lbl_Info.TabIndex = 5;
            this.lbl_Info.Text = "label1";
            this.lbl_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_NFC
            // 
            this.txt_NFC.Location = new System.Drawing.Point(12, 24);
            this.txt_NFC.Name = "txt_NFC";
            this.txt_NFC.Size = new System.Drawing.Size(260, 21);
            this.txt_NFC.TabIndex = 4;
            // 
            // btn_Exit
            // 
            this.btn_Exit.BackgroundImage = global::st_bread.Properties.Resources.bg_btntype03;
            this.btn_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Exit.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Exit.ForeColor = System.Drawing.Color.White;
            this.btn_Exit.Location = new System.Drawing.Point(166, 535);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(106, 44);
            this.btn_Exit.TabIndex = 10;
            this.btn_Exit.Text = "나 가 기";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // frmNFC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(429, 591);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbl_Info);
            this.Controls.Add(this.txt_NFC);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "frmNFC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmNFC";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNFC_FormClosing);
            this.Load += new System.EventHandler(this.frmNFC_Load);
            this.Shown += new System.EventHandler(this.frmNFC_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmNFC_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_Info;
        private System.Windows.Forms.TextBox txt_NFC;
        private System.Windows.Forms.Button btn_Exit;
    }
}