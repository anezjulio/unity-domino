using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile] // Optimización para ECS
public partial struct TurnAdvanceSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        foreach (var turn in SystemAPI.Query<RefRW<TurnComponent>>())
        {
            if (turn.ValueRO.hasPlayed)
            {
                int nextPlayerId = (turn.ValueRO.currentPlayerId + 1) % 4; // Rotación de 4 jugadores
                turn.ValueRW.currentPlayerId = nextPlayerId;
                turn.ValueRW.hasPlayed = false;

                Debug.Log($"🔄 Cambio de turno: Ahora juega el jugador {nextPlayerId}");
            }
        }
    }
}
