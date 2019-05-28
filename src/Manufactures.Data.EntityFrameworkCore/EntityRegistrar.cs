// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.EntityFramework;
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

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });

            modelBuilder.Entity<GarmentPreparingItemReadModel>(etb =>
            {
                etb.ToTable("GarmentPreparingItems");
                etb.HasKey(e => e.Identity);

                etb.ApplyAuditTrail();
                etb.ApplySoftDelete();
            });
        }
    }
}
