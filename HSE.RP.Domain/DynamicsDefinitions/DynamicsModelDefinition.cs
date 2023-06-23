using HSE.RP.Domain.Entities;

namespace HSE.RP.Domain.DynamicsDefinitions;

public abstract class DynamicsModelDefinition<TEntity, TDynamicsEntity> : IDynamicsModelDefinition where TEntity : Entity
    where TDynamicsEntity : DynamicsEntity<TEntity>
{
    public abstract string Endpoint { get; }

    public abstract TDynamicsEntity BuildDynamicsEntity(TEntity entity);

    public abstract TEntity BuildEntity(TDynamicsEntity dynamicsEntity);
}

public interface IDynamicsModelDefinition
{
}