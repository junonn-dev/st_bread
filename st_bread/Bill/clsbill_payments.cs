using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    public class clsbill_payments : IDisposable
    {
        private bool disposed = false;

        public string bill_pos { set; get; } //기기 번호               
        public int bill_date { set; get; }  //영수증 날짜

        public int bill_seq { set; get; }
        public clsEnum.Payment_Kind bill_paymentskind { set; get; }
         
        public string sRepCode { get; set; }

        /// <summary>
        /// 받은금액
        /// </summary>
        public double bill_recvamt { set; get; } //받은돈
        
        /// <summary>
        /// 거스름돈
        /// </summary>
        public double bill_restAmt { set; get; } //받은돈
        
        /// <summary>
        /// 승인금액
        /// </summary>
        public double bill_paymentamt { set; get; } //승인금액
        /// <summary>
        /// 시리얼번호
        /// </summary>
        public string bill_serialnum { set; get; }
        /// <summary>
        /// 매입사
        /// </summary>
        public string bill_buycmpny { set; get; }
        /// <summary>
        /// 카드사
        /// </summary>
        public string bill_cardcmpny { set; get; }
        /// <summary>
        /// 밴사
        /// </summary>
        public string bill_vancmpny { set; get; }
        /// <summary>
        /// 사인
        /// </summary>
        public string bill_signpath { set; get; }
        /// <summary>
        /// 할부
        /// </summary>
        public string bill_halbu { set; get; }
        /// <summary>
        /// 카드번호
        /// </summary>
        public string bill_cardnum { set; get; }
        /// <summary>
        /// 승인번호
        /// </summary>
        public string bill_authnum { set; get; }
        /// <summary>
        /// 캣아이디
        /// </summary>
        public string bill_vanid { set; get; }
        /// <summary>
        /// 티아이디
        /// </summary>
        public string bill_tid { set; get; }
        /// <summary>
        /// 승인일시
        /// </summary>
        public int bill_authdatetime { set; get; }

        /// <summary>
        /// 가맹점코드
        /// </summary>
        public string bill_cmpny { get; set; }

        /// <summary>
        /// 승인고유번호
        /// </summary>
        public string bill_OrgApprovalNum { set; get; }


        public clsEnum.bill_isCancel bill_iscancel { set; get; } //취소 여부


        /// <summary>
        /// 카드구분 2byte
        /// JTnet기준
        /// 'CK':신용, 'CH':체크, 'GK':기프트카드, 'UP':중국은련
        /// 'NT':BC/NH면세유, 'OL':유가보조카드, 'CP':원카드
        /// </summary>
        public string bill_cardkind { get; set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
                disposed = true;

            }

        }

        ~clsbill_payments()
        {
            Dispose(false);
        }

    }
}
