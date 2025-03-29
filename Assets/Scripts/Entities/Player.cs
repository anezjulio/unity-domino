using Unity.Entities;

public struct Player : IComponentData {
    public int userName;

    public DynamicBuffer<Tile> tiles; 

}