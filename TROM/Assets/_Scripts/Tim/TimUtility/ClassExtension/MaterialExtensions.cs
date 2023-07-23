using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialExtensions
{
    public static void ChangeMaterial(this SkinnedMeshRenderer skinnedMeshRenderer, Material newMaterial)
    {
        // Get the current materials array
        Material[] currentMaterials = skinnedMeshRenderer.materials;

        // Create a new materials array with the same length as the current array
        Material[] modifiedMaterials = new Material[currentMaterials.Length];

        // Assign the new material to each element in the modifiedMaterials array
        for (int i = 0; i < currentMaterials.Length; i++)
        {
            modifiedMaterials[i] = newMaterial;
        }

        // Assign the modified materials array back to the SkinnedMeshRenderer
        skinnedMeshRenderer.materials = modifiedMaterials;
    }
}
