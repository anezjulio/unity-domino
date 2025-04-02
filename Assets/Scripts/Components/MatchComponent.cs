using System;
using Unity.Entities;

public struct MatchComponent : IComponentData
{
    public DateTime gameDate;
    public Entity boardEntity;
    public DynamicBuffer<PlayerComponent> Players;
}