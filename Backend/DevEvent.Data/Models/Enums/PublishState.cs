using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Models
{
    public enum PublishState
    {
        /// <summary>
        /// 생성됨
        /// </summary>
        Created = 0,
        /// <summary>
        /// 배포됨
        /// </summary>
        Published = 1,
        /// <summary>
        /// 삭제됨
        /// </summary>
        Removed = 2,
    }
}
