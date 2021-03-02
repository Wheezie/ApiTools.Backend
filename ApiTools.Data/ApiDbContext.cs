using ApiTools.Domain.Data;
using ApiTools.Domain.Data.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace ApiTools.Data
{
    public class ApiDbContext : IdentityDbContext<Account, Role, ulong>
    {
        /* Account */
        // Accounts are in the Users DbSet inherited from Identity
        public DbSet<AccountEmail> Emails { get; set; }
        public DbSet<AccountInvite> Invites { get; set; }
        public DbSet<AccountSession> Sessions { get; set; }

        /* Album */
        public DbSet<Album> Albums { get; set; }
        public DbSet<AlbumLike> AlbumLikes { get; set; }

        /* Blog */
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogUserAccess> BlogAccesses { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }

        /* Comment */
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentReply> CommentReplies { get; set; }

        /* Pictures */
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureLike> PictureLikes { get; set; }

        /* Role */
        // Roles are in the Roles DbSet inherited from Identity
        public DbSet<RolePermission> RolePermissions { get; set; }


        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Ignore<UserAccess>();
            builder.Ignore<ContentBase<uint>>();
            builder.Ignore<ContentBase<ulong>>();
            builder.Ignore<Like>();
            builder.Ignore<Post>();

            #region Account
            builder.Entity<Account>(account =>
            {
                // Ignores
                account.Ignore(a => a.FullName);
                account.Ignore(a => a.Confirmed);

                account.Property(a => a.UserName)
                    .HasMaxLength(32)
                    .IsRequired()
                    .IsUnicode();
                account.Property(a => a.NormalizedUserName)
                    .HasMaxLength(32)
                    .IsRequired()
                    .IsUnicode();

                account.Property(a => a.ConcurrencyStamp)
                    .HasMaxLength(36);

                account.Property(a => a.SecurityStamp)
                    .HasMaxLength(36);

                account.Property(a => a.FirstName)
                    .HasMaxLength(32)
                    .IsUnicode();

                account.Property(a => a.LastName)
                    .HasMaxLength(32)
                    .IsUnicode();

                account.Property(a => a.Description)
                    .HasMaxLength(1024)
                    .IsUnicode();

                account.Property(p => p.ProfileEmailId)
                    .HasMaxLength(255)
                    .IsUnicode();

                account.Property(p => p.Bio)
                    .HasMaxLength(255)
                    .IsUnicode();

                account.Property(p => p.Website)
                    .HasMaxLength(255)
                    .IsUnicode();

                account.HasOne(a => a.Picture)
                    .WithOne()
                    .OnDelete(DeleteBehavior.SetNull)
                    .IsRequired(false);

                account.HasMany(a => a.Emails)
                    .WithOne(e => e.Account)
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.SetNull);

                account.HasOne(a => a.ProfileEmail)
                    .WithOne()
                    .HasForeignKey<Account>(a => a.ProfileEmailId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .IsRequired(false);

                account.HasOne(a => a.PrimaryEmail)
                    .WithOne()
                    .HasForeignKey<Account>(a => a.Email)
                    .OnDelete(DeleteBehavior.Restrict);
                
                // Delete invite if user is removed.
                account.HasOne(a => a.Invite)
                    .WithOne(i => i.Acceptor)
                    .HasForeignKey<AccountInvite>(i => i.AcceptorId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Set inviter to null if inviter is removed.
                account.HasMany(a => a.Invites)
                    .WithOne(i => i.Inviter)
                    .HasForeignKey(i => i.InviterId)
                    .OnDelete(DeleteBehavior.SetNull);

                account.HasMany(a => a.Posts)
                    .WithOne(p => p.Account)
                    .HasForeignKey(p => p.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                account.HasMany(a => a.Albums)
                    .WithOne(a => a.Account)
                    .HasForeignKey(a => a.AccountId)
                    .OnDelete(DeleteBehavior.SetNull);

                account.HasOne(a => a.Role)
                    .WithMany(r => r.Members)
                    .HasForeignKey(a => a.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);

                account.HasMany(a => a.Sessions)
                    .WithOne(a => a.Account)
                    .OnDelete(DeleteBehavior.Cascade);

                account.HasIndex(a => a.UserName)
                    .IsUnique();
                account.HasIndex(a => a.NormalizedUserName)
                    .IsUnique();
            });
            builder.Entity<AccountEmail>(email =>
            {
                email.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsRequired()
                    .IsUnicode();

                email.HasKey(e => e.Email);
            });
            builder.Entity<AccountInvite>(invite =>
            {
                invite.Property(i => i.AcceptorId)
                    .IsRequired(false);

                invite.Property(i => i.InviterId)
                    .IsRequired(false);

                invite.HasKey(i => i.Token);
            });
            #endregion
            #region Album
            builder.Entity<Album>(album =>
            {
                album.Property(a => a.Name)
                    .HasMaxLength(48)
                    .IsRequired();

                album.Property(a => a.Description)
                    .HasMaxLength(256)
                    .IsRequired(false);

                album.HasOne(a => a.Account)
                    .WithMany(a => a.Albums)
                    .HasForeignKey(a => a.AccountId)
                    .OnDelete(DeleteBehavior.SetNull);

                album.HasMany(a => a.Pictures)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<AlbumLike>(like =>
            {
                like.HasKey(l => new { l.AlbumId, l.LikerId });

                like.HasOne(l => l.Liker)
                    .WithMany()
                    .HasForeignKey(l => l.LikerId)
                    .OnDelete(DeleteBehavior.Cascade);

                like.HasOne(l => l.Album)
                    .WithMany(a => a.Likes)
                    .HasForeignKey(l => l.AlbumId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion
            #region Blog
            builder.Entity<Blog>(blog =>
            {
                blog.Property(b => b.Name)
                    .HasMaxLength(48)
                    .IsRequired()
                    .IsUnicode();

                blog.Property(b => b.Description)
                    .HasMaxLength(256)
                    .IsUnicode();

                blog.HasOne(b => b.Creator)
                    .WithMany(a => a.Blogs)
                    .HasForeignKey(b => b.CreatorId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            builder.Entity<BlogUserAccess>(access =>
            {
                access.HasBaseType((Type)null);

                access.HasKey(ba => new { ba.BlogId, ba.AccountId });

                access.HasOne(ba => ba.Account)
                    .WithMany(a => a.BlogAccess)
                    .HasForeignKey(ba => ba.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                access.HasOne(ba => ba.Blog)
                    .WithMany(b => b.UserAccess)
                    .HasForeignKey(ba => ba.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<BlogRoleAccess>(access =>
            {
                access.HasBaseType((Type)null);

                access.HasKey(ba => new { ba.BlogId, ba.RoleId });

                access.HasOne(ba => ba.Role)
                    .WithMany(r => r.BlogAccess)
                    .HasForeignKey(ba => ba.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                access.HasOne(a => a.Blog)
                    .WithMany(b => b.RoleAccess)
                    .HasForeignKey(a => a.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<BlogPost>(post =>
            {
                post.HasBaseType((Type)null);

                post.HasKey(p => new { p.Id, p.BlogId });

                post.Property(p => p.AccountId)
                    .IsRequired(false);

                post.Property(p => p.Title)
                    .HasMaxLength(128)
                    .IsRequired()
                    .IsUnicode();

                post.Property(p => p.Content)
                    .HasColumnType("TEXT")
                    .HasMaxLength(65535)
                    .IsRequired()
                    .IsUnicode();

                post.HasOne(p => p.Account)
                    .WithMany(b => b.BlogPosts)
                    .HasForeignKey(p => p.AccountId)
                    .OnDelete(DeleteBehavior.SetNull);

                post.HasOne(p => p.Blog)
                    .WithMany(b => b.Posts)
                    .HasForeignKey(p => p.BlogId)
                    .OnDelete(DeleteBehavior.Cascade);

                post.HasMany(p => p.Comments)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion
            #region Comment
            builder.Entity<Comment>(comment =>
            {
                comment.Property(c => c.Content)
                    .HasMaxLength(1024)
                    .IsRequired()
                    .IsUnicode();

                comment.Property(c => c.AccountId)
                    .IsRequired();

                comment.HasOne(c => c.Account)
                    .WithMany()
                    .HasForeignKey(c => c.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<CommentReply>(reply =>
            {
                reply.HasKey(r => new { r.CommentId, r.ReplyId });

                reply.HasOne(r => r.Reply)
                    .WithOne()
                    .HasForeignKey<CommentReply>(r => r.ReplyId)
                    .OnDelete(DeleteBehavior.Cascade);

                reply.HasOne(r => r.Comment)
                    .WithMany(c => c.Replies)
                    .HasForeignKey(r => r.CommentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion
            #region Picture
            builder.Entity<Picture>(picture =>
            {
                picture.Property(p => p.Uploaded);

                picture.HasOne(p => p.Uploader)
                    .WithMany()
                    .HasForeignKey(p => p.UploaderId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            builder.Entity<PictureLike>(like =>
            {
                like.HasKey(l => new { l.PictureId, l.LikerId });

                like.HasOne(l => l.Picture)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(l => l.PictureId)
                    .OnDelete(DeleteBehavior.Cascade);

                like.HasOne(l => l.Liker)
                    .WithMany()
                    .HasForeignKey(l => l.LikerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion
            #region Role
            builder.Entity<Role>(role =>
            {
                role.HasKey(r => r.Id);

                role.Property(r => r.ConcurrencyStamp)
                    .HasMaxLength(36);

                role.Property(r => r.Name)
                    .HasMaxLength(32)
                    .IsRequired();

                role.Property(r => r.Description)
                    .HasMaxLength(512);

                role.HasData(
                    new Role { Id = 1, Targets = 1000, Name = "Admin", NormalizedName = "ADMIN", Description = "Default admin role", Disabled = false }
                );
            });
            builder.Entity<RolePermission>(perm =>
            {
                perm.HasKey(p => new { p.RoleId, p.Permission });

                perm.HasOne(p => p.Role)
                    .WithMany(r => r.Permissions)
                    .HasForeignKey(p => p.RoleId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                perm.HasData(
                    new RolePermission { RoleId = 1, Permission = "*", Target = 1000 }
                );
            });
            #endregion
            #region Session
            builder.Entity<AccountSession>(session =>
            {
                session.ToTable("sessions");

                session.HasKey(s => s.Id);

                session.Property(s => s.AccountId)
                    .IsRequired(false);

                session.Property(s => s.Ip)
                    .HasMaxLength(16);

                session.Property(s => s.LastIp)
                    .HasMaxLength(16);

                session.HasOne(s => s.Account)
                    .WithMany(s => s.Sessions)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            #endregion
            #region Timeline
            builder.Entity<TimelinePost>(post =>
            {
                post.HasBaseType((Type)null);

                post.Property(p => p.Title)
                    .HasMaxLength(100)
                    .IsRequired()
                    .IsUnicode();

                post.Property(p => p.Content)
                    .HasColumnType("TEXT")
                    .HasMaxLength(65535)
                    .IsRequired()
                    .IsUnicode();

                post.HasKey(p => new { p.Id, p.AccountId });

                post.HasMany(p => p.Comments)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

                post.HasMany(p => p.Pictures)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

                post.HasMany(p => p.Likes)
                    .WithOne(p => p.Post)
                    .HasForeignKey(p => new { p.PostId, p.AccountId });
            });
            builder.Entity<TimelinePostLike>(like =>
            {
                like.HasKey(l => new { l.AccountId, l.PostId, l.LikerId });

                like.HasOne(l => l.Account)
                    .WithMany()
                    .HasForeignKey(l => l.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                like.HasOne(l => l.Liker)
                    .WithMany()
                    .HasForeignKey(l => l.LikerId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            #endregion
            #region UTC-hack
            /* Current hack to set all datebase-related datetimes to UTC */
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            foreach (var entityType in builder.Model.GetEntityTypes())
                foreach (var property in entityType.GetProperties())
                    if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                        property.SetValueConverter(dateTimeConverter);
            #endregion
        }
    }
}
