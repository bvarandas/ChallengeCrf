using ChallengeCrf.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChallengeCrf.Infra.Data.Mappings
{
    //public class CashFlowMap : IEntityTypeConfiguration<CashFlow>
    //{
    //    public void Configure( EntityTypeBuilder<CashFlow> builder)
    //    {
    //        builder.Property(c => c.CashFlowId)
    //            .HasColumnName("CashFlowId").
    //            ValueGeneratedOnAdd();

    //        builder.Property(c => c.Description)
    //            .HasColumnType("varchar(100)")
    //            .HasMaxLength(100)
    //            .IsRequired();

    //        builder.Property(c => c.Amount)
    //            .HasColumnType("float(100)")
    //            .HasMaxLength(120);

    //        builder.Property(c => c.Entry)
    //            .HasColumnType("varchar(1)")
    //            .HasMaxLength(100);

    //        builder.Property(c => c.Date)
    //            .HasColumnType("DateTime");

    //    }
    //}
}
