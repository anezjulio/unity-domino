using System;
using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    public int playerId;

    public DynamicBuffer<TileNodeComponent> tiles;

}