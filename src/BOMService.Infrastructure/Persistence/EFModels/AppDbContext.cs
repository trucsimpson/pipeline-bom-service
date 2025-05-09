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

    public virtual DbSet<BuilderHousesInCommunity> BuilderHousesInCommunities { get; set; }

    public virtual DbSet<ProductsProduct> ProductsProducts { get; set; }

    public virtual DbSet<ProductsProductsToBuildingPhase> ProductsProductsToBuildingPhases { get; set; }

    public virtual DbSet<ProductsProductsToStyle> ProductsProductsToStyles { get; set; }

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

        modelBuilder.Entity<BuilderHousesInCommunity>(entity =>
        {
            entity.HasKey(e => e.HousesInCommunityId).HasName("PK_HousesInCommunity");

            entity.ToTable("Builder_HousesInCommunity");

            entity.HasIndex(e => new { e.HousesInCommunityId, e.CommunitiesId, e.HousesId }, "Builder_HousesInCommunity_NCIndex001")
                .IsUnique()
                .HasFillFactor(85);

            entity.HasIndex(e => new { e.CommunitiesId, e.HousesId }, "NCIX_Builder_HousesInCommunity_CommunitiesIdHousesId").HasFillFactor(85);

            entity.HasIndex(e => e.CommunitiesId, "NCIX_Builder_HousesInCommunity_Communities_id").HasFillFactor(85);

            entity.HasIndex(e => e.HousesId, "NCIX_Builder_HousesInCommunity_Houses_Id").HasFillFactor(85);

            entity.Property(e => e.HousesInCommunityId).HasColumnName("HousesInCommunity_Id");
            entity.Property(e => e.CommunitiesId).HasColumnName("Communities_id");
            entity.Property(e => e.HousesId).HasColumnName("Houses_Id");
            entity.Property(e => e.HousesInCommunityNotes)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("HousesInCommunity_Notes");
            entity.Property(e => e.HousesInCommunityPrice)
                .HasColumnType("money")
                .HasColumnName("HousesInCommunity_Price");

            entity.HasOne(d => d.Houses).WithMany(p => p.BuilderHousesInCommunities)
                .HasForeignKey(d => d.HousesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HousesInCommunity_Houses");
        });

        modelBuilder.Entity<ProductsProduct>(entity =>
        {
            entity.HasKey(e => e.ProductsId).HasName("PK_Products");

            entity.ToTable("Products_Products", tb => tb.HasTrigger("Trigger_CascadeToProductsToStyles"));

            entity.HasIndex(e => new { e.ProductsAccuracy, e.ProductsRounding, e.ProductsWaste }, "NCIX_ProductsProducts_ProductAccuracyProductRoundingProductWaste");

            entity.HasIndex(e => e.ProductsName, "NCIX_Products_Products_ProductsName").HasFillFactor(85);

            entity.HasIndex(e => e.ProductsSupplementalToBid, "NCIX_Products_Products_ProductsSupplementalToBidTrue").HasFilter("([Products_SupplementalToBid]=(1))");

            entity.HasIndex(e => e.ProductsName, "Products_Products_Products_Name_INC_Products_Id").HasFillFactor(85);

            entity.Property(e => e.ProductsId).HasColumnName("Products_Id");
            entity.Property(e => e.ProductUnitTypesId).HasColumnName("ProductUnitTypes_Id");
            entity.Property(e => e.ProductsAccuracy)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((1))")
                .HasColumnName("Products_Accuracy");
            entity.Property(e => e.ProductsDescription)
                .HasMaxLength(4000)
                .IsUnicode(false)
                .HasColumnName("Products_Description");
            entity.Property(e => e.ProductsIsArchived)
                .HasDefaultValue(false)
                .HasColumnName("Products_IsArchived");
            entity.Property(e => e.ProductsName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Products_Name");
            entity.Property(e => e.ProductsNotes)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("Products_Notes");
            entity.Property(e => e.ProductsProjectedCost)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("Products_ProjectedCost");
            entity.Property(e => e.ProductsRounding)
                .HasDefaultValue((short)0)
                .HasColumnName("Products_Rounding");
            entity.Property(e => e.ProductsSupplementalToBid).HasColumnName("Products_SupplementalToBid");
            entity.Property(e => e.ProductsWaste)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("Products_Waste");
        });

        modelBuilder.Entity<ProductsProductsToBuildingPhase>(entity =>
        {
            entity.HasKey(e => e.ProductsToBuildingPhasesId).HasName("PK_ProductsToBuildingPhases");

            entity.ToTable("Products_ProductsToBuildingPhases");

            entity.HasIndex(e => e.BuildingPhasesId, "NCIX_Products_ProductsToBuildingPhases_BuildingPhasesId").HasFillFactor(85);

            entity.HasIndex(e => e.ProductsId, "NCIX_Products_ProductsToBuildingPhases_ProductsId").HasFillFactor(85);

            entity.HasIndex(e => new { e.ProductsId, e.BuildingPhasesId }, "NCIX_Products_ProductsToBuildingPhases_ProductsIdBuildingPhasesId").HasFillFactor(85);

            entity.HasIndex(e => e.ProductsId, "NCIX_Products_ProductsToBuildingPhases_ProductsIdIsDefaultInPhaseTrue").HasFilter("([ProductsToBuildingPhases_IsDefault]=(1))");

            entity.Property(e => e.ProductsToBuildingPhasesId).HasColumnName("ProductsToBuildingPhases_Id");
            entity.Property(e => e.BuildingPhasesId).HasColumnName("BuildingPhases_Id");
            entity.Property(e => e.ProductsId).HasColumnName("Products_Id");
            entity.Property(e => e.ProductsToBuildingPhasesIsDefault).HasColumnName("ProductsToBuildingPhases_IsDefault");
            entity.Property(e => e.ProductsToBuildingPhasesTaxable)
                .HasDefaultValue((short)-1)
                .HasColumnName("ProductsToBuildingPhases_Taxable");

            entity.HasOne(d => d.Products).WithMany(p => p.ProductsProductsToBuildingPhases)
                .HasForeignKey(d => d.ProductsId)
                .HasConstraintName("FK_ProductsToBuildingPhases_Products");
        });

        modelBuilder.Entity<ProductsProductsToStyle>(entity =>
        {
            entity.HasKey(e => e.ProductsToStylesId);

            entity.ToTable("Products_ProductsToStyles", tb =>
                {
                    tb.HasTrigger("Products_ProductsToStyles_DeleteTrigger");
                    tb.HasTrigger("Products_ProductsToStyles_UpdateTrigger");
                });

            entity.HasIndex(e => new { e.ProductsId, e.StylesId }, "NCIX_Products_ProductsToStyles_ProductIdStyleId");

            entity.HasIndex(e => e.ProductsId, "NCIX_Products_ProductsToStyles_ProductsId").HasFillFactor(85);

            entity.HasIndex(e => new { e.ProductsId, e.ProductsToStylesProductCode }, "NCIX_Products_ProductsToStyles_ProductsIdProductsToStylesProductCode").HasFillFactor(85);

            entity.HasIndex(e => e.ProductsId, "NCIX_Products_ProductsToStyles_ProductsIdProductsToStylesProductCodeIsDefaultTrue").HasFilter("([ProductsToStyles_IsDefault]=(1))");

            entity.HasIndex(e => e.StylesId, "NCIX_Products_ProductsToStyles_StylesId");

            entity.Property(e => e.ProductsToStylesId).HasColumnName("ProductsToStyles_Id");
            entity.Property(e => e.ProductsId).HasColumnName("Products_Id");
            entity.Property(e => e.ProductsToStylesIsDefault).HasColumnName("ProductsToStyles_IsDefault");
            entity.Property(e => e.ProductsToStylesProductCode)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ProductsToStyles_ProductCode");
            entity.Property(e => e.StylesId).HasColumnName("Styles_Id");

            entity.HasOne(d => d.Products).WithMany(p => p.ProductsProductsToStyles)
                .HasForeignKey(d => d.ProductsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_ProductsToStyles_Products_Products");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
