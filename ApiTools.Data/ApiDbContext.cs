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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountEmail> Emails { get; set; }
        public DbSet<AccountInvite> Invites { get; set; }

        public ApiDbContext()
        {
        }

        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

                account.HasMany(a => a.Emails)
                    .WithOne(e => e.Account)
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.SetNull);

                account.HasOne(a => a.ProfileEmail)
                    .WithOne()
                    .HasForeignKey<Account>(a => a.ProfileEmailId);

                account.HasOne(a => a.PrimaryEmail)
                    .WithOne()
                    .HasForeignKey<Account>(a => a.Email);

                account.HasOne(a => a.Invite)
                    .WithOne(i => i.Acceptor)
                    .HasForeignKey<AccountInvite>(i => i.AcceptorId)
                    .OnDelete(DeleteBehavior.Cascade);

                account.HasMany(a => a.Invites)
                    .WithOne(i => i.Inviter)
                    .HasForeignKey(i => i.InviterId)
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
                like.HasKey(l => new { l.PictureId, l.AccountId });

                like.HasOne(l => l.Picture)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(l => l.PictureId)
                    .OnDelete(DeleteBehavior.Cascade);

                like.HasOne(l => l.Account)
                    .WithMany()
                    .HasForeignKey(l => l.AccountId)
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
            builder.Entity<JwtSession>(session =>
            {
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
