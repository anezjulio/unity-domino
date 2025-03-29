using Unity.Entities;
using Unity.Collections;

public partial class MatchSystem : SystemBase
{
    // Función para crear todas las piezas del dominó
    private NativeList<Tile> CreateTiles()
    {
        NativeList<Tile> allTiles = new NativeList<Tile>(Allocator.TempJob);

        int index = 0;
        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                allTiles[index++] = new Tile { aSide = i, bSide = j };
            }
        }

        return allTiles;
    }

    // Función para mezclar las piezas
    private void ShuffleTiles(ref NativeList<Tile> tiles)
    {
        System.Random rng = new System.Random();
        int n = tiles.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Tile value = tiles[k];
            tiles[k] = tiles[n];
            tiles[n] = value;
        }
    }

    // Función para crear jugadores
    private void CreatePlayers(NativeList<Tile> shuffledTiles, int numberOfPlayers, EntityManager entityManager)
    {
        int tilesPerPlayer = (numberOfPlayers == 2) ? 14 : 7;
        int tileIndex = 0;

        for (int i = 0; i < numberOfPlayers; i++)
        {
            // Crear la entidad para el jugador
            Entity playerEntity = entityManager.CreateEntity(typeof(Player));

            // Asignar fichas al jugador
            var player = entityManager.GetComponentData<Player>(playerEntity);
            player.tiles.Clear(); // Limpiar las piezas antes de repartir
            for (int j = 0; j < tilesPerPlayer; j++)
            {
                player.tiles.Add(shuffledTiles[tileIndex++]);
            }
            entityManager.SetComponentData(playerEntity, player);
        }
    }

    
    protected override void OnUpdate()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // Crear y mezclar las piezas
        NativeList<Tile> tiles = CreateTiles();
        ShuffleTiles(ref tiles);

        // Crear jugadores
        CreatePlayers(tiles, 4, entityManager);  // Supón que son 4 jugadores, puede ser variable
    
        // Suponiendo que boardEntity es una referencia al tablero
        //Entity boardEntity = GetBoardEntity();

        // Agregar una ficha de ejemplo
        //AddTileToBoard(entityManager, boardEntity, new Tile { aSide = 3, bSide = 5 });

        // Imprimir el estado del tablero
        //PrintBoard(entityManager, boardEntity);
    }
}