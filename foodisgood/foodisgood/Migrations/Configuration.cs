namespace foodisgood.Migrations
{
    using foodisgood.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;


    internal sealed class Configuration : DbMigrationsConfiguration<foodisgood.Models.ApplicationDbContext>
    {
        public Configuration()
        {

            AutomaticMigrationsEnabled = false;
            ContextKey = "foodisgood.Models.ApplicationDbContext";
        }


        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            if (!context.Roles.Any(r => r.Name == "AppAdmin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppAdmin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Customer"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Customer" };

                manager.Create(role);
            }

            if (!context.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product{Name="Tomatoes",Type="Spanish",Quality=1,ExpirationDate=DateTime.Parse("2021-4-6")},
                    new Product{Name="Carrots",Type="French",Quality=1,ExpirationDate=DateTime.Parse("2021-4-3")},
                    new Product{Name="Oranges",Type="Portugese",Quality=1,ExpirationDate=DateTime.Parse("2021-5-6")},
                    new Product{Name="Banana",Type="American",Quality=1,ExpirationDate=DateTime.Parse("2021-5-6")},
                    new Product{Name="Pineapple",Type="Taiwanese",Quality=1,ExpirationDate=DateTime.Parse("2021-5-6")},
                    new Product{Name="Apples",Type="French",Quality=1,ExpirationDate=DateTime.Parse("2021-5-6")},
                    new Product{Name="Cucumber",Type="French",Quality=1,ExpirationDate=DateTime.Parse("2021-5-6")}
                };
                products.ForEach(s => context.Products.Add(s));
                context.SaveChanges();
            }

            if (!context.Rewiews.Any())
            {
                var Rewiews = new List<Rewiew>
            {
                new Rewiew{UserID="b187ddc7-13bd-4d54-8610-89d7a24160ca",Text="Positive opinion",date=DateTime.Now, note=4 },
                new Rewiew{UserID="b187ddc7-13bd-4d54-8610-89d7a24160ca",Text="Negative opinion",date=DateTime.Now, note=3 }
            };
                Rewiews.ForEach(s => context.Rewiews.Add(s));
            }

            if (!context.Users.Any())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user1 = new ApplicationUser { UserName = "customerOne@Test.com", Email = "customerOne@Test.com", FirstName = "Adrian_1", LastName = "Petrica_1", Location = "Cluj" };
                var user2 = new ApplicationUser { UserName = "customerTwo@Test.com", Email = "customerTwo@Test.com", FirstName = "Adrian_2", LastName = "Petrica_2", Location = "Cluj" };
                var user3 = new ApplicationUser { UserName = "customerThree@Test.com", Email = "customerThree@Test.com", FirstName = "Adrian_3", LastName = "Petrica_3", Location = "Cluj" };
                var admin = new ApplicationUser { UserName = "admin@Test.com", Email = "admin@Test.com", FirstName = "Admin", LastName = "Admin", Location = "Constanta" };

                manager.Create(user1, "123456");
                manager.Create(user2, "123456");
                manager.Create(user3, "123456");
                manager.Create(admin, "123456");
                manager.AddToRole(user1.Id, "Customer");
                manager.AddToRole(user2.Id, "Customer");
                manager.AddToRole(user3.Id, "Customer");
                manager.AddToRole(admin.Id, "AppAdmin");
            }

            if (!context.Offers.Any())
            {
                var product = context.Products.Where(p => p.Name.Equals("Banana")).ToList();
                var user = context.Users.Where(u => u.UserName.Equals("customerOne@Test.com")).ToList();

                var offers = new List<Offer>
                {
                    new Offer{UserID = user[0].Id, Name="Offer one (expired)", PriceUnit=2, Quantity=100, CreateTime=DateTime.Parse("2020-4-3"), EndTime=DateTime.Parse("2020-9-3"), OfferType=false, Description="Quality 1", ProductID=product[0].ID},
                    new Offer{UserID = user[0].Id, Name="I sell more stuff!", PriceUnit=4, Quantity=179, CreateTime=DateTime.Parse("2021-4-6"), EndTime=DateTime.Parse("2023-9-3"), OfferType=false, Description="uwu", ProductID=product[0].ID},
                    new Offer{UserID = user[0].Id, Name="Fantastic products!", PriceUnit=7, Quantity=163, CreateTime=DateTime.Parse("2021-4-18"), EndTime=DateTime.Parse("2022-9-3"), OfferType=false, Description="lul", ProductID=product[0].ID},
                    new Offer{UserID = user[0].Id, Name="Great quality vegetables!", PriceUnit=9, Quantity=24, CreateTime=DateTime.Parse("2021-4-4"), EndTime=DateTime.Parse("2025-9-3"), OfferType=false, Description="Quality 2", ProductID=product[0].ID},
                    new Offer{UserID = user[0].Id, Name="Lovely vegetables!", PriceUnit=4, Quantity=35, CreateTime=DateTime.Parse("2021-4-2"), EndTime=DateTime.Parse("2021-8-3"), OfferType=false, Description="Genius delivery.", ProductID=product[0].ID},
                };
                offers.ForEach(s => context.Offers.Add(s));
                context.SaveChanges();
            }
        }
    }
}
