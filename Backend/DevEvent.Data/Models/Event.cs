using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Models
{
    public class Event
    {
        /// <summary>
        /// 이벤트 키 
        /// </summary>
        [Key]
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 누가 만들었나
        /// </summary>
        public string CreateUserId { get; set; }

        public ApplicationUser CreateUser { get; set; }
    }
}
