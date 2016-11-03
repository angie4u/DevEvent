using DevEvent.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEvent.Data.Services
{
    public class MobileUserService : IMobileUserService
    {
        private ApplicationDbContext DbContext;

        public MobileUserService(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        /// <summary>
        /// 새로운 MobileUser를 생성하거나 기존 MobileUser를 가져와서 return
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public string AddMobileUser(string sid, string provider)
        {
            var muser = this.DbContext.MobileUsers.Where(x => x.sId == sid).FirstOrDefault();
            if (muser == null)
            {
                // Add new mobile user
                MobileUser newuser = new MobileUser
                {
                    CreatedTime = DateTimeOffset.Now,
                    LastLoginTime = DateTimeOffset.Now,
                    ProviderName = provider,
                    sId = sid
                };

                this.DbContext.MobileUsers.Add(newuser);
                this.DbContext.SaveChanges();

                return newuser.sId;
            }
            else
            {
                return muser.sId;
            }
        }

        public MobileUser GetMobileUser(string sid)
        {
            return this.DbContext.MobileUsers.Where(x => x.sId == sid).FirstOrDefault();
        }
    }
}
