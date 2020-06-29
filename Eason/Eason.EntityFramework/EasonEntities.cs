namespace Eason.EntityFramework
{
    using Entities.Activity;
    using Entities.Authorization;
    using Entities.Banner;
    using Entities.Message;
    using Entities.News;
    using Entities.School;
    using Migrations;
    using System.Data.Entity;

    public class EasonEntities : DbContext
    {
        //您的上下文已配置为从您的应用程序的配置文件(App.config 或 Web.config)
        //使用“EasonEntities”连接字符串。默认情况下，此连接字符串针对您的 LocalDb 实例上的
        //“Eason.EntityFramework.EasonEntities”数据库。
        // 
        //如果您想要针对其他数据库和/或数据库提供程序，请在应用程序配置文件中修改“EasonEntities”
        //连接字符串。
        public EasonEntities()
            : base("name=EasonConnectionString")
        {
        }
        static EasonEntities()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EasonEntities, Configuration>());
        }

        //为您要在模型中包含的每种实体类型都添加 DbSet。有关配置和使用 Code First  模型
        //的详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=390109。

        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleComment> ArticleComments { get; set; }
        public virtual DbSet<ArticleCategory> Categories { get; set; }
      //  public virtual DbSet<ArticleChannel> Channels { get; set; }
        public virtual DbSet<PermissionRole> PermissionRoles { get; set; }
        public virtual DbSet<VideoBanner> VideoBanners { get; set; }
        public virtual DbSet<InfoBanner> InfoBanners { get; set; }
    
        public virtual DbSet<IndexBanner> IndexBanners { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<ShortMessage> ShortMessages { get; set; }
        public virtual DbSet<TotalMessage> TotalMessages { get; set; }
        public virtual DbSet<Forward> Forwards { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Role>().HasKey(m => m.id);
            modelBuilder.Entity<User>().HasKey(m => m.id);
            modelBuilder.Entity<UserRole>().HasKey(m => m.id).HasRequired(m => m.role).WithMany().HasForeignKey(l => l.roleId);
            modelBuilder.Entity<UserRole>().HasRequired(m => m.user).WithMany().HasForeignKey(l => l.userId);
            modelBuilder.Entity<Permission>().HasKey(m => m.id);
            modelBuilder.Entity<Article>().HasKey(m => m.id).HasRequired(m => m.category).WithMany().HasForeignKey(n => n.categoryId);
           // modelBuilder.Entity<Article>().HasRequired(m => m.channel).WithMany().HasForeignKey(n => n.channelId);
            modelBuilder.Entity<ArticleComment>().HasKey(m => m.id);
            modelBuilder.Entity<ArticleCategory>().HasKey(m => m.id);
           // modelBuilder.Entity<ArticleChannel>().HasKey(m => m.id);
            modelBuilder.Entity<PermissionRole>().HasKey(m => m.id).HasRequired(m => m.permission).WithMany().HasForeignKey(n => n.permissionId);
            modelBuilder.Entity<PermissionRole>().HasKey(m => m.id).HasRequired(m => m.role).WithMany().HasForeignKey(n => n.roleId);
            modelBuilder.Entity<VideoBanner>().HasKey(m => m.id);
            modelBuilder.Entity<InfoBanner>().HasKey(m => m.id);
            modelBuilder.Entity<IndexBanner>().HasKey(m => m.id);
            modelBuilder.Entity<Teacher>().HasKey(m => m.id);
            modelBuilder.Entity<Course>().HasKey(m => m.id);
            modelBuilder.Entity<ShortMessage>().HasKey(m => m.id);
            modelBuilder.Entity<TotalMessage>().HasKey(m => m.id);
            modelBuilder.Entity<Forward>().HasKey(m => m.hdname);
            base.OnModelCreating(modelBuilder);
        }
    }


    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}