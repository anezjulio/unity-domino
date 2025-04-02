using UnityEngine;

using Unity.Entities;

public class GameInitializer : MonoBehaviour
{
    void Start()
    {
        World world = World.DefaultGameObjectInjectionWorld;
        EntityManager entityManager = world.EntityManager;

        // Crear la entidad con el StartMatchComponent
        Entity startMatchEntity = entityManager.CreateEntity(typeof(StartMatchComponent));
        Debug.Log("🎮 Partida iniciada: Se creó la entidad StartMatch.");


      // 🔹 FORZAR la ejecución de MatchSystem manualmente para debug
        var matchSystem = world.GetExistingSystemManaged<MatchSystem>();
        if (matchSystem != null)
        {
            Debug.Log("🐞 Ejecutando MatchSystem manualmente para debug...");
            matchSystem.Update(); // Ejecutar manualmente una vez
        }
        else
        {
            Debug.LogError("❌ MatchSystem no encontrado en el mundo de ECS.");
        }

    }
}