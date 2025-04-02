using Unity.Entities;

public partial class BoardSystem : SystemBase
{
    private Entity GetBoardEntity(EntityManager entityManager)
    {
        EntityQuery boardQuery = entityManager.CreateEntityQuery(typeof(BoardComponent));
        return boardQuery.IsEmpty ? Entity.Null : boardQuery.GetSingletonEntity();
    }

    private void AddTileToBoard(EntityManager entityManager, Entity boardEntity, TileComponent newTile, Entity previousTileEntity)
    {
        if (boardEntity == Entity.Null) return;

        DynamicBuffer<TileNodeComponent> boardTiles = entityManager.GetBuffer<TileNodeComponent>(boardEntity);

        // Crear un nuevo nodo de ficha
        TileNodeComponent newNode = new TileNodeComponent
        {
            tile = newTile,
            previousTile = previousTileEntity, // Establecer el nodo anterior
            nextTile = Entity.Null // Este será el último nodo, por ahora no hay siguiente
        };

        // Si hay un nodo anterior, establecer su "nextTile" al nuevo nodo
        if (previousTileEntity != Entity.Null)
        {
            var previousTileNode = entityManager.GetBuffer<TileNodeComponent>(previousTileEntity);
            previousTileNode[previousTileNode.Length - 1] = newNode; // Actualizar el nodo anterior
        }

        // Añadir el nuevo nodo al tablero
        boardTiles.Add(newNode);
    }

    protected override void OnUpdate()
    {
        // Lógica de verificación de movimientos en el tablero
    }
}