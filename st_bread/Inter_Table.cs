using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st_bread
{
    public  interface Inter_Table
    {
        void Inter_TableSelected(usc_Table oTable);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oTable"></param>
        /// <param name="btnTag">번호지정 = 1 , 테이블 삭제 = 2</param>
        void Inter_TableFunc(usc_Table oTable,string btnTag);

    }
}
