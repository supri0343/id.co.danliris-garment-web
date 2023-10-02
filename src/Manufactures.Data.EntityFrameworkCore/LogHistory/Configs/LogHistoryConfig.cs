
using Manufactures.Domain.LogHistories.ReadModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.LogHistories.Configs
{
    public class LogHistoryConfig : IEntityTypeConfiguration<LogHistoryReadModel>
    {
        public void Configure(EntityTypeBuilder<LogHistoryReadModel> builder)
        {
            builder.ToTable("LogHistories");
            builder.HasKey(e => e.Identity);

            builder.Property(a => a.Division).HasMaxLength(255);
            builder.Property(a => a.Activity).HasMaxLength(1000);
        }
    }
}
