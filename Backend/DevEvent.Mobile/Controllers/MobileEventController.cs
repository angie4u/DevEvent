using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using DevEvent.Data.DataObjects;
using DevEvent.Data.Models;
using System.Security.Claims;
using DevEvent.Data.ViewModels;
using System;

namespace DevEvent.Mobile.Controllers
{
    public class MobileEventController : TableController<MobileEvent>
    {
        private ApplicationDbContext DbContext;

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            DbContext = new ApplicationDbContext();
            DomainManager = new EventDtoDomainManager(DbContext, Request);
        }

        /// <summary>
        /// MobileEvent 전체 리스트 
        /// </summary>
        /// <returns></returns>
        public IQueryable<MobileEvent> GetAllMobileEvent()
        {
            var user = GetUserClaim();
            (DomainManager as EventDtoDomainManager).sId = AddMobileUser(user);
            return DomainManager.Query();
        }

        /// <summary>
        /// MobileEvent 상세
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SingleResult<MobileEvent> GetMobileEvent(string id)
        {
            var user = GetUserClaim();
            (DomainManager as EventDtoDomainManager).sId = AddMobileUser(user);
            return DomainManager.Lookup(id);
        }

        /// <summary>
        /// MobileEvent 수정요청
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        [Authorize]
        public Task<MobileEvent> PatchMobileEvent(string id, Delta<MobileEvent> patch)
        {
            var user = GetUserClaim();
            (DomainManager as EventDtoDomainManager).sId = AddMobileUser(user);
            return DomainManager.UpdateAsync(id, patch);
        }

        /// <summary>
        /// 새로운 MobileEvent
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IHttpActionResult> PostMobileEvent(MobileEvent item)
        {
            var user = GetUserClaim();
            (DomainManager as EventDtoDomainManager).sId = AddMobileUser(user);

            MobileEvent current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        /// <summary>
        /// 모바일 이벤트 삭제
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public Task DeleteMobileEvent(string id)
        {
            var user = GetUserClaim();
            (DomainManager as EventDtoDomainManager).sId = AddMobileUser(user);

            return DeleteAsync(id);
        }

        /// <summary>
        /// 인증된 사용자의 sid / provider 가져오기
        /// </summary>
        /// <returns></returns>
        private AuthenticatedUserViewModel GetUserClaim()
        {
            string provider = "";
            string sid = "";
            // 인증이 되었다면 sid 가져옴. 
            if (this.User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal principal = this.User as ClaimsPrincipal;

                provider = principal.FindFirst("http://schemas.microsoft.com/identity/claims/identityprovider").Value;
                sid = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            return new AuthenticatedUserViewModel { ProviderName = provider, sId = sid };
        }

        /// <summary>
        /// 인증된 사용자 등록
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        private string AddMobileUser(AuthenticatedUserViewModel userinfo)
        {
            var muser = this.DbContext.MobileUsers.Where(x => x.sId == userinfo.sId).FirstOrDefault();
            if (muser == null)
            {
                // Add new mobile user
                MobileUser newuser = new MobileUser
                {
                    CreatedTime = DateTimeOffset.Now,
                    LastLoginTime = DateTimeOffset.Now,
                    ProviderName = userinfo.ProviderName,
                    sId = userinfo.sId
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.DbContext.Dispose();
        }
    }
}
