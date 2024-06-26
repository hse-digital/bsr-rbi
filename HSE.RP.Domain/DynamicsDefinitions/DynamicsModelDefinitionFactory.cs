using HSE.RP.Domain.Entities;

namespace HSE.RP.Domain.DynamicsDefinitions;

public class DynamicsModelDefinitionFactory
{
    private readonly Dictionary<Type, IDynamicsModelDefinition> definitions = new()
    {
        [typeof(Contact)] = new ContactModelDefinition(),
        [typeof(Building)] = new BuildingModelDefinition(),
        [typeof(Block)] = new BlockModelDefinition(),
        [typeof(BuildingProfessionApplication)] = new BuildingProfessionApplicationModelDefinition(),
        [typeof(Structure)] = new StructureDefinition(),
    };

    public DynamicsModelDefinition<TEntity, TDynamicsEntity> GetDefinitionFor<TEntity, TDynamicsEntity>() where TEntity : Entity 
        where TDynamicsEntity : DynamicsEntity<TEntity>
    {
        if (definitions.TryGetValue(typeof(TEntity), out var definition))
        {
            return (definition as DynamicsModelDefinition<TEntity, TDynamicsEntity>)!;
        }

        throw new ArgumentException();
    }
}