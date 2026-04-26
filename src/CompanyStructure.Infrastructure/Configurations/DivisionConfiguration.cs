using CompanyStructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyStructure.Infrastructure.Configurations
{
    public class DivisionConfiguration : StructureNodeConfiguration<Division>
    {
        public override void Configure(EntityTypeBuilder<Division> builder)
        {
            base.Configure(builder);

            builder.HasMany(d => d.Projects)
                .WithOne(p => p.Division)
                .HasForeignKey(p => p.DivisionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
