using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile] // Optimización para ECS
public partial struct TurnSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var turn in SystemAPI.Query<RefRW<TurnComponent>>())
        {
            if (turn.ValueRO.hasPlayed) // Si el jugador ya jugó, pasa el turno
            {
                int nextPlayerId = (turn.ValueRO.currentPlayerId + 1) % 4; // Ciclo entre los jugadores

                // Buscar el siguiente jugador con fichas disponibles
                while (!HasTiles(ref state, nextPlayerId))
                {
                    nextPlayerId = (nextPlayerId + 1) % 4;
                }

                turn.ValueRW.currentPlayerId = nextPlayerId;
                turn.ValueRW.hasPlayed = false;
                Debug.Log($"🎲 Turno del jugador {turn.ValueRO.currentPlayerId}");
            }
        }
    }

    private bool HasTiles(ref SystemState state, int playerId)
    {
        // Eliminamos la palabra 'static' para permitir el uso de SystemAPI.Query<> dentro del método
        foreach (var (player, tiles) in SystemAPI.Query<PlayerComponent, DynamicBuffer<TileNodeComponent>>())
        {
            if (player.playerId == playerId && tiles.Length > 0)
            {
                return true; // El jugador tiene fichas
            }
        }
        return false; // El jugador no tiene fichas
    }
}
