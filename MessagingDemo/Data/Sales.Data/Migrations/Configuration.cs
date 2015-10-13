namespace Sales.Data.Migrations
{
  using Models;
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;

  internal sealed class Configuration : DbMigrationsConfiguration<Sales.Data.Context.SalesContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(Sales.Data.Context.SalesContext context)
    {
      context.Products.AddOrUpdate(
        new Product { Id = Guid.NewGuid(), Description = "Star Wars Episode 1 : The Phantom Menace", Price = 9.99M },
        new Product { Id = Guid.NewGuid(), Description = "Star Wars Episode 2 : Attack of the Clones", Price = 9.99M },
        new Product { Id = Guid.NewGuid(), Description = "Star Wars Episode 3 : Revenge of the Sith", Price = 9.99M },
        new Product { Id = Guid.NewGuid(), Description = "Star Wars Episode 4 : A New Hope", Price = 9.99M },
        new Product { Id = Guid.NewGuid(), Description = "Star Wars Episode 5 : The Empire Strikes Back", Price = 9.99M },
        new Product { Id = Guid.NewGuid(), Description = "Star Wars Episode 6 : Return of the Jedi", Price = 9.99M },
        new Product { Id = Guid.NewGuid(), Description = "Star Wars Episode 7 : The Force Awakens", Price = 9.99M }
        );
    }
  }
}
