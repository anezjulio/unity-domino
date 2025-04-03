using Unity.Entities;
using Unity.Collections;
using UnityEngine;

public partial class MatchSystem : SystemBase
{
    private EntityCommandBufferSystem ecbSystem;

    protected override void OnCreate()
    {
        ecbSystem = World.GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

        Entities
            .WithStructuralChanges()
            .ForEach((Entity entity, ref StartMatchComponent startMatch) =>
            {
                EntityManager.RemoveComponent<StartMatchComponent>(entity);
                StartMatch(ecb);
            }).Run();
    }

    private void StartMatch(EntityCommandBuffer ecb)
    {
        Debug.Log("🚀 Iniciando la partida...");

        int numberOfPlayers = 4;
        var shuffledTiles = CreateTiles();

        // Crear entidad del Match
        Entity matchEntity = ecb.CreateEntity();
        ecb.AddComponent(matchEntity, new MatchComponent { hasStarted = true });

        // Agregar buffer de jugadores y fichas
        ecb.AddBuffer<MatchPlayersBuffer>(matchEntity);
        ecb.AddBuffer<TileNodeComponent>(matchEntity);

        // Crear jugadores
        CreatePlayers(ecb, matchEntity, shuffledTiles, numberOfPlayers);

        shuffledTiles.Dispose();
    }

    private void CreatePlayers(EntityCommandBuffer ecb, Entity matchEntity, NativeList<TileNodeComponent> shuffledTiles, int numberOfPlayers)
    {
        for (int i = 0; i < numberOfPlayers; i++)
        {
            Entity playerEntity = ecb.CreateEntity();
            ecb.AddComponent(playerEntity, new PlayerComponent { playerId = i });

            // Crear y asignar buffer de fichas al jugador
            var playerTilesBuffer = ecb.AddBuffer<TileNodeComponent>(playerEntity);
            for (int j = 0; j < 7; j++)
            {
                playerTilesBuffer.Add(shuffledTiles[i * 7 + j]);
            }

            // Agregar jugador al Match
            ecb.AppendToBuffer(matchEntity, new MatchPlayersBuffer { playerEntity = playerEntity });
        }
    }

    private NativeList<TileNodeComponent> CreateTiles()
    {
        NativeList<TileNodeComponent> allTiles = new NativeList<TileNodeComponent>(Allocator.Temp);
        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                allTiles.Add(new TileNodeComponent { tile = new TileComponent { aSide = i, bSide = j } });
            }
        }

        ShuffleTiles(ref allTiles);
        return allTiles;
    }

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
