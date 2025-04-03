using Unity.Entities;

public struct RoundComponent : IComponentData
{
    public int roundNumber; // Número de la ronda actual
    public bool isActive; // Indica si la ronda está en juego
}