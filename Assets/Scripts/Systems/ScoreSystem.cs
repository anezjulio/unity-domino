using Unity.Entities;
using Unity.Collections;
using UnityEngine;

public partial class ScoreSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // Verificar si la ronda ha terminado
        if (!HasEndRoundComponent())
            return;

        // Calcular los puntajes de los equipos
        (int team1Points, int team2Points) = CalculateScores();

        // Mostrar los puntajes en la consola
        Debug.Log($"🏆 Fin de Ronda - Puntajes:");
        Debug.Log($"🟦 Equipo 1 (Jugadores 0 y 2): {team1Points} puntos");
        Debug.Log($"🟥 Equipo 2 (Jugadores 1 y 3): {team2Points} puntos");

        // Actualizar los puntajes en el tablero
        UpdateBoardScores(team1Points, team2Points);

        // Remover EndRoundComponent de las entidades correspondientes
        RemoveEndRoundComponent();
    }

    private bool HasEndRoundComponent()
    {
        return !SystemAPI.QueryBuilder().WithAll<EndRoundComponent>().Build().IsEmpty;
    }

    private (int, int) CalculateScores()
    {
        int team1Points = 0;
        int team2Points = 0;

        foreach (var (player, tiles) in SystemAPI.Query<RefRO<PlayerComponent>, DynamicBuffer<TileNodeComponent>>())
        {
            int sum = 0;
            foreach (var tile in tiles)
            {
                sum += tile.tile.aSide + tile.tile.bSide;
            }

            if (player.ValueRO.playerId % 2 == 0)
                team1Points += sum;
            else
                team2Points += sum;
        }

        return (team1Points, team2Points);
    }

    private void UpdateBoardScores(int team1Points, int team2Points)
    {
        foreach (var board in SystemAPI.Query<RefRW<BoardComponent>>())
        {
            board.ValueRW.team1Score += team1Points;
            board.ValueRW.team2Score += team2Points;
        }
    }

    private void RemoveEndRoundComponent()
    {
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (var entity in SystemAPI.QueryBuilder().WithAll<EndRoundComponent>().Build().ToEntityArray(Allocator.Temp))
        {
            ecb.RemoveComponent<EndRoundComponent>(entity);
        }
        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}
