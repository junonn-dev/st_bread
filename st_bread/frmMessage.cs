using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace st_bread
{
    public partial class frmMessage : Form
    {   
        public frmMessage(int i)
        {
            InitializeComponent();
            switch (i)
            {
                case (0): //경고
                    pictureBox1.Size = new Size(468, 216);
                   // pictureBox1.Image = Properties.Resources.sorry;
                    break;
                case (1): //안내
                    pictureBox1.Size = new Size(346, 319);
                   // pictureBox1.Image = Properties.Resources.atten;
                    break;

            }
        }

        private void pic_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Check_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /////
        public void Set_Message(int iMsg)
        {
            switch(iMsg)
            {
                case (0):
                    lbl_Message3.Text = "선택 전표 없음";
                    lbl_Message2.Text = "취소할 전표를 ";
                    lbl_Message.Text = "선택 해주세요.";
                    break;

                case (1):
                    lbl_Message3.Text = "취소 완료";
                    lbl_Message2.Text = "";
                    lbl_Message.Text = "";
                    break;

                case (2):
                    lbl_Message3.Text = "비밀 번호 오류";
                    lbl_Message2.Text = "기재 해 주세요.";
                    lbl_Message.Text = "로그인 시 사용한 비밀번호를";

                    break;

                case (3):
                    lbl_Message3.Text = "인터넷 연결끊김";
                    lbl_Message.Text = "죄송합니다.  ";
                    lbl_Message2.Text = "카드 사용 불가.";
                    break;
                case (4):
                    lbl_Message3.Text = "인터넷 연결끊김";
                    lbl_Message.Text = "죄송합니다.  ";
                    lbl_Message2.Text = "현금영수증 사용불가.";
                    break;
                case (5):
                    lbl_Message3.Text = "전화번호 미 기재";
                    lbl_Message.Text = "전화 번호를 숫자만";
                    lbl_Message2.Text = "입력해주세요.";
                    break;
                case (6):
                    lbl_Message3.Text = "자료 저장 실패";
                    lbl_Message.Text = "죄송합니다.  ";
                    lbl_Message2.Text = "직원에게 문의 해주세요.";
                    break;
                case (7):
                    lbl_Message3.Text = "승인 실패";
                    lbl_Message.Text = "죄송합니다.  ";
                    lbl_Message2.Text = "직원에게 문의 해주세요.";
                    break;
                case (8):
                    lbl_Message3.Text = "취소 전표";
                    lbl_Message.Text = "이미 취소가 된 전표  ";
                    lbl_Message2.Text = "입니다.";
                    break;
                case (9):
                    lbl_Message3.Text = "현금 영수증";
                    lbl_Message.Text = "현금 영수증 발급 여부를";
                    lbl_Message2.Text = "선택 해주세요.";
                    break;
                case (10):
                    lbl_Message3.Text = "현금 영수증";
                    lbl_Message.Text = "이미 현금 영수증을 발급 ";
                    lbl_Message2.Text = "받았습니다.";
                    break;
                case (11):
                    lbl_Message3.Text = "수량 등록";
                    lbl_Message.Text = "저장이 완료 되었습니다. ";
                    lbl_Message2.Text = "";
                    break;
                case (12):
                    lbl_Message3.Text = "인터넷 연결끊김";
                    lbl_Message.Text = "죄송합니다.  ";
                    lbl_Message2.Text = "수량등록 업무 불가.";
                    break;
                case (13):
                    lbl_Message3.Text = "현금 영수증";
                    lbl_Message.Text = "신용 카드 로 결제  ";
                    lbl_Message2.Text = "하였습니다.";
                    break;
                case (14):
                    lbl_Message3.Text = "등록 상품 없음";
                    lbl_Message.Text = "등록된 상품이 없습니다.  ";
                    lbl_Message2.Text = "상품을 등록 해주세요.";
                    break;

            }

        }


    }
}

