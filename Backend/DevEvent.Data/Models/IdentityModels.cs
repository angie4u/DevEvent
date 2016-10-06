using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace DevEvent.Data.Models
{
    // ApplicationUser 클래스에 더 많은 속성을 추가하여 사용자에 대한 프로필 데이터를 추가할 수 있습니다. 자세히 알아보려면 http://go.microsoft.com/fwlink/?LinkID=317594를 방문하십시오.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType은 CookieAuthenticationOptions.AuthenticationType에 정의된 항목과 일치해야 합니다.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 여기에 사용자 지정 사용자 클레임 추가
            return userIdentity;
        }

        /// <summary>
        /// 이름(실명)
        /// </summary>
        [Index]
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 가입일
        /// </summary>
        [Index]
        [Required]
        public DateTimeOffset CreatedTime { get; set; }

        /// <summary>
        /// 업데이트 일 
        /// </summary>
        public DateTimeOffset? UpdatedTime { get; set; }

        /// <summary>
        /// 가입상태 
        /// (가입 / 승인 / 중지 / 탈퇴)
        /// </summary>
        [Index]
        public UserRegisterState RegisterState { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Event> Events { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            // user
            modelBuilder.Entity<ApplicationUser>().Property(t => t.Name).IsRequired();

            // Events
            modelBuilder.Entity<Event>().ToTable("Events");
            modelBuilder.Entity<Event>().Property(t => t.Title).IsRequired();
            modelBuilder.Entity<Event>().HasRequired(t => t.CreateUser).WithMany(t => t.Events).HasForeignKey(t => t.CreateUserId);
            modelBuilder.Entity<Event>().HasMany(t => t.RelatedLinks).WithRequired(t => t.Event).HasForeignKey(t => t.EventId);



        }
    }
}