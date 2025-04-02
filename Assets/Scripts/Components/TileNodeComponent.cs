using Unity.Entities;

public struct TileNodeComponent : IBufferElementData
{
    public TileComponent tile;
    public Entity previousTile; // Referencia al nodo anterior
    public Entity nextTile;     // Referencia al nodo siguiente
}