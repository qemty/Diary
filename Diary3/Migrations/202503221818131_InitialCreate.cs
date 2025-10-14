namespace Diary3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

            
            CreateTable(
                "dbo.LabWorks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LessonId = c.Int(nullable: false),
                        Topic = c.String(),
                        Hours = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lessons", t => t.LessonId, cascadeDelete: true)
                .Index(t => t.LessonId);
            
            CreateTable(
                "dbo.Lessons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TeacherId = c.Int(nullable: false),
                        SubjectId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        Place = c.String(nullable: false),
                        DayOfWeek = c.Int(nullable: false),
                        PairNumber = c.Int(nullable: false),
                        WeekNumber = c.Int(nullable: false),
                        IsReplacement = c.Boolean(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Subjects", t => t.SubjectId, cascadeDelete: true)
                .ForeignKey("dbo.Teacher", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId)
                .Index(t => t.SubjectId)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.Subjects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Teacher",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LessonNotes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayOfWeek = c.Int(nullable: false),
                        PairNumber = c.Int(nullable: false),
                        Notes = c.String(),
                        TeacherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teacher", t => t.TeacherId, cascadeDelete: true)
                .Index(t => t.TeacherId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LessonNotes", "TeacherId", "dbo.Teacher");
            DropForeignKey("dbo.LabWorks", "LessonId", "dbo.Lessons");
            DropForeignKey("dbo.Lessons", "TeacherId", "dbo.Teacher");
            DropForeignKey("dbo.Lessons", "SubjectId", "dbo.Subjects");
            DropForeignKey("dbo.Lessons", "GroupId", "dbo.Groups");
            DropIndex("dbo.LessonNotes", new[] { "TeacherId" });
            DropIndex("dbo.Lessons", new[] { "GroupId" });
            DropIndex("dbo.Lessons", new[] { "SubjectId" });
            DropIndex("dbo.Lessons", new[] { "TeacherId" });
            DropIndex("dbo.LabWorks", new[] { "LessonId" });
            DropTable("dbo.LessonNotes");
            DropTable("dbo.Teacher");
            DropTable("dbo.Subjects");
            DropTable("dbo.Lessons");
            DropTable("dbo.LabWorks");
            DropTable("dbo.Groups");
        }
    }
}
