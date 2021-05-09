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

            var Rewiews = new List<Rewiew>
            {
                new Rewiew{UserID="b187ddc7-13bd-4d54-8610-89d7a24160ca",Text="Positive opinion"},
                new Rewiew{UserID="b187ddc7-13bd-4d54-8610-89d7a24160ca",Text="Negative opinion"}
            };
            Rewiews.ForEach(s => context.Rewiews.Add(s));

            var offers = new List<Offer>
            {
                new Offer{Name="Petrica vinde", PriceUnit=2, Quantity=100, CreateTime=DateTime.Parse("2021-4-3"), EndTime=DateTime.Parse("2021-9-3"), OfferType=true, Description="Quality 1", ProductID=products[0].ID},
                new Offer{Name="Mihaita vinde", PriceUnit=4, Quantity=179, CreateTime=DateTime.Parse("2021-4-6"), EndTime=DateTime.Parse("2023-9-3"), OfferType=true, Description="", ProductID=products[0].ID},
                new Offer{Name="Eu vand", PriceUnit=7, Quantity=163, CreateTime=DateTime.Parse("2021-4-18"), EndTime=DateTime.Parse("2022-9-3"), OfferType=true, Description="", ProductID=products[0].ID},
                new Offer{Name="Ceva de Mihaita", PriceUnit=9, Quantity=24, CreateTime=DateTime.Parse("2021-4-4"), EndTime=DateTime.Parse("2025-9-3"), OfferType=true, Description="Quality 2", ProductID=products[0].ID},
                new Offer{Name="Oferta lui Petrisor", PriceUnit=4, Quantity=35, CreateTime=DateTime.Parse("2021-4-2"), EndTime=DateTime.Parse("2021-8-3"), OfferType=true, Description="Genius delivery.", ProductID=products[0].ID},
            };
            offers.ForEach(s => context.Offers.Add(s));
            context.SaveChanges();
            /*
            var orders = new List<Order>
            {
                new Order{BuyerID=1, OfferID = 1, DesiredQuantity = 500},
                new Order{BuyerID=1, OfferID = 2, DesiredQuantity=300},
            };
            orders.ForEach(s => context.Orders.Add(s));
            context.SaveChanges();
            */
        }
    }
}
