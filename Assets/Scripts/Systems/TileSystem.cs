using Unity.Entities;
using Unity.Collections;

public partial class TileSystem : SystemBase {

    // Función para mezclar las piezas
    private NativeArray<TileComponent> ShuffleTiles()
    {
        NativeArray<TileComponent> allTiles = new NativeArray<TileComponent>(28, Allocator.TempJob);

        // Crear todas las fichas (por ejemplo, 0-0, 0-1, ..., 6-6)
        int index = 0;
        for (int i = 0; i <= 6; i++)
        {
            for (int j = i; j <= 6; j++)
            {
                allTiles[index++] = new TileComponent { aSide = i, bSide = j };
            }
        }

        // Revolver las fichas
        System.Random rng = new System.Random();
        int n = allTiles.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            TileComponent value = allTiles[k];
            allTiles[k] = allTiles[n];
            allTiles[n] = value;
        }

        return allTiles;
    }



    protected override void OnUpdate() {
       
    }
}