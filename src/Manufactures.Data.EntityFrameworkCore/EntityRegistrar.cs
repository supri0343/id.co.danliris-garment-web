// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.EntityFramework;
using Manufactures.Data.EntityFrameworkCore.GarmentCuttingIns.Configs;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Data.EntityFrameworkCore
{
    public class EntityRegistrar : IEntityRegistrar
    {
        public void RegisterEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GarmentPreparingReadModel>(etb =>
            {
                etb.ToTable("GarmentPreparings");
                etb.HasKey(e => e.Identity);

                etb.Property(o => o.UENNo)
                   .HasMaxLength(100);
                etb.Property(o => o.UnitCode)
                   .HasMaxLength(25);
                etb.Property(o => o.UnitName)
                   .HasMaxLength(100);
                etb.Property(o => o.RONo)
                   .HasMaxLength(100);
                etb.Property(o => o.Article)
                   .HasMaxLength(500);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<GarmentPreparingItemReadModel>(etb =>
            {
                etb.ToTable("GarmentPreparingItems");
                etb.HasKey(e => e.Identity);
                etb.HasOne(a => a.GarmentPreparingIdentity)
                   .WithMany(a => a.GarmentPreparingItem)
                   .HasForeignKey(a => a.GarmentPreparingId);

                etb.Property(o => o.ProductCode)
                   .HasMaxLength(25);
                etb.Property(o => o.ProductName)
                   .HasMaxLength(100);
                etb.Property(o => o.UomUnit)
                   .HasMaxLength(100);
                etb.Property(o => o.DesignColor)
                   .HasMaxLength(100);
                etb.Property(o => o.FabricType)
                   .HasMaxLength(100);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<GarmentAvalProductReadModel>(etb =>
            {
                etb.ToTable("GarmentAvalProducts");
                etb.HasKey(e => e.Identity);

                etb.Property(a => a.RONo)
                   .HasMaxLength(100);
                etb.Property(a => a.Article)
                   .HasMaxLength(100);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<GarmentAvalProductItemReadModel>(etb =>
            {
                etb.ToTable("GarmentAvalProductItems");
                etb.HasKey(e => e.Identity);
                etb.HasOne(a => a.GarmentAvalProductIdentity)
                   .WithMany(a => a.GarmentAvalProductItem)
                   .HasForeignKey(a => a.APId);

                etb.Property(o => o.ProductCode)
                   .HasMaxLength(25);
                etb.Property(o => o.ProductName)
                   .HasMaxLength(100);
                etb.Property(o => o.DesignColor)
                   .HasMaxLength(100);
                etb.Property(o => o.UomUnit)
                   .HasMaxLength(100);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.ApplyConfiguration(new GarmentCuttingInConfig());
            modelBuilder.ApplyConfiguration(new GarmentCuttingInItemConfig());
            modelBuilder.ApplyConfiguration(new GarmentCuttingInDetailConfig());

            modelBuilder.Entity<GarmentDeliveryReturnReadModel>(etb =>
            {
                etb.ToTable("GarmentDeliveryReturns");
                etb.HasKey(e => e.Identity);

                etb.Property(a => a.DRNo)
                   .HasMaxLength(25);
                etb.Property(a => a.RONo)
                   .HasMaxLength(100);
                etb.Property(a => a.Article)
                   .HasMaxLength(100);
                etb.Property(a => a.UnitDONo)
                   .HasMaxLength(100);
                etb.Property(a => a.ReturnType)
                   .HasMaxLength(25);
                etb.Property(a => a.UnitCode)
                   .HasMaxLength(25);
                etb.Property(a => a.UnitName)
                   .HasMaxLength(100);
                etb.Property(a => a.StorageCode)
                   .HasMaxLength(25);
                etb.Property(a => a.StorageName)
                   .HasMaxLength(100);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<GarmentDeliveryReturnItemReadModel>(etb =>
            {
                etb.ToTable("GarmentDeliveryReturnItems");
                etb.HasKey(e => e.Identity);
                etb.HasOne(a => a.GarmentDeliveryReturnIdentity)
                   .WithMany(a => a.GarmentDeliveryReturnItem)
                   .HasForeignKey(a => a.DRId);

                etb.Property(a => a.ProductCode)
                   .HasMaxLength(25);
                etb.Property(a => a.ProductName)
                   .HasMaxLength(100);
                etb.Property(a => a.DesignColor)
                   .HasMaxLength(100);
                etb.Property(a => a.RONo)
                   .HasMaxLength(100);
                etb.Property(a => a.UomUnit)
                   .HasMaxLength(100);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });
        }
    }
}
