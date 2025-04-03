using Unity.Entities;
using UnityEngine;

public partial class PlayerSelectionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        int leftValue = 0, rightValue = 0;

        // Obtener los extremos del tablero desde placedTiles
        foreach (var (board, placedTiles) in SystemAPI.Query<RefRO<BoardComponent>, DynamicBuffer<TileNodeComponent>>())
        {
            if (placedTiles.Length > 0)
            {
                leftValue = placedTiles[0].tile.aSide; // Lado izquierdo de la primera ficha
                rightValue = placedTiles[^1].tile.bSide; // Lado derecho de la última ficha
            }
        }

        foreach (var (turn, turnEntity) in SystemAPI.Query<RefRW<TurnComponent>>().WithEntityAccess())
        {
            int currentPlayerId = turn.ValueRO.currentPlayerId;

            foreach (var (player, tiles, playerEntity) in SystemAPI.Query<RefRW<PlayerComponent>, DynamicBuffer<TileNodeComponent>>().WithEntityAccess())
            {
                if (player.ValueRO.playerId == currentPlayerId)
                {
                    int selectedIndex = -1;

                    // Buscar la primera ficha válida
                    for (int i = 0; i < tiles.Length; i++)
                    {
                        var tile = tiles[i].tile;
                        if (tile.aSide == leftValue || tile.bSide == leftValue ||
                            tile.aSide == rightValue || tile.bSide == rightValue)
                        {
                            selectedIndex = i;
                            break;
                        }
                    }

                    if (selectedIndex != -1)
                    {
                        // Agregar el componente de selección con el índice de la ficha seleccionada
                        EntityManager.SetComponentData(playerEntity, new SelectedTileComponent { selectedTileIndex = selectedIndex });

                        Debug.Log($"🎯 Jugador {player.ValueRO.playerId} seleccionó la ficha en índice {selectedIndex}");
                    }
                    else
                    {
                        Debug.Log($"🚫 Jugador {player.ValueRO.playerId} no tiene fichas válidas, pasando turno.");
                        turn.ValueRW.hasPlayed = true; // Si no hay fichas, pasa el turno automáticamente
                    }
                }
            }
        }
    }
}
