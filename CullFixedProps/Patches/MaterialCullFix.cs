using System.Collections;
using UWE;
using UnityEngine;

namespace CullFixedProps.Plants;

public static class MaterialCullFix
{
    // These are the class ids of all prefabs we want to fix
    private static readonly string[] TargetClassIds = new[]
    {
        "4d0c8cbd-6127-4681-9d86-d9175e6df722", // Orange Flower 1
        "7ce83863-b4f2-4d46-9f46-1830799f3e5f", // Orange Flower 2
        "2d422d6b-3c1f-484d-84ee-a07b5b8e32a4", // Seacrown light
        "a1040915-abcf-4843-a16f-39a10d6a1c2d", // Seacrown
        "37ea521a-6be4-437c-8ed7-6b453d9218a8", // Large Floater
        "b78912bc-0191-4455-a9de-3b708e165393", // Cuddlefish Egg
        "0a993944-87d3-441e-b21d-6c314f723cc7", // Hoverfish 
    };
    
    // This runs during the early loading screen to then modify the materials
    public static IEnumerator RegisterAsync()
    {
        foreach (var classId in TargetClassIds)
        {
            IPrefabRequest task = PrefabDatabase.GetPrefabAsync(classId);
            yield return task;

            if (task.TryGetPrefab(out GameObject prefab))
            {
                ModifyPrefabMaterials(prefab);
            }
        }
    }

    // modify the materials of the listed class ids to turn off back-face culling
    private static void ModifyPrefabMaterials(GameObject prefab)
    {
        var renderers = prefab.GetComponentsInChildren<Renderer>(true);
        foreach (var renderer in renderers)
        {
            if (renderer.materials != null)
            {
                foreach (var material in renderer.materials)
                {
                    if (material != null && material.HasProperty("_MyCullVariable"))
                    {
                        material.SetFloat("_MyCullVariable", 0f);
                    }
                }
            }
        }
    }
}
