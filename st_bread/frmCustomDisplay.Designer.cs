namespace st_bread
{
    partial class frmCustomDisplay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCustomDisplay));
            this.lst_Items = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pan_Show = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbl_Rest = new System.Windows.Forms.Label();
            this.lbl_CustomPay = new System.Windows.Forms.Label();
            this.lbl_SalePrice = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_tot = new System.Windows.Forms.Label();
            this.pictureBox20 = new System.Windows.Forms.PictureBox();
            this.pan_Cong = new System.Windows.Forms.Panel();
            this.lbl_AccNum = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Media_Player = new AxWMPLib.AxWindowsMediaPlayer();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pan_Show.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).BeginInit();
            this.pan_Cong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Media_Player)).BeginInit();
            this.SuspendLayout();
            // 
            // lst_Items
            // 
            this.lst_Items.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lst_Items.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lst_Items.Font = new System.Drawing.Font("나눔고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lst_Items.FullRowSelect = true;
            this.lst_Items.GridLines = true;
            this.lst_Items.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lst_Items.Location = new System.Drawing.Point(3, 30);
            this.lst_Items.MultiSelect = false;
            this.lst_Items.Name = "lst_Items";
            this.lst_Items.Size = new System.Drawing.Size(427, 504);
            this.lst_Items.TabIndex = 1;
            this.lst_Items.UseCompatibleStateImageBehavior = false;
            this.lst_Items.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "번호";
            this.columnHeader1.Width = 39;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "상품코드";
            this.columnHeader2.Width = 0;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "상품명";
            this.columnHeader3.Width = 183;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "수량";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 38;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "단가";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader5.Width = 83;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "금액";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader6.Width = 83;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "gr_cd";
            this.columnHeader7.Width = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pan_Show);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.pictureBox20);
            this.panel1.Controls.Add(this.lst_Items);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1024, 768);
            this.panel1.TabIndex = 16;
            // 
            // pictureBox1
            // 
            //this.pictureBox1.Image = global::st_bread.Properties.Resources.빠아앙가로고;
            this.pictureBox1.Location = new System.Drawing.Point(46, 572);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(370, 173);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // pan_Show
            // 
            this.pan_Show.Controls.Add(this.Media_Player);
            this.pan_Show.Location = new System.Drawing.Point(434, 0);
            this.pan_Show.Name = "pan_Show";
            this.pan_Show.Size = new System.Drawing.Size(587, 463);
            this.pan_Show.TabIndex = 16;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.BackgroundImage = global::st_bread.Properties.Resources.bg_totalbox;
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel4.Controls.Add(this.lbl_Rest);
            this.panel4.Controls.Add(this.lbl_CustomPay);
            this.panel4.Controls.Add(this.lbl_SalePrice);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.lbl_tot);
            this.panel4.Location = new System.Drawing.Point(436, 469);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(576, 296);
            this.panel4.TabIndex = 15;
            // 
            // lbl_Rest
            // 
            this.lbl_Rest.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Rest.Font = new System.Drawing.Font("나눔고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Rest.ForeColor = System.Drawing.Color.White;
            this.lbl_Rest.Location = new System.Drawing.Point(227, 232);
            this.lbl_Rest.Name = "lbl_Rest";
            this.lbl_Rest.Size = new System.Drawing.Size(308, 37);
            this.lbl_Rest.TabIndex = 7;
            this.lbl_Rest.Text = "0";
            this.lbl_Rest.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_CustomPay
            // 
            this.lbl_CustomPay.BackColor = System.Drawing.Color.Transparent;
            this.lbl_CustomPay.Font = new System.Drawing.Font("나눔고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_CustomPay.ForeColor = System.Drawing.Color.White;
            this.lbl_CustomPay.Location = new System.Drawing.Point(227, 160);
            this.lbl_CustomPay.Name = "lbl_CustomPay";
            this.lbl_CustomPay.Size = new System.Drawing.Size(308, 37);
            this.lbl_CustomPay.TabIndex = 6;
            this.lbl_CustomPay.Text = "0";
            this.lbl_CustomPay.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_SalePrice
            // 
            this.lbl_SalePrice.BackColor = System.Drawing.Color.Transparent;
            this.lbl_SalePrice.Font = new System.Drawing.Font("나눔고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_SalePrice.ForeColor = System.Drawing.Color.White;
            this.lbl_SalePrice.Location = new System.Drawing.Point(227, 95);
            this.lbl_SalePrice.Name = "lbl_SalePrice";
            this.lbl_SalePrice.Size = new System.Drawing.Size(308, 37);
            this.lbl_SalePrice.TabIndex = 5;
            this.lbl_SalePrice.Text = "0";
            this.lbl_SalePrice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("나눔고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(37, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 40);
            this.label4.TabIndex = 4;
            this.label4.Text = "거스름돈";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("나눔고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(37, 164);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(149, 40);
            this.label3.TabIndex = 3;
            this.label3.Text = "받은금액";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(37, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 40);
            this.label2.TabIndex = 2;
            this.label2.Text = "할인금액";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("나눔고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(37, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "주문금액";
            // 
            // lbl_tot
            // 
            this.lbl_tot.BackColor = System.Drawing.Color.Transparent;
            this.lbl_tot.Font = new System.Drawing.Font("나눔고딕", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_tot.ForeColor = System.Drawing.Color.White;
            this.lbl_tot.Location = new System.Drawing.Point(227, 23);
            this.lbl_tot.Name = "lbl_tot";
            this.lbl_tot.Size = new System.Drawing.Size(308, 37);
            this.lbl_tot.TabIndex = 0;
            this.lbl_tot.Text = "0";
            this.lbl_tot.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox20
            // 
            this.pictureBox20.Image = global::st_bread.Properties.Resources.img_th;
            this.pictureBox20.Location = new System.Drawing.Point(3, 4);
            this.pictureBox20.Name = "pictureBox20";
            this.pictureBox20.Size = new System.Drawing.Size(427, 32);
            this.pictureBox20.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox20.TabIndex = 14;
            this.pictureBox20.TabStop = false;
            // 
            // pan_Cong
            // 
            
            this.pan_Cong.Controls.Add(this.lbl_AccNum);
            this.pan_Cong.Controls.Add(this.label8);
            this.pan_Cong.Controls.Add(this.label7);
            this.pan_Cong.Controls.Add(this.label6);
            this.pan_Cong.Controls.Add(this.label5);
            this.pan_Cong.Location = new System.Drawing.Point(1051, 233);
            this.pan_Cong.Name = "pan_Cong";
            this.pan_Cong.Size = new System.Drawing.Size(1027, 768);
            this.pan_Cong.TabIndex = 18;
            this.pan_Cong.Visible = false;
            // 
            // lbl_AccNum
            // 
            this.lbl_AccNum.BackColor = System.Drawing.Color.Transparent;
            this.lbl_AccNum.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_AccNum.Location = new System.Drawing.Point(244, 331);
            this.lbl_AccNum.Name = "lbl_AccNum";
            this.lbl_AccNum.Size = new System.Drawing.Size(538, 52);
            this.lbl_AccNum.TabIndex = 4;
            this.lbl_AccNum.Text = "10 번 째";
            this.lbl_AccNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(244, 487);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(538, 52);
            this.label8.TabIndex = 3;
            this.label8.Text = "무료로 드립니다.";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(244, 435);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(538, 52);
            this.label7.TabIndex = 2;
            this.label7.Text = "구매 하신 모든 상품을";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.Location = new System.Drawing.Point(244, 383);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(538, 52);
            this.label6.TabIndex = 1;
            this.label6.Text = "고객 으로 당첨 되셨습니다.";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.Location = new System.Drawing.Point(244, 239);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(538, 57);
            this.label5.TabIndex = 0;
            this.label5.Text = "축 하 합 니 다 !!!!!";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Media_Player
            // 
            this.Media_Player.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Media_Player.Enabled = true;
            this.Media_Player.Location = new System.Drawing.Point(0, 0);
            this.Media_Player.Name = "Media_Player";
            this.Media_Player.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("Media_Player.OcxState")));
            this.Media_Player.Size = new System.Drawing.Size(587, 463);
            this.Media_Player.TabIndex = 0;
            this.Media_Player.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.Media_Player_PlayStateChange);
            // 
            // frmCustomDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1089, 831);
            this.Controls.Add(this.pan_Cong);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCustomDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmCustomDisplay";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCustomDisplay_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pan_Show.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox20)).EndInit();
            this.pan_Cong.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Media_Player)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lst_Items;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.PictureBox pictureBox20;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lbl_Rest;
        private System.Windows.Forms.Label lbl_CustomPay;
        private System.Windows.Forms.Label lbl_SalePrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_tot;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pan_Show;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pan_Cong;
        private System.Windows.Forms.Label lbl_AccNum;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private AxWMPLib.AxWindowsMediaPlayer Media_Player;

    }
}