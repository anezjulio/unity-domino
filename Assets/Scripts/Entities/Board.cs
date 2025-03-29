using System.Collections.Generic;
using Unity.Entities;

public struct Board : IComponentData {
    public DynamicBuffer<TileNode> placedTiles;  // Lista de las fichas colocadas en el tablero

}