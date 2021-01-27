using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiTools.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(512) CHARACTER SET utf8mb4", maxLength: 512, nullable: true),
                    Disabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Targets = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Name = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    NormalizedName = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(36) CHARACTER SET utf8mb4", maxLength: 36, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Permission = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Target = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.Permission });
                    table.ForeignKey(
                        name: "FK_RolePermissions_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumLikes",
                columns: table => new
                {
                    LikerId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    AlbumId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Unliked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumLikes", x => new { x.AlbumId, x.LikerId });
                });

            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Uploaded = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UploaderId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    State = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    StateChanged = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AlbumId = table.Column<uint>(type: "int unsigned", nullable: true),
                    TimelinePostAccountId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    TimelinePostId = table.Column<uint>(type: "int unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    RoleId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: true),
                    LastName = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: true),
                    Description = table.Column<string>(type: "varchar(1024) CHARACTER SET utf8mb4", maxLength: 1024, nullable: true),
                    Bio = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    Website = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    ProfileEmailId = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: true),
                    ShowName = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    ShowEmail = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    ShowWebsite = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    PictureId = table.Column<Guid>(type: "char(36)", nullable: true),
                    RegisteredDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BlockedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RoleId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    NormalizedEmail = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    UserName = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "varchar(32) CHARACTER SET utf8mb4", maxLength: 32, nullable: false),
                    Email = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    SecurityStamp = table.Column<string>(type: "varchar(36) CHARACTER SET utf8mb4", maxLength: 36, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(36) CHARACTER SET utf8mb4", maxLength: 36, nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Pictures_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AccountId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    Name = table.Column<string>(type: "varchar(48) CHARACTER SET utf8mb4", maxLength: 48, nullable: false),
                    Description = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Name = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", nullable: false),
                    Value = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(48) CHARACTER SET utf8mb4", maxLength: 48, nullable: false),
                    Description = table.Column<string>(type: "varchar(256) CHARACTER SET utf8mb4", maxLength: 256, nullable: true),
                    CreatorId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Visibility = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    Email = table.Column<string>(type: "varchar(255) CHARACTER SET utf8mb4", maxLength: 255, nullable: false),
                    AccountId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    Type = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Verified = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Emails_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    Token = table.Column<Guid>(type: "char(36)", nullable: false),
                    InviterId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    Invited = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Expire = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AcceptorId = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => x.Token);
                    table.ForeignKey(
                        name: "FK_Invites_AspNetUsers_AcceptorId",
                        column: x => x.AcceptorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invites_AspNetUsers_InviterId",
                        column: x => x.InviterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PictureLikes",
                columns: table => new
                {
                    LikerId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    PictureId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Unliked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PictureLikes", x => new { x.PictureId, x.LikerId });
                    table.ForeignKey(
                        name: "FK_PictureLikes_AspNetUsers_LikerId",
                        column: x => x.LikerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PictureLikes_Pictures_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    AccountId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    Browser = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Platform = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Issued = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    LastUse = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ip = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: true),
                    LastIp = table.Column<byte[]>(type: "varbinary(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_sessions_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "TimelinePost",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    AccountId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 65535, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Title = table.Column<string>(type: "varchar(100) CHARACTER SET utf8mb4", maxLength: 100, nullable: false),
                    Visibility = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelinePost", x => new { x.Id, x.AccountId });
                    table.ForeignKey(
                        name: "FK_TimelinePost_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogAccesses",
                columns: table => new
                {
                    AccountId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    BlogId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Access = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogAccesses", x => new { x.BlogId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_BlogAccesses_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogAccesses_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "int unsigned", nullable: false),
                    BlogId = table.Column<uint>(type: "int unsigned", nullable: false),
                    AccountId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    Content = table.Column<string>(type: "TEXT", maxLength: 65535, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Title = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Visibility = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => new { x.Id, x.BlogId });
                    table.ForeignKey(
                        name: "FK_BlogPosts_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BlogPosts_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogRoleAccess",
                columns: table => new
                {
                    RoleId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    BlogId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Access = table.Column<byte>(type: "tinyint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogRoleAccess", x => new { x.BlogId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_BlogRoleAccess_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogRoleAccess_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimelinePostLike",
                columns: table => new
                {
                    LikerId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    AccountId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    PostId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Unliked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimelinePostLike", x => new { x.AccountId, x.PostId, x.LikerId });
                    table.ForeignKey(
                        name: "FK_TimelinePostLike_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimelinePostLike_AspNetUsers_LikerId",
                        column: x => x.LikerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimelinePostLike_TimelinePost_PostId_AccountId",
                        columns: x => new { x.PostId, x.AccountId },
                        principalTable: "TimelinePost",
                        principalColumns: new[] { "Id", "AccountId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BlogPostBlogId = table.Column<uint>(type: "int unsigned", nullable: true),
                    BlogPostId = table.Column<uint>(type: "int unsigned", nullable: true),
                    TimelinePostAccountId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    TimelinePostId = table.Column<uint>(type: "int unsigned", nullable: true),
                    AccountId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    Content = table.Column<string>(type: "varchar(1024) CHARACTER SET utf8mb4", maxLength: 1024, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_AccountId",
                        column: x => x.AccountId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_BlogPosts_BlogPostId_BlogPostBlogId",
                        columns: x => new { x.BlogPostId, x.BlogPostBlogId },
                        principalTable: "BlogPosts",
                        principalColumns: new[] { "Id", "BlogId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_TimelinePost_TimelinePostId_TimelinePostAccountId",
                        columns: x => new { x.TimelinePostId, x.TimelinePostAccountId },
                        principalTable: "TimelinePost",
                        principalColumns: new[] { "Id", "AccountId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentReplies",
                columns: table => new
                {
                    CommentId = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    ReplyId = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReplies", x => new { x.CommentId, x.ReplyId });
                    table.ForeignKey(
                        name: "FK_CommentReplies_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentReplies_Comments_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Disabled", "Name", "NormalizedName", "Targets" },
                values: new object[] { 1ul, "47fc8208-cd44-413c-9445-73693e06118e", "Default admin role", false, "Admin", "ADMIN", 1000ul });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "Permission", "RoleId", "Target" },
                values: new object[] { "*", 1ul, 1000ul });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumLikes_LikerId",
                table: "AlbumLikes",
                column: "LikerId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_AccountId",
                table: "Albums",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PictureId",
                table: "AspNetUsers",
                column: "PictureId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileEmailId",
                table: "AspNetUsers",
                column: "ProfileEmailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogAccesses_AccountId",
                table: "BlogAccesses",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_AccountId",
                table: "BlogPosts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_BlogId",
                table: "BlogPosts",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_BlogRoleAccess_RoleId",
                table: "BlogRoleAccess",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CreatorId",
                table: "Blogs",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReplies_ReplyId",
                table: "CommentReplies",
                column: "ReplyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AccountId",
                table: "Comments",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogPostId_BlogPostBlogId",
                table: "Comments",
                columns: new[] { "BlogPostId", "BlogPostBlogId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TimelinePostId_TimelinePostAccountId",
                table: "Comments",
                columns: new[] { "TimelinePostId", "TimelinePostAccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_Emails_AccountId",
                table: "Emails",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_AcceptorId",
                table: "Invites",
                column: "AcceptorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invites_InviterId",
                table: "Invites",
                column: "InviterId");

            migrationBuilder.CreateIndex(
                name: "IX_PictureLikes_LikerId",
                table: "PictureLikes",
                column: "LikerId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_AlbumId",
                table: "Pictures",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_TimelinePostId_TimelinePostAccountId",
                table: "Pictures",
                columns: new[] { "TimelinePostId", "TimelinePostAccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_Pictures_UploaderId",
                table: "Pictures",
                column: "UploaderId");

            migrationBuilder.CreateIndex(
                name: "IX_sessions_AccountId",
                table: "sessions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelinePost_AccountId",
                table: "TimelinePost",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelinePostLike_LikerId",
                table: "TimelinePostLike",
                column: "LikerId");

            migrationBuilder.CreateIndex(
                name: "IX_TimelinePostLike_PostId_AccountId",
                table: "TimelinePostLike",
                columns: new[] { "PostId", "AccountId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumLikes_Albums_AlbumId",
                table: "AlbumLikes",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumLikes_AspNetUsers_LikerId",
                table: "AlbumLikes",
                column: "LikerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_Albums_AlbumId",
                table: "Pictures",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_AspNetUsers_UploaderId",
                table: "Pictures",
                column: "UploaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Pictures_TimelinePost_TimelinePostId_TimelinePostAccountId",
                table: "Pictures",
                columns: new[] { "TimelinePostId", "TimelinePostAccountId" },
                principalTable: "TimelinePost",
                principalColumns: new[] { "Id", "AccountId" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Emails_Email",
                table: "AspNetUsers",
                column: "Email",
                principalTable: "Emails",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Emails_ProfileEmailId",
                table: "AspNetUsers",
                column: "ProfileEmailId",
                principalTable: "Emails",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_Albums_AlbumId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Emails_AspNetUsers_AccountId",
                table: "Emails");

            migrationBuilder.DropForeignKey(
                name: "FK_Pictures_AspNetUsers_UploaderId",
                table: "Pictures");

            migrationBuilder.DropForeignKey(
                name: "FK_TimelinePost_AspNetUsers_AccountId",
                table: "TimelinePost");

            migrationBuilder.DropTable(
                name: "AlbumLikes");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BlogAccesses");

            migrationBuilder.DropTable(
                name: "BlogRoleAccess");

            migrationBuilder.DropTable(
                name: "CommentReplies");

            migrationBuilder.DropTable(
                name: "Invites");

            migrationBuilder.DropTable(
                name: "PictureLikes");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "sessions");

            migrationBuilder.DropTable(
                name: "TimelinePostLike");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "Pictures");

            migrationBuilder.DropTable(
                name: "TimelinePost");
        }
    }
}
