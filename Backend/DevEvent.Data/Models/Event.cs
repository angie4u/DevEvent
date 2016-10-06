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

        /// <summary>
        /// 이벤트 타이틀
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 이벤트 설명 
        /// </summary>
        public string Description { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public string Venue { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }


        public string Audience { get; set; }

        public string RegistrationUrl { get; set; }

        /// <summary>
        /// 행사 끝나고 나서 관련된 링크 
        /// </summary>
        public string ContentUrl { get; set; }

        public string ThumbnailImageUrl { get; set; }

        public string FeatureImageUrl { get; set; }


        /// <summary>
        /// 누가 만들었나
        /// </summary>
        public string CreateUserId { get; set; }

        public ApplicationUser CreateUser { get; set; }
    }
}
