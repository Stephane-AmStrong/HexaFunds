using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public static class AccountBehaviorConfiguration
{
    public static void Configure(EntityTypeBuilder<BankAccount> builder)
    {
        builder.Property(b => b.AccountBehavior)
            .HasConversion(new AccountBehaviorConverter())
            .HasColumnType("jsonb");
    }
}
