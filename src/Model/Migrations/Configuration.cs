using System.Data.Entity.Migrations.Model;

namespace Model.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using sd = Model.Seed;

    public class Configuration : DbMigrationsConfiguration<Model.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationDataLossAllowed = true;
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Model.DatabaseContext context)
        {
            var seed = new sd.Seed();
            seed.Execute(context);
        }
    }
}
