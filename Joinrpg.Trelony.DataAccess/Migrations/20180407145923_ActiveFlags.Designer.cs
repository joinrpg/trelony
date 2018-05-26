﻿// <auto-generated />
using Joinrpg.Trelony.DataAccess;
using Joinrpg.Trelony.DataModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Joinrpg.Trelony.DataAccess.Migrations
{
    [DbContext(typeof(TrelonyContext))]
    [Migration("20180407145923_ActiveFlags")]
    partial class ActiveFlags
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Joinrpg.Trelony.DataModel.Game", b =>
                {
                    b.Property<int>("GameId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Email");

                    b.Property<string>("FacebookLink");

                    b.Property<string>("GameName");

                    b.Property<int>("GameStatus");

                    b.Property<int>("GameType");

                    b.Property<string>("GameUrl");

                    b.Property<string>("LivejournalLink");

                    b.Property<string>("Organizers");

                    b.Property<int?>("PlayersCount");

                    b.Property<int?>("PolygonId");

                    b.Property<int>("SubRegionId");

                    b.Property<string>("TelegramLink");

                    b.Property<string>("VkontakteLink");

                    b.HasKey("GameId");

                    b.HasIndex("PolygonId");

                    b.HasIndex("SubRegionId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Joinrpg.Trelony.DataModel.GameDate", b =>
                {
                    b.Property<int>("GameDateId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Actual");

                    b.Property<int>("GameDurationDays");

                    b.Property<int>("GameId");

                    b.Property<DateTime>("GameStartDate");

                    b.HasKey("GameDateId");

                    b.HasIndex("GameId");

                    b.ToTable("GameDate");
                });

            modelBuilder.Entity("Joinrpg.Trelony.DataModel.MacroRegion", b =>
                {
                    b.Property<int>("MacroRegionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MacroRegionName")
                        .IsRequired();

                    b.HasKey("MacroRegionId");

                    b.ToTable("MacroRegions");
                });

            modelBuilder.Entity("Joinrpg.Trelony.DataModel.Polygon", b =>
                {
                    b.Property<int>("PolygonId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("PolygonName")
                        .IsRequired();

                    b.Property<int>("SubRegionId");

                    b.HasKey("PolygonId");

                    b.HasIndex("SubRegionId");

                    b.ToTable("Polygons");
                });

            modelBuilder.Entity("Joinrpg.Trelony.DataModel.SubRegion", b =>
                {
                    b.Property<int>("SubRegionId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MacroRegionId");

                    b.Property<string>("SubRegionName")
                        .IsRequired();

                    b.HasKey("SubRegionId");

                    b.HasIndex("MacroRegionId");

                    b.ToTable("SubRegions");
                });

            modelBuilder.Entity("Joinrpg.Trelony.DataModel.Game", b =>
                {
                    b.HasOne("Joinrpg.Trelony.DataModel.Polygon", "Polygon")
                        .WithMany()
                        .HasForeignKey("PolygonId");

                    b.HasOne("Joinrpg.Trelony.DataModel.SubRegion", "SubRegion")
                        .WithMany()
                        .HasForeignKey("SubRegionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Joinrpg.Trelony.DataModel.GameDate", b =>
                {
                    b.HasOne("Joinrpg.Trelony.DataModel.Game", "Game")
                        .WithMany("Dates")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Joinrpg.Trelony.DataModel.Polygon", b =>
                {
                    b.HasOne("Joinrpg.Trelony.DataModel.SubRegion", "SubRegion")
                        .WithMany("Polygons")
                        .HasForeignKey("SubRegionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Joinrpg.Trelony.DataModel.SubRegion", b =>
                {
                    b.HasOne("Joinrpg.Trelony.DataModel.MacroRegion", "MacroRegion")
                        .WithMany("SubRegions")
                        .HasForeignKey("MacroRegionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}