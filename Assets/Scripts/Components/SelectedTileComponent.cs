using Unity.Entities;

public struct SelectedTileComponent : IComponentData
{
    public int selectedTileIndex; // Índice de la ficha seleccionada en el buffer
}