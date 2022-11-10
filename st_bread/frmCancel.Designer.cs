namespace st_bread
{
    partial class frmCancel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCancel));
            this.pan_Result = new System.Windows.Forms.Panel();
            this.web = new System.Windows.Forms.WebBrowser();
            this.Date_Picker = new System.Windows.Forms.DateTimePicker();
            this.lst_Pay = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader15 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_select = new System.Windows.Forms.ToolStripMenuItem();
            this.lst_ = new System.Windows.Forms.ListView();
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pic_Payno = new System.Windows.Forms.PictureBox();
            this.btn_Pre = new System.Windows.Forms.PictureBox();
            this.btn_Exit = new System.Windows.Forms.PictureBox();
            this.btn_RePrint = new System.Windows.Forms.PictureBox();
            this.btn_ReAuthCash = new System.Windows.Forms.PictureBox();
            this.pan_Result.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_Payno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Pre)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Exit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_RePrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ReAuthCash)).BeginInit();
            this.SuspendLayout();
            // 
            // pan_Result
            // 
            this.pan_Result.Controls.Add(this.web);
            this.pan_Result.Location = new System.Drawing.Point(15, 67);
            this.pan_Result.Name = "pan_Result";
            this.pan_Result.Size = new System.Drawing.Size(1000, 571);
            this.pan_Result.TabIndex = 3;
            this.pan_Result.Tag = "RESULT";
            // 
            // web
            // 
            this.web.Location = new System.Drawing.Point(3, 3);
            this.web.MinimumSize = new System.Drawing.Size(20, 20);
            this.web.Name = "web";
            this.web.Size = new System.Drawing.Size(994, 565);
            this.web.TabIndex = 1;
            this.web.Tag = "WEB";
            this.web.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // Date_Picker
            // 
            this.Date_Picker.CalendarFont = new System.Drawing.Font("굴림체", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Date_Picker.Font = new System.Drawing.Font("굴림체", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Date_Picker.Location = new System.Drawing.Point(15, 11);
            this.Date_Picker.Name = "Date_Picker";
            this.Date_Picker.Size = new System.Drawing.Size(342, 50);
            this.Date_Picker.TabIndex = 6;
            this.Date_Picker.ValueChanged += new System.EventHandler(this.Date_Picker_ValueChanged);
            // 
            // lst_Pay
            // 
            this.lst_Pay.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader14,
            this.columnHeader15});
            this.lst_Pay.ContextMenuStrip = this.contextMenuStrip1;
            this.lst_Pay.Font = new System.Drawing.Font("굴림체", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lst_Pay.FullRowSelect = true;
            this.lst_Pay.Location = new System.Drawing.Point(1052, 34);
            this.lst_Pay.MultiSelect = false;
            this.lst_Pay.Name = "lst_Pay";
            this.lst_Pay.Size = new System.Drawing.Size(932, 753);
            this.lst_Pay.TabIndex = 7;
            this.lst_Pay.UseCompatibleStateImageBehavior = false;
            this.lst_Pay.View = System.Windows.Forms.View.Details;
            this.lst_Pay.Click += new System.EventHandler(this.lst_Pay_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "날짜";
            this.columnHeader1.Width = 173;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "시 간";
            this.columnHeader2.Width = 165;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "전표번호";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 150;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "총금액";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 163;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "결제방법";
            this.columnHeader5.Width = 200;
            // 
            // columnHeader14
            // 
            this.columnHeader14.Text = "주문 여부";
            this.columnHeader14.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader14.Width = 170;
            // 
            // columnHeader15
            // 
            this.columnHeader15.Text = "결제방법";
            this.columnHeader15.Width = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_select});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(99, 26);
            // 
            // mnu_select
            // 
            this.mnu_select.Name = "mnu_select";
            this.mnu_select.Size = new System.Drawing.Size(98, 22);
            this.mnu_select.Text = "선 택";
            this.mnu_select.Click += new System.EventHandler(this.mnu_select_Click);
            // 
            // lst_
            // 
            this.lst_.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader12,
            this.columnHeader13});
            this.lst_.Location = new System.Drawing.Point(984, 847);
            this.lst_.Name = "lst_";
            this.lst_.Size = new System.Drawing.Size(631, 125);
            this.lst_.TabIndex = 9;
            this.lst_.UseCompatibleStateImageBehavior = false;
            this.lst_.View = System.Windows.Forms.View.Details;
            this.lst_.Visible = false;
            // 
            // pic_Payno
            // 
            this.pic_Payno.Image = global::st_bread.Properties.Resources.btn_payno;
            this.pic_Payno.Location = new System.Drawing.Point(363, 17);
            this.pic_Payno.Name = "pic_Payno";
            this.pic_Payno.Size = new System.Drawing.Size(193, 44);
            this.pic_Payno.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic_Payno.TabIndex = 10;
            this.pic_Payno.TabStop = false;
            this.pic_Payno.Click += new System.EventHandler(this.pic_Payno_Click);
            // 
            // btn_Pre
            // 
            this.btn_Pre.Image = global::st_bread.Properties.Resources.btn_pre1;
            this.btn_Pre.Location = new System.Drawing.Point(18, 654);
            this.btn_Pre.Name = "btn_Pre";
            this.btn_Pre.Size = new System.Drawing.Size(339, 87);
            this.btn_Pre.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Pre.TabIndex = 5;
            this.btn_Pre.TabStop = false;
            this.btn_Pre.Tag = "PRE";
            this.btn_Pre.Click += new System.EventHandler(this.btn_Pre_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.Image = global::st_bread.Properties.Resources.btn_cancel;
            this.btn_Exit.Location = new System.Drawing.Point(688, 654);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(324, 87);
            this.btn_Exit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_Exit.TabIndex = 4;
            this.btn_Exit.TabStop = false;
            this.btn_Exit.Tag = "CANCEL";
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // btn_RePrint
            // 
            this.btn_RePrint.Image = global::st_bread.Properties.Resources.btn_reprint;
            this.btn_RePrint.Location = new System.Drawing.Point(358, 654);
            this.btn_RePrint.Name = "btn_RePrint";
            this.btn_RePrint.Size = new System.Drawing.Size(324, 87);
            this.btn_RePrint.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_RePrint.TabIndex = 11;
            this.btn_RePrint.TabStop = false;
            this.btn_RePrint.Tag = "CANCEL";
            this.btn_RePrint.Click += new System.EventHandler(this.btn_RePrint_Click);
            // 
            // btn_ReAuthCash
            // 
            this.btn_ReAuthCash.Image = global::st_bread.Properties.Resources.btn_reprint;
            this.btn_ReAuthCash.Location = new System.Drawing.Point(271, 813);
            this.btn_ReAuthCash.Name = "btn_ReAuthCash";
            this.btn_ReAuthCash.Size = new System.Drawing.Size(324, 87);
            this.btn_ReAuthCash.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btn_ReAuthCash.TabIndex = 12;
            this.btn_ReAuthCash.TabStop = false;
            this.btn_ReAuthCash.Tag = "CANCEL";
            this.btn_ReAuthCash.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // frmCancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 988);
            this.Controls.Add(this.btn_ReAuthCash);
            this.Controls.Add(this.btn_Pre);
            this.Controls.Add(this.lst_Pay);
            this.Controls.Add(this.pic_Payno);
            this.Controls.Add(this.lst_);
            this.Controls.Add(this.Date_Picker);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.pan_Result);
            this.Controls.Add(this.btn_RePrint);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCancel";
            this.Text = "frmCancel";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCancel_Load);
            this.pan_Result.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pic_Payno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Pre)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_Exit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_RePrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ReAuthCash)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pan_Result;
        private System.Windows.Forms.WebBrowser web;
        private System.Windows.Forms.PictureBox btn_Exit;
        private System.Windows.Forms.PictureBox btn_Pre;
        private System.Windows.Forms.DateTimePicker Date_Picker;
        private System.Windows.Forms.ListView lst_Pay;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnu_select;
        public System.Windows.Forms.ListView lst_;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.ColumnHeader columnHeader12;
        private System.Windows.Forms.ColumnHeader columnHeader13;
        private System.Windows.Forms.ColumnHeader columnHeader14;
        private System.Windows.Forms.PictureBox pic_Payno;
        private System.Windows.Forms.PictureBox btn_RePrint;
        private System.Windows.Forms.PictureBox btn_ReAuthCash;
        private System.Windows.Forms.ColumnHeader columnHeader15;
    }
}