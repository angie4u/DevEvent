using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Apps.Models
{
    public class MobileEvent : INotifyPropertyChanged
    {

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Version]
        public string Version { get; set; }


        /// <summary>
        /// 배포(Publish) 
        /// </summary>
        public PublishState PublishState { get; set; }

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
        /// 생성일
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }
        /// <summary>
        /// 수정일
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }

        /// <summary>
        ///  만든사람 이름
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 만든사람 아이디
        /// </summary>
        public string CreateUserId { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
