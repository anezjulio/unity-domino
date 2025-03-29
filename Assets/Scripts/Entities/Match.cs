using System;
using Unity.Entities;

public struct Match : IComponentData {
    public DateTime gameDate;
    public Board board;
    public DynamicBuffer<Player> Players;

    //public int[] moveHistory;      // Histórico de movimientos realizados
}