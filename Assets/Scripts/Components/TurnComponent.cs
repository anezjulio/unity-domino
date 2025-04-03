using Unity.Entities;

public struct TurnComponent : IComponentData
{
    public int currentPlayerId; // ID del jugador que tiene el turno
    public bool hasPlayed; // Indica si el jugador ya hizo un movimiento
}