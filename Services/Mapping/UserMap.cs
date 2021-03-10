using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared;

namespace Services.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnName("id");

            builder.Property(x => x.Email)
                .HasColumnName("email")
                .HasMaxLength(150)
                .IsRequired();
            
            builder.Property(x => x.Password)
                .HasColumnName("password")
                .IsRequired();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasMaxLength(100);

            builder.Property(x => x.CreatedAt)
                .HasColumnName("createdat")
                .IsRequired();

            builder.Property(x => x.Active)
                .HasColumnName("active")
                .IsRequired()
                .HasDefaultValue(true);
        }
    }
}
