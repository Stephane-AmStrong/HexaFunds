using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain;

internal interface IComplexPropertyConfiguration<TEntity> where TEntity : class
{
    ComplexPropertyBuilder<TEntity> Configure(ComplexPropertyBuilder<TEntity> builder);
}
