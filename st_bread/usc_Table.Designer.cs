namespace st_bread
{
    partial class usc_Table
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_TableNo = new System.Windows.Forms.Label();
            this.lbl_Amt = new System.Windows.Forms.Label();
            this.btn_Func = new System.Windows.Forms.Button();
            this.pan_state = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lbl_TableNo
            // 
            this.lbl_TableNo.BackColor = System.Drawing.Color.Transparent;
            this.lbl_TableNo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbl_TableNo.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_TableNo.Location = new System.Drawing.Point(0, 0);
            this.lbl_TableNo.Name = "lbl_TableNo";
            this.lbl_TableNo.Size = new System.Drawing.Size(170, 33);
            this.lbl_TableNo.TabIndex = 0;
            this.lbl_TableNo.Text = "1번";
            this.lbl_TableNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_TableNo.Click += new System.EventHandler(this.lbl_Amt_Click);
            // 
            // lbl_Amt
            // 
            this.lbl_Amt.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Amt.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbl_Amt.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Amt.Location = new System.Drawing.Point(0, 63);
            this.lbl_Amt.Name = "lbl_Amt";
            this.lbl_Amt.Size = new System.Drawing.Size(170, 37);
            this.lbl_Amt.TabIndex = 1;
            this.lbl_Amt.Text = "999,999,999";
            this.lbl_Amt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lbl_Amt.Click += new System.EventHandler(this.lbl_Amt_Click);
            // 
            // btn_Func
            // 
            this.btn_Func.Font = new System.Drawing.Font("나눔고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_Func.Location = new System.Drawing.Point(245, 36);
            this.btn_Func.Name = "btn_Func";
            this.btn_Func.Size = new System.Drawing.Size(84, 55);
            this.btn_Func.TabIndex = 2;
            this.btn_Func.Tag = "0";
            this.btn_Func.UseVisualStyleBackColor = true;
            this.btn_Func.Click += new System.EventHandler(this.btn_Func_Click);
            // 
            // pan_state
            // 
            this.pan_state.BackColor = System.Drawing.Color.Transparent;
            this.pan_state.BackgroundImage = global::st_bread.Properties.Resources.person_color;
            this.pan_state.Location = new System.Drawing.Point(135, 33);
            this.pan_state.Name = "pan_state";
            this.pan_state.Size = new System.Drawing.Size(30, 30);
            this.pan_state.TabIndex = 3;
            // 
            // usc_Table
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.btn_Func);
            this.Controls.Add(this.lbl_TableNo);
            this.Controls.Add(this.lbl_Amt);
            this.Controls.Add(this.pan_state);
            this.Font = new System.Drawing.Font("나눔고딕", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "usc_Table";
            this.Size = new System.Drawing.Size(170, 100);
            this.Click += new System.EventHandler(this.lbl_Amt_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_TableNo;
        private System.Windows.Forms.Label lbl_Amt;
        private System.Windows.Forms.Button btn_Func;
        private System.Windows.Forms.Panel pan_state;
    }
}
