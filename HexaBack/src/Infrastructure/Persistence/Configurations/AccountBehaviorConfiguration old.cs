// using System;
// using System.Linq;
// using Domain.Entities;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Metadata.Builders;
// using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

// namespace Persistence.Configurations;

// public static class AccountBehaviorConfiguration
// {
//     public static void Configure(EntityTypeBuilder<BankAccount> builder)
//     {
//         // Utilise un champ de backing pour stocker AccountBehavior (pour le convertir manuellement)
//         builder.Ignore(b => b.AccountBehavior);

//         // Discriminant
//         builder.Property<string>("AccountBehaviorDiscriminator")
//             .HasColumnName("AccountBehavior");

//         // Valeurs spécifiques selon le comportement
//         builder.Property<float?>("OverdraftLimit");
//         builder.Property<float?>("BalanceCeiling");

//         // Eventuellement : Initialisation manuelle via materializer (non-trivial, voir ci-dessous)
//     }

//     // Tu peux avoir des helpers ici pour gérer la conversion
//     public static IAccountBehavior ToBehavior(string? discriminator, float? overdraft, float? ceiling) =>
//         discriminator switch
//         {
//             "Checking" when overdraft is not null => new CheckingBehavior(overdraft.Value),
//             "Savings" when ceiling is not null => new SavingsBehavior(ceiling.Value),
//             _ => throw new InvalidOperationException("Invalid AccountBehavior data.")
//         };

//     public static string FromBehavior(IAccountBehavior behavior) =>
//         behavior switch
//         {
//             CheckingBehavior => "Checking",
//             SavingsBehavior => "Savings",
//             _ => throw new ArgumentOutOfRangeException(nameof(behavior))
//         };

//     public static float? ExtractOverdraft(IAccountBehavior behavior) =>
//         behavior is CheckingBehavior checking ? checking.OverdraftLimit : null;

//     public static float? ExtractCeiling(IAccountBehavior behavior) =>
//         behavior is SavingsBehavior savings ? savings.BalanceCeiling : null;
// }
