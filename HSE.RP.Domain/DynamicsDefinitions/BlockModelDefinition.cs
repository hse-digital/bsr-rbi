using HSE.RP.Domain.Entities;

namespace HSE.RP.Domain.DynamicsDefinitions;

public class BlockModelDefinition : DynamicsModelDefinition<Block, DynamicsBlock>
{
    public override string Endpoint => "bsr_blocks";
    
    public override DynamicsBlock BuildDynamicsEntity(Block entity)
    {
        throw new NotImplementedException();
    }

    public override Block BuildEntity(DynamicsBlock dynamicsEntity)
    {
        throw new NotImplementedException();
    }
}