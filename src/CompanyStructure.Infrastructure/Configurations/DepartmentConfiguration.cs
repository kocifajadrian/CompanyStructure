using CompanyStructure.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CompanyStructure.Infrastructure.Configurations
{
    public class DepartmentConfiguration : StructureNodeConfiguration<Department>
    {
        public override void Configure(EntityTypeBuilder<Department> builder)
        {
            base.Configure(builder);
        }
    }
}
