using System;
using Unity.Entities;

public struct MatchComponent : IComponentData
{
    public bool hasStarted; 
    public DateTime gameDate;
    public Entity boardEntity;    
}

// Agregar un buffer para almacenar las entidades de los jugadores
[InternalBufferCapacity(4)] // Si son 4 jugadores, podemos definir un tamaño inicial
public struct MatchPlayersBuffer : IBufferElementData
{
    public Entity playerEntity;
}