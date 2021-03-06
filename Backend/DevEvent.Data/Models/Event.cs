﻿using Microsoft.Azure.Mobile.Server.Tables;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Models
{
    public class Event
    {
        #region "For Mobile Offline Sync"

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Index]
        [TableColumn(TableColumnType.CreatedAt)]
        public DateTimeOffset? CreatedAt { get; set; }

        [TableColumn(TableColumnType.Deleted)]
        public bool Deleted { get; set; }

        [Index]
        [TableColumn(TableColumnType.Id)]
        [MaxLength(36)]
        public string Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [TableColumn(TableColumnType.UpdatedAt)]
        public DateTimeOffset? UpdatedAt { get; set; }

        [TableColumn(TableColumnType.Version)]
        [Timestamp]
        public byte[] Version { get; set; }

        #endregion

        /// <summary>
        /// 이벤트 키 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long EventId { get; set; }

        /// <summary>
        /// 배포(Publish) 
        /// </summary>
        public PublishState PublishState { get; set; }

        /// <summary>
        /// 이벤트 타이틀
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// 이벤트 설명 
        /// 웹 편집기를 이용해서 HTML 로 작성되어 저장됨. 
        /// </summary>
        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        /// <summary>
        /// 시작시각
        /// </summary>
        [Required]
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// 끝나는 시각
        /// </summary>
        [Required]
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// 행사장소
        /// </summary>
        public string Venue { get; set; }

        /// <summary>
        /// 행사장 주소
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 위도
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// 경도
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// 행사 주요 참여자의 분류
        /// IT Pro, 개발자, 비즈니스 담당자 등 ... 
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 행사 등록 페이지 Url
        /// </summary>
        public string RegistrationUrl { get; set; }

        /// <summary>
        /// 대표이미지로부터 만든 Thumbnail Image Url
        /// </summary>
        public string ThumbnailImageUrl { get; set; }

        /// <summary>
        /// 특성이미지, 대표이미지 Url
        /// </summary>
        public string FeaturedImageUrl { get; set; }


        /// <summary>
        /// 누가 만들었나
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 만든 Admin Nav Prop
        /// </summary>
        public ApplicationUser CreateUser { get; set; }

        /// <summary>
        /// 관련 링크들
        /// </summary>
        //public ICollection<EventRelatedLink> RelatedLinks { get; set; }

        public virtual ICollection<MobileUser> FavoriteMobileUsers { get; set; }
    }
}
