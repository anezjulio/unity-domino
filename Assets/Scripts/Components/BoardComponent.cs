using System.Collections.Generic;
using Unity.Entities;

public struct BoardComponent : IComponentData
{
    public int team1Score;
    public int team2Score;
    public DynamicBuffer<TileNodeComponent> placedTiles;

}