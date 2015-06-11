namespace StartWatch.Migrations
{
    using StartWatch.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<StartWatch.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "StartWatch.Models.ApplicationDbContext";
        }

        protected override void Seed(StartWatch.Models.ApplicationDbContext context)
        {
            context.Activities.AddOrUpdate(
                a => a.Id,
                new Activity
                {
                    Id = 1,
                    UserId = "jonpaul.simonelli@gmail.com",
                    Quota = 7200,
                    ProgressToday = 0,
                    ProgressWeek = 0,
                    ProgressTotal = 0,
                    CreationDate = DateTime.Today,
                    Required = true,
                    Name = "Test",
                    Days = null,
                    SessionCount = 0
                }
                );

            context.Users.AddOrUpdate(
                u => u.Id,
                new ApplicationUser
                {
                    Id = "1",

                }
                );
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
