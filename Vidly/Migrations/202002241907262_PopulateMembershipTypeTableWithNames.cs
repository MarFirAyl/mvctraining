namespace Vidly.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateMembershipTypeTableWithNames : DbMigration
    {
        public override void Up()
        {
            Sql("UPDATE MembershipTypes set Name = 'Pay as You Go' WHERE id = 1");
            Sql("UPDATE MembershipTypes set Name = 'Monthly' WHERE id = 2");
            Sql("UPDATE MembershipTypes set Name = 'Quarterly' WHERE id = 3");
            Sql("UPDATE MembershipTypes set Name = 'Yearly' WHERE id = 4");
        }
        
        public override void Down()
        {
        }
    }
}
