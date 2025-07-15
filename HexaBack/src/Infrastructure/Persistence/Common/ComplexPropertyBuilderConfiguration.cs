using Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Common;

internal static class ComplexPropertyBuilderConfiguration
{
    public static ComplexPropertyBuilder<TEntity> Configure<TEntity>(
        this ComplexPropertyBuilder<TEntity> propertyBuilder,
        IComplexPropertyConfiguration<TEntity> configuration)
        where TEntity : class
    {
        configuration.Configure(propertyBuilder);
        return propertyBuilder;
    }
}
