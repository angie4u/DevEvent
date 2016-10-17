namespace DevEvent.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DevEvent.Data.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DevEvent.Data.Models.ApplicationDbContext context)
        {
            // Admin Role 추가
            if (!(context.Roles.Any(r => r.Name == "Administrators")))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var roleToInsert = new IdentityRole { Name = "Administrators" };

                roleManager.Create(roleToInsert);
            }

            // Admin User 추가
            if (!(context.Users.Any(u => u.UserName == "admin@devevent.azurewebsite.net")))
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new ApplicationUser { UserName = "admin@devevent.azurewebsite.net", Email = "admin@devevent.azurewebsite.net", PhoneNumber = "1234567", Name = "Default Admin", EmailConfirmed = true, RegisterState = UserRegisterState.ConfirmedByAdmin };
                userManager.Create(userToInsert, "Admin!0987");
                userManager.AddToRole(userToInsert.Id, "Administrators");

            }
        }
    }
}
