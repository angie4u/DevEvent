using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Models
{
    public enum UserRegisterState
    {
        /// <summary>
        /// 가입
        /// </summary>
        Created = 0,
        /// <summary>
        /// 승인
        /// </summary>
        ConfirmedByAdmin = 1,
        /// <summary>
        /// 중지
        /// </summary>
        Pending = 2,
        /// <summary>
        /// 탈퇴
        /// </summary>
        Withdrawn = 3
    }
}
