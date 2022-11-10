namespace st_bread
{
    partial class frmStock
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
            this.pan_QtyChange = new System.Windows.Forms.Panel();
            this.txt_Date = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lst_View = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pan_QtyChange);
            this.panel1.Controls.Add(this.txt_Date);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.lst_View);
            this.panel1.Controls.Add(this.btn_Save);
            this.panel1.Controls.Add(this.btn_Exit);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1024, 768);
            this.panel1.TabIndex = 0;
            // 
            // pan_QtyChange
            // 
            this.pan_QtyChange.Location = new System.Drawing.Point(835, 295);
            this.pan_QtyChange.Name = "pan_QtyChange";
            this.pan_QtyChange.Size = new System.Drawing.Size(668, 305);
            this.pan_QtyChange.TabIndex = 19;
            // 
            // txt_Date
            // 
            this.txt_Date.Font = new System.Drawing.Font("나눔고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_Date.Location = new System.Drawing.Point(326, 12);
            this.txt_Date.Name = "txt_Date";
            this.txt_Date.Size = new System.Drawing.Size(212, 39);
            this.txt_Date.TabIndex = 17;
            this.txt_Date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("나눔고딕", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.Location = new System.Drawing.Point(190, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(130, 31);
            this.label10.TabIndex = 16;
            this.label10.Text = "영업일자 :";
            // 
            // lst_View
            // 
            this.lst_View.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lst_View.Font = new System.Drawing.Font("나눔고딕", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lst_View.FullRowSelect = true;
            this.lst_View.GridLines = true;
            this.lst_View.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lst_View.Location = new System.Drawing.Point(190, 69);
            this.lst_View.MultiSelect = false;
            this.lst_View.Name = "lst_View";
            this.lst_View.Size = new System.Drawing.Size(516, 613);
            this.lst_View.TabIndex = 15;
            this.lst_View.UseCompatibleStateImageBehavior = false;
            this.lst_View.View = System.Windows.Forms.View.Details;
            this.lst_View.SelectedIndexChanged += new System.EventHandler(this.lst_View_SelectedIndexChanged);
            this.lst_View.Click += new System.EventHandler(this.lst_View_Click);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "번 호";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "상품코드";
            this.columnHeader2.Width = 0;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "상 품 명";
            this.columnHeader3.Width = 300;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "수 량";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader4.Width = 130;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "그룹코드";
            this.columnHeader5.Width = 0;
            // 
            // btn_Save
            // 
            this.btn_Save.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Save.Font = new System.Drawing.Font("굴림체", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Save.ForeColor = System.Drawing.Color.White;
            this.btn_Save.Location = new System.Drawing.Point(597, 701);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(199, 64);
            this.btn_Save.TabIndex = 14;
            this.btn_Save.Text = "저 장";
            this.btn_Save.UseVisualStyleBackColor = false;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_Exit
            // 
            this.btn_Exit.BackColor = System.Drawing.Color.LightBlue;
            this.btn_Exit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Exit.Font = new System.Drawing.Font("굴림체", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Exit.ForeColor = System.Drawing.Color.White;
            this.btn_Exit.Location = new System.Drawing.Point(822, 701);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(199, 64);
            this.btn_Exit.TabIndex = 13;
            this.btn_Exit.Text = "닫 기";
            this.btn_Exit.UseVisualStyleBackColor = false;
            this.btn_Exit.Click += new System.EventHandler(this.btn_Exit_Click);
            // 
            // frmStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1183, 829);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmStock";
            this.Text = "frmStock";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmStock_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_Save;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.ListView lst_View;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox txt_Date;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel pan_QtyChange;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}