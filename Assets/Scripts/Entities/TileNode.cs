using Unity.Entities;

public struct TileNode : IBufferElementData
{
    public Tile tile;           // La ficha
    public Entity previousTile; // Referencia al nodo anterior
    public Entity nextTile;     // Referencia al nodo siguiente
}