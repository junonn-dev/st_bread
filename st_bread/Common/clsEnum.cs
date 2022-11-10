using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace st_bread
{
    public class clsEnum
    {
        public enum Select_SerialSet : int
        {
            [Description("포트")]
            port = 0,
            [Description("속도")]
            speed = 1,
            [Description("패리티")]
            parity = 2,
            [Description("데이타")]
            databits = 3,
            [Description("스톱바이트")]
            stopbits = 4,
            [Description("핸드쉐이킹")]
            handshaking = 5,
            [Description("VAN")]
            van = 6,
            [Description("상품")]
            items = 7
            

        }



        public enum Sendor_State : int
        {
            [Description("상태미정")]
            none = 0,
            [Description("상태이상")]
            fail = 1,
            [Description("상태미정")]
            none2 = 2,
            [Description("정상")]
            ok = 3
        }

        public enum Door_State : int
        {
            [Description("상태미정")]
            none = 0,
            [Description("열림")]
            opened = 1,
            [Description("닫힘")]
            closed = 2,
            [Description("중간열림")]
            firstopen = 3
        }


        public enum Slot_State : int
        {
            [Description("정위치,상품적재")]
            motor_items = 0,
            [Description("오위치,상품적재")]
            nomotor_items = 1,
            [Description("정위치,상품없음")]
            motor_noitems = 2,
            [Description("오위치,상품없음")]
            nomotor_noitems = 3
        }


        public enum Item_vat : int
        {
            [Description("면세")]
            item_duty_free = 0,
            [Description("과세")]
            item_tax = 1
        }


        public enum Item_Kind : int 
        {
            [Description("한우")]
            item_kind_1 = 1,
            [Description("한돈")]
            item_kind_2 = 2,
            [Description("기타")]
            item_kind_3 = 3,
            [Description("기타2")]
            item_kind_4 = 4
        }

        public enum Item_State : int
        {
            [Description("신선육")]
            item_fresh = 0,
            [Description("익힌제품")]
            item_cook = 1
        }

        public enum Item_price_set : int
        {
            [Description("중량")]
            item_capa = 0,
            [Description("판매가")]
            item_price = 1
        }


        /// <summary>
        /// 전표 취소 여부
        /// </summary>
        /// 
        public enum bill_isCancel : int
        {
            [Description("정상")]
            auth = 0,
            [Description("취소")]
            cancel = 1,
            [Description("수정")]
            reissue = 2
        }


        /// <summary>
        /// 상품이 내장고에서 나가는 이유
        /// </summary>
        public enum Stock_Out_Reason : int
        {
            [Description("판매")]
            sale = 0,
            [Description("회수")]
            refund = 1

        }

        /// <summary>
        /// 결제 수단
        /// </summary>
        public enum Payment_Kind : int
        {
            [Description("현금")]
            cash = 0,
            [Description("현금영수증")]
            cashwithaut = 1,
            [Description("신용카드")]
            card = 2,
            [Description("삼성페이")]
            sampay = 3,
            [Description("카카오페이")]
            kakaopay = 4,
            [Description("제로페이")]
            monkeypay = 5

        }


        /// <summary>
        /// 슬롯 아이템 상태 /// 상품배출 성공여부 0-기본값 1-성공 2-실패
        /// </summary>
        public enum Slot_Item_State : int
        {
            [Description("대기중")]
            standby = 0,
            [Description("배출완료")]
            let_ok = 1,
            [Description("배출실패")]
            let_fail = 2
        }


        /// <summary>
        /// van 회사 종료
        /// </summary>
        public enum Van_Cmpny : int
        {
            [Description("none")]
            none = 0,
            [Description("JTNET")]
            jtnet = 1,
            [Description("Kovan")]
            kovan = 2,            
            [Description("Kicc")]
            kicc = 3, 
            [Description("Ksnet")]
            ksnet = 4,
            [Description("Nice")]
            nice = 5,
            [Description("Smartro")]
            smartro = 6,
            [Description("JTNET2")]
            jtnet2 = 7
        }


        /// <summary>
        /// 화면 스킨 종류
        /// </summary>
        public enum skin : int
        {
            [Description("밝은색")]
            White = 0,
            [Description("어두운색")]
            Black = 1
        }


        /// <summary>
        /// 품절화면에 들어온 이유
        ///  -1 = 시스템장애 0 - 상품 품절  2= 인터넷장애
        ///  3 = 상품로딩시 에러 4 = 재고파악시 에러 5= 시리얼장애시 에러 
        ///  6 = 결제후 작동이상으로 구매 취소한경우 7 = db연결문자열 오류
        ///  8 = van 이상 11=일부품절
        /// </summary>
        public enum Close_Reson : int
        {
            [Description("프로그램시작")]
            Start = -2,
            [Description("시스템장애")]
            Sys_Err = -1,
            [Description("상품품절")]
            Sold_Out = 0,
            [Description("정상작동")]
            Default = 1,
            [Description("인터넷장애")]
            Net_Err = 2,
            [Description("상품읽기에러")]
            Item_Err = 3,
            [Description("재고읽기에러")]
            Stock_Err = 4,
            [Description("냉장고장애")]
            Port_Err = 5,
            [Description("작동에러")]
            Frozen_Err = 6,
            [Description("DB에러")]
            DB_Err = 7,
            [Description("Van에러")]
            Van_Err = 8,
            [Description("일부품절")]
            Part_SoldOut = 11,
            [Description("상품걸림")]
            Item_Stuck = 12,
            [Description("프로그램종료")]
            exit = 99
        }

        /// <summary>
        /// 자판기서 실행할 명령
        /// 
        /// </summary>
        public enum Machine_Cmd : int
        {
            [Description("없음")]
            def = 0,
            /// <summary>
            /// 프로그램종료 1
            /// </summary>
            [Description("프로그래종료")]
            end_prog = 1,
            /// <summary>
            /// 프로그램재시작 2
            /// </summary>
            [Description("프로그램재시작")]
            re_prog = 2,
            /// <summary>
            /// 윈도우 종료 3
            /// </summary>
            [Description("윈도우종료")]
            end_os = 3,
            /// <summary>
            /// 윈도우 재시작 4
            /// </summary>
            [Description("윈도우재시작")]
            re_os = 4,           
            /// <summary>
            /// 관리자열기 5
            /// </summary>
            [Description("관리자열기")]
            go_config = 5,
            /// <summary>
            /// 품절표시 6
            /// </summary>
            [Description("품절화면표시")]
            go_close = 6,

        }



        public enum Table_State : int
        {
            [Description("테이블아님")]
            empty = 0,
            [Description("테이블")]
            table = 1,
            [Description("테이블_손님")]
            table_On = 2,
            [Description("테이블_합체")]
            table_Assem = 3
        }
    }
}
