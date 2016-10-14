using DevEvent.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.ViewModels
{
    public class EventListViewModel
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset StartDate { get; set; }

        public DateTimeOffset EndDate { get; set; }

        public PublishState PublishState { get; set; }

        /// <summary>
        /// 대표이미지로부터 만든 Thumbnail Image Url
        /// </summary>
        public string ThumbnailImageUrl { get; set; }
    }
}
