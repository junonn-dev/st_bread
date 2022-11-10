namespace st_bread
{
    partial class uscCalendar
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
            this.lbl_Date = new System.Windows.Forms.Label();
            this.lbl_Amt = new System.Windows.Forms.Label();
            this.lbl_Cnt = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_IsClose = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_Date
            // 
            this.lbl_Date.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Date.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Date.Location = new System.Drawing.Point(3, 0);
            this.lbl_Date.Name = "lbl_Date";
            this.lbl_Date.Size = new System.Drawing.Size(98, 21);
            this.lbl_Date.TabIndex = 0;
            this.lbl_Date.Text = "label1";
            this.lbl_Date.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lbl_Amt
            // 
            this.lbl_Amt.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Amt.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Amt.Location = new System.Drawing.Point(22, 21);
            this.lbl_Amt.Name = "lbl_Amt";
            this.lbl_Amt.Size = new System.Drawing.Size(78, 20);
            this.lbl_Amt.TabIndex = 1;
            this.lbl_Amt.Text = "1,000,000";
            this.lbl_Amt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl_Cnt
            // 
            this.lbl_Cnt.BackColor = System.Drawing.Color.Transparent;
            this.lbl_Cnt.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_Cnt.Location = new System.Drawing.Point(33, 41);
            this.lbl_Cnt.Name = "lbl_Cnt";
            this.lbl_Cnt.Size = new System.Drawing.Size(68, 22);
            this.lbl_Cnt.TabIndex = 2;
            this.lbl_Cnt.Text = "label3";
            this.lbl_Cnt.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(-4, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "매출";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(-4, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "객수";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_IsClose
            // 
            this.lbl_IsClose.BackColor = System.Drawing.Color.Transparent;
            this.lbl_IsClose.Font = new System.Drawing.Font("나눔고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_IsClose.Location = new System.Drawing.Point(-4, 70);
            this.lbl_IsClose.Name = "lbl_IsClose";
            this.lbl_IsClose.Size = new System.Drawing.Size(105, 20);
            this.lbl_IsClose.TabIndex = 6;
            this.lbl_IsClose.Text = "마 감";
            this.lbl_IsClose.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uscCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::st_bread.Properties.Resources.bg_btntype01;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbl_IsClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_Cnt);
            this.Controls.Add(this.lbl_Amt);
            this.Controls.Add(this.lbl_Date);
            this.Name = "uscCalendar";
            this.Size = new System.Drawing.Size(100, 67);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_Date;
        private System.Windows.Forms.Label lbl_Amt;
        private System.Windows.Forms.Label lbl_Cnt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_IsClose;
    }
}
