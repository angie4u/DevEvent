﻿using DevEvent.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.ViewModels
{
    public class EventRelatedLinkViewModel
    {
        public long Id { get; set; }

        /// <summary>
        /// 링크타입: 웹페이지, 비디오, 프리젠테이션, 문서
        /// </summary>
        public LinkType LinkType { get; set; }

        /// <summary>
        /// 링크주소
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 생성시각
        /// </summary>
        public DateTimeOffset CreatedTime { get; set; }

        /// <summary>
        /// 링크 설명
        /// </summary>
        public string Description { get; set; }
    }
}
