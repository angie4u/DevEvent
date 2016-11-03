using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Models
{
    public class MobileUser
    {
        /// <summary>
        /// 아이디 / Mobile App의 SID 를 사용함. 
        /// </summary>
        [Key]
        public string sId { get; set; }

        /// <summary>
        /// 생성시각 
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }

        /// <summary>
        /// 마지막 로그인 시각
        /// </summary>
        public DateTimeOffset? LastLoginTime { get; set; }

        /// <summary>
        /// Login Provider name 
        /// (AAD, Google, Facebook, Microsoftaccount, Twitter) 
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// 좋아요 표시한 Event 들 ...
        /// </summary>
        public virtual ICollection<Event> Events { get; set; }
    }
}
