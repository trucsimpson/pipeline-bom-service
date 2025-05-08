using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BOMService.Infrastructure.Persistence.EFModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BuilderHouse> BuilderHouses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=105VNA255;Database=293-sql-pipeline-simpsonqastaging_Dec-26_13_23;TrustServerCertificate=True;User ID=sa;Password=1234@Sim");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BuilderHouse>(entity =>
        {
            entity.HasKey(e => e.HousesId);

            entity.ToTable("Builder_Houses", tb => tb.HasTrigger("BuilderHousesDelete"));

            entity.HasIndex(e => e.HousesName, "NCIX_Builder_Houses_HousesName").HasFillFactor(85);

            entity.HasIndex(e => e.HousesPlanNo, "NCIX_Builder_Houses_HousesPlanNo").HasFillFactor(85);

            entity.HasIndex(e => new { e.HousesPlanNo, e.HousesName }, "NCIX_Builder_Houses_HousesPlanNoHousesName").HasFillFactor(85);

            entity.HasIndex(e => e.HousesTotalSqf, "NCIX_Builder_Houses_HousesTotalSQF");

            entity.HasIndex(e => e.SeriesId, "NCIX_Builder_Houses_Series_Id").HasFillFactor(85);

            entity.Property(e => e.HousesId).HasColumnName("Houses_Id");
            entity.Property(e => e.HousesBasePrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("Houses_BasePrice");
            entity.Property(e => e.HousesBasementSqf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Houses_BasementSQF");
            entity.Property(e => e.HousesBathroomHouseComponentId).HasColumnName("Houses_BathroomHouseComponentId");
            entity.Property(e => e.HousesBathrooms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Houses_Bathrooms");
            entity.Property(e => e.HousesBedroomHouseComponentId).HasColumnName("Houses_BedroomHouseComponentId");
            entity.Property(e => e.HousesBedrooms)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Houses_Bedrooms");
            entity.Property(e => e.HousesExteriorDesigner).HasColumnName("Houses_ExteriorDesigner");
            entity.Property(e => e.HousesFirstSqf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Houses_FirstSQF");
            entity.Property(e => e.HousesFloorPlanner).HasColumnName("Houses_FloorPlanner");
            entity.Property(e => e.HousesGarage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Houses_Garage");
            entity.Property(e => e.HousesGarageHouseComponentId).HasColumnName("Houses_GarageHouseComponentId");
            entity.Property(e => e.HousesGarageSqf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Houses_GarageSQF");
            entity.Property(e => e.HousesHeatedSqf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Houses_HeatedSQF");
            entity.Property(e => e.HousesHomeStoryHouseComponentId).HasColumnName("Houses_HomeStoryHouseComponentId");
            entity.Property(e => e.HousesHomeStyle)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Houses_HomeStyle");
            entity.Property(e => e.HousesHomeStyleHouseComponentId).HasColumnName("Houses_HomeStyleHouseComponentId");
            entity.Property(e => e.HousesImgBasement)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Houses_ImgBasement");
            entity.Property(e => e.HousesImgFirstFloor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Houses_ImgFirstFloor");
            entity.Property(e => e.HousesImgSecondFloor)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Houses_ImgSecondFloor");
            entity.Property(e => e.HousesInfo)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("Houses_Info");
            entity.Property(e => e.HousesInteriorDesigner).HasColumnName("Houses_InteriorDesigner");
            entity.Property(e => e.HousesMediaCenter).HasColumnName("Houses_MediaCenter");
            entity.Property(e => e.HousesName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Houses_Name");
            entity.Property(e => e.HousesOptions).HasColumnName("Houses_Options");
            entity.Property(e => e.HousesPicFile)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Houses_PicFile");
            entity.Property(e => e.HousesPicFile2)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Houses_PicFile2");
            entity.Property(e => e.HousesPlanNo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Houses_PlanNo");
            entity.Property(e => e.HousesSecondSqf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Houses_SecondSQF");
            entity.Property(e => e.HousesStories)
                .HasDefaultValue((byte)0)
                .HasColumnName("Houses_Stories");
            entity.Property(e => e.HousesTotalSqf)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))")
                .HasColumnName("Houses_TotalSQF");
            entity.Property(e => e.SeriesId).HasColumnName("Series_Id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
