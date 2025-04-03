using Unity.Entities;
using Unity.Collections;
using UnityEngine;

public partial class RoundSystem : SystemBase
{
    protected override void OnUpdate()
    {
        foreach (var round in SystemAPI.Query<RefRW<RoundComponent>>())
        {
            if (!round.ValueRO.isActive) // Si la ronda no está activa, iniciar una nueva
            {
                round.ValueRW.roundNumber++;
                round.ValueRW.isActive = true;
                Debug.Log($"🔄 Iniciando Ronda {round.ValueRO.roundNumber}");

                // Asignar el primer turno al jugador con el doble 6
                AssignFirstTurn();
            }
        }
    }

    private void AssignFirstTurn()
    {
        int firstPlayerId = -1;

        // Usar un QueryBuilder para evitar conflictos en SystemAPI.Query
        var playerEntities = SystemAPI.QueryBuilder()
            .WithAll<PlayerComponent, TileNodeComponent>()
            .Build()
            .ToEntityArray(Allocator.Temp);

        foreach (var playerEntity in playerEntities)
        {
            var player = SystemAPI.GetComponent<PlayerComponent>(playerEntity);
            var tiles = SystemAPI.GetBuffer<TileNodeComponent>(playerEntity);

            foreach (var tile in tiles)
            {
                if (tile.tile.aSide == 6 && tile.tile.bSide == 6)
                {
                    firstPlayerId = player.playerId;
                    break;
                }
            }

            if (firstPlayerId != -1)
                break;
        }

        playerEntities.Dispose(); // Liberar la memoria del array

        if (firstPlayerId != -1)
        {
            // Crear o actualizar el TurnComponent con el jugador inicial
            Entity turnEntity = EntityManager.CreateEntity(typeof(TurnComponent));
            EntityManager.SetComponentData(turnEntity, new TurnComponent { currentPlayerId = firstPlayerId, hasPlayed = false });

            Debug.Log($"🎲 Jugador {firstPlayerId} inicia la partida con el doble 6.");
        }
    }
}
