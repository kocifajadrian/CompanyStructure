using CompanyStructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyStructure.Infrastructure.Configurations
{
    public abstract class StructureNodeConfiguration<T>
        : IEntityTypeConfiguration<T> where T : StructureNode
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(n => n.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasOne(n => n.Manager)
                .WithMany()
                .HasForeignKey(n => n.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
