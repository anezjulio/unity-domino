using Unity.Entities;
using UnityEngine;

public partial class TurnValidationSystem : SystemBase
{
    protected override void OnUpdate()
    {
        int leftValue = -1, rightValue = -1;

        // Obtener los valores de los extremos del tablero
        foreach (var board in SystemAPI.Query<RefRO<BoardComponent>>())
        {
            var placedTiles = board.ValueRO.placedTiles;

            if (placedTiles.Length > 0)
            {
                leftValue = placedTiles[0].tile.aSide; // Extremo izquierdo
                rightValue = placedTiles[placedTiles.Length - 1].tile.bSide; // Extremo derecho
            }
        }

        // Validar el turno actual
        foreach (var turn in SystemAPI.Query<RefRW<TurnComponent>>())
        {
            int currentPlayerId = turn.ValueRO.currentPlayerId;

            // Buscar la entidad del Match para obtener los jugadores
            foreach (var (match, matchEntity) in SystemAPI.Query<RefRO<MatchComponent>>().WithEntityAccess())
            {
                // Obtener la lista de jugadores de la partida
                DynamicBuffer<MatchPlayersBuffer> matchPlayers = SystemAPI.GetBuffer<MatchPlayersBuffer>(matchEntity);

                foreach (var playerBuffer in matchPlayers)
                {
                    Entity playerEntity = playerBuffer.playerEntity;
                    if (!SystemAPI.Exists(playerEntity)) continue;

                    // Obtener los componentes del jugador
                    var player = SystemAPI.GetComponent<PlayerComponent>(playerEntity);
                    var tiles = SystemAPI.GetBuffer<TileNodeComponent>(playerEntity);
                    var selectedTile = SystemAPI.GetComponent<SelectedTileComponent>(playerEntity);

                    if (player.playerId == currentPlayerId)
                    {
                        TileNodeComponent selectedTileData = tiles[selectedTile.selectedTileIndex];

                        bool isValidMove = (selectedTileData.tile.aSide == leftValue || selectedTileData.tile.bSide == leftValue ||
                                            selectedTileData.tile.aSide == rightValue || selectedTileData.tile.bSide == rightValue);

                        if (!isValidMove)
                        {
                            Debug.Log($"🚫 Jugador {player.playerId} intentó jugar una ficha inválida. Debe seleccionar otra.");
                            return;
                        }

                        // Si la ficha es válida, se remueve del jugador y se coloca en el tablero
                        tiles.RemoveAt(selectedTile.selectedTileIndex);
                        foreach (var board in SystemAPI.Query<RefRW<BoardComponent>>())
                        {
                            board.ValueRW.placedTiles.Add(selectedTileData);
                        }

                        Debug.Log($"✅ Jugador {player.playerId} jugó una ficha válida.");
                        turn.ValueRW.hasPlayed = true; // Marcar turno como completado
                    }
                }
            }
        }
    }
}
