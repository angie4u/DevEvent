using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Models
{
    public class EventRelatedLink
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 링크타입: 웹페이지, 비디오, 프리젠테이션, 문서
        /// </summary>
        public LinkType LinkType { get; set; }

        /// <summary>
        /// 링크주소
        /// </summary>
        [Required]
        public string Url { get; set; }

        /// <summary>
        /// 생성시각
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }

        /// <summary>
        /// 링크 설명
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 관련 이벤트 아이디
        /// </summary>
        public long EventId { get; set; }

        /// <summary>
        /// 이벤트 Nav prop
        /// </summary>
        public Event Event { get; set; }
    }
}
