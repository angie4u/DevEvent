﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.ViewModels
{
    public class EventDetailViewModel
    {
        public long Id { get; set; }

        /// <summary>
        /// 이벤트 타이틀
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 이벤트 설명 
        /// 웹 편집기를 이용해서 HTML 로 작성되어 저장됨. 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 시작시각
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// 끝나는 시각
        /// </summary>
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
        public double Latitude { get; set; }

        /// <summary>
        /// 경도
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 행사 주요 참여자의 분류
        /// IT Pro, 개발자, 비즈니스 담당자 등 ... 
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 행상 등록 페이지 Url
        /// </summary>
        public string RegistrationUrl { get; set; }

        /// <summary>
        /// 대표이미지로부터 만든 Thumbnail Image Url
        /// </summary>
        public string ThumbnailImageUrl { get; set; }

        /// <summary>
        /// 특성이미지, 대표이미지 Url
        /// </summary>
        public string FeatureImageUrl { get; set; }

        /// <summary>
        /// 누가 만들었나
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 만든 Admin Nav Prop
        /// </summary>
        public string CreateUserName { get; set; }

        public IList<EventRelatedLinkViewModel> RelatedLinks { get; set; }
    }
}
