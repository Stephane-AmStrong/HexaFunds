using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public static class AccountBehaviorConfiguration
{
    public static void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.Ignore(b => b.AccountBehavior);
        builder.ComplexProperty<AccountBehaviorData>("BehaviorData", c =>
        {
            c.Property(d => d.Type).HasColumnName("Discriminator").HasMaxLength(20).IsRequired();
            c.Property(d => d.OverdraftLimit).HasColumnName("OverdraftLimit");
            c.Property(d => d.BalanceCeiling).HasColumnName("BalanceCeiling");
        });
    }
}
