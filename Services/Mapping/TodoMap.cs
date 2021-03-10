using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared;

namespace Services.Mapping
{
    public class TodoMap : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable("todos");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id");
            builder.Property(x => x.Description)
                .HasColumnName("description")
                .IsRequired();
            builder.Property(x => x.Done)
                .HasColumnName("done");
        }
    }
}
