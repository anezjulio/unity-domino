using Unity.Entities;
using Unity.Collections;


public struct PlayerComponent : IComponentData
{
    public int playerId;
    public FixedString64Bytes playerName;

    public DynamicBuffer<TileNodeComponent> tiles;

}