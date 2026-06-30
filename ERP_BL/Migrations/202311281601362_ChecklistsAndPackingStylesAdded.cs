namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChecklistsAndPackingStylesAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Checklists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        creationDate = c.DateTime(),
                        TransactionType = c.Int(nullable: false),
                        creatorId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.creatorId)
                .Index(t => t.creatorId);
            
            CreateTable(
                "dbo.PackingStyles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Tasks", "checklistId", c => c.Int());
            AddColumn("dbo.ProcurementProducts", "DispatchedQuantity", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "DispatchedWeight", c => c.Double(nullable: false));
            AddColumn("dbo.ProcurementProducts", "PackingDimensions", c => c.String());
            AddColumn("dbo.ProcurementProducts", "packingStyleId", c => c.Int());
            AddColumn("dbo.ProcurementProducts", "Checklist_Id", c => c.Int());
            CreateIndex("dbo.Tasks", "checklistId");
            CreateIndex("dbo.ProcurementProducts", "packingStyleId");
            CreateIndex("dbo.ProcurementProducts", "Checklist_Id");
            AddForeignKey("dbo.ProcurementProducts", "packingStyleId", "dbo.PackingStyles", "Id");
            AddForeignKey("dbo.ProcurementProducts", "Checklist_Id", "dbo.Checklists", "Id");
            AddForeignKey("dbo.Tasks", "checklistId", "dbo.Checklists", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "checklistId", "dbo.Checklists");
            DropForeignKey("dbo.ProcurementProducts", "Checklist_Id", "dbo.Checklists");
            DropForeignKey("dbo.ProcurementProducts", "packingStyleId", "dbo.PackingStyles");
            DropForeignKey("dbo.Checklists", "creatorId", "dbo.Users");
            DropIndex("dbo.ProcurementProducts", new[] { "Checklist_Id" });
            DropIndex("dbo.ProcurementProducts", new[] { "packingStyleId" });
            DropIndex("dbo.Checklists", new[] { "creatorId" });
            DropIndex("dbo.Tasks", new[] { "checklistId" });
            DropColumn("dbo.ProcurementProducts", "Checklist_Id");
            DropColumn("dbo.ProcurementProducts", "packingStyleId");
            DropColumn("dbo.ProcurementProducts", "PackingDimensions");
            DropColumn("dbo.ProcurementProducts", "DispatchedWeight");
            DropColumn("dbo.ProcurementProducts", "DispatchedQuantity");
            DropColumn("dbo.Tasks", "checklistId");
            DropTable("dbo.PackingStyles");
            DropTable("dbo.Checklists");
        }
    }
}
