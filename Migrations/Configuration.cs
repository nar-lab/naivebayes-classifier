using System.Collections.Generic;
using NBayes.Models;

namespace NBayes.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NBayes.Models.NbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(NBayes.Models.NbContext context)
        {
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

            var people = new List<Person>()
                             {
                                new Person() {PersonId = 1,Education = "Middle",Age="Old",Sex="Male",Acceptence = "Yes"},
                                new Person() {PersonId = 2,Education = "Low",Age="Teen",Sex="Male",Acceptence = "No"},
                                new Person() {PersonId = 3,Education = "High",Age="Middle",Sex="Female",Acceptence = "No"},
                                new Person() {PersonId = 4,Education = "Middle",Age="Middle",Sex="Male",Acceptence = "Yes"},
                                new Person() {PersonId = 5,Education = "Low",Age="Middle",Sex="Male",Acceptence = "Yes"},
                                new Person() {PersonId = 6,Education = "High",Age="Old",Sex="Female",Acceptence = "Yes"},
                                new Person() {PersonId = 7,Education = "Low",Age="Teen",Sex="Female",Acceptence = "No"},
                                new Person() {PersonId = 8,Education = "Middle",Age="Middle",Sex="Female",Acceptence = "Yes"}
                             };
            people.ForEach(s => context.People.AddOrUpdate(p => p.PersonId, s));
            context.SaveChanges();
        }
    }
}
