using System.Data.Entity.Migrations;

namespace Fync.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Context context)
        {
            base.Seed(context);
        }
    }
}
