using Unity.Entities;

public struct ScoreComponent : IComponentData
{
    public int team1Score; // Puntos del equipo 1 (jugadores 1 y 3)
    public int team2Score; // Puntos del equipo 2 (jugadores 2 y 4)
    public int winningScore; // Puntaje necesario para ganar (50 o 100)
}