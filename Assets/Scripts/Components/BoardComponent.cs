using System.Collections.Generic;
using Unity.Entities;

public struct BoardComponent : IComponentData
{
    public DynamicBuffer<TileNodeComponent> placedTiles;  // Lista de las fichas colocadas en el tablero

}