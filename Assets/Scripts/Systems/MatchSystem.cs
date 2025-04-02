using Unity.Entities;
using Unity.Collections;

public partial class MatchSystem : SystemBase
{    
    protected override void OnUpdate()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // Buscar la entidad que tiene el StartMatchComponent
        Entities
            .WithStructuralChanges()
            .ForEach((Entity entity, ref StartMatchComponent startMatch) =>
            {
                // Remover el StartMatchComponent para evitar que se ejecute de nuevo
                entityManager.RemoveComponent<StartMatchComponent>(entity);

                // Crear la partida
                StartMatch(entityManager);

            }).Run();

    }

    private void StartMatch(EntityManager entityManager)
    {
        // 1️⃣ Crear las fichas
        NativeList<TileNodeComponent> tiles = CreateTiles();
        
        // 2️⃣ Mezclar las fichas
        ShuffleTiles(ref tiles);

        // 3️⃣ Crear el tablero
        Entity boardEntity = CreateBoard(entityManager);

        // 4️⃣ Crear jugadores
        CreatePlayers(tiles, 4, entityManager);

        // 5️⃣ Crear la entidad del Match
        Entity matchEntity = entityManager.CreateEntity(typeof(MatchComponent));
        entityManager.SetComponentData(matchEntity, new MatchComponent
        {
            gameDate = System.DateTime.Now,
            boardEntity = boardEntity
        });

        // ⚠️ IMPORTANTE: Liberar memoria de las fichas
        tiles.Dispose();
    }


    private Entity CreateBoard(EntityManager entityManager)
    {
        Entity boardEntity = entityManager.CreateEntity(typeof(BoardComponent));
        entityManager.SetComponentData(boardEntity, new BoardComponent());
        return boardEntity;
    }

private void CreatePlayers(NativeList<TileNodeComponent> shuffledTiles, int numberOfPlayers, EntityManager entityManager)
{
    int tilesPerPlayer = (numberOfPlayers == 2) ? 14 : 7;
    int tileIndex = 0;

    for (int i = 0; i < numberOfPlayers; i++)
    {
        Entity playerEntity = entityManager.CreateEntity(typeof(PlayerComponent));
        entityManager.SetComponentData(playerEntity, new PlayerComponent { playerId = i });

        // Crear el DynamicBuffer para almacenar las fichas del jugador
        DynamicBuffer<TileNodeComponent> playerTiles = entityManager.AddBuffer<TileNodeComponent>(playerEntity);

        for (int j = 0; j < tilesPerPlayer; j++)
        {
            playerTiles.Add(new TileNodeComponent
            {
                tile = shuffledTiles[tileIndex++].tile,
                previousTile = Entity.Null,
                nextTile = Entity.Null
            });
        }
    }
}

    // Función para crear todas las piezas del dominó
    private NativeList<TileNodeComponent> CreateTiles()
    {
        NativeList<TileNodeComponent> allTiles = new NativeList<TileNodeComponent>(Allocator.TempJob);
        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                allTiles.Add(new TileNodeComponent { tile = new TileComponent { aSide = i, bSide = j }});
            }
        }
        return allTiles;
    }

    // Función para mezclar las piezas
    private void ShuffleTiles(ref NativeList<TileNodeComponent> tiles)
    {
        System.Random rng = new System.Random();
        int n = tiles.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            TileNodeComponent value = tiles[k];
            tiles[k] = tiles[n];
            tiles[n] = value;
        }
    }


}