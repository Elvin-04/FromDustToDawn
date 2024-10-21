using UnityEngine;
using System.Collections.Generic;

using UnityEditor;

public class optionsGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject waterPlane;

    public GenerationOptions genOptions; 



    private void Start()
    {
        genOptions.seed = Random.Range(1, 1000000);

        GenerateMap();
        UpdateChunkValues();
    }

    private void OnValidate()
    {
        UpdateChunkValues();
    }

    //Update chunks values depending of the GenerationOptions struct
    public void UpdateChunkValues()
    {
        foreach (Chunk chunk in GetComponentsInChildren<Chunk>())
        {
            chunk.GenerateTerrain(genOptions);
        }
    }

    //Create one chunk and set the position
    private void CreateChunk(Vector3 position)
    {
        GameObject obj = Instantiate(chunkPrefab, this.transform);

        obj.transform.position = position;
    }

    //Generate all the chunks and set the water options
    private void GenerateMap()
    {
        // Ajouter des chunks supplémentaires autour de la carte pour les falaises
        for (int x = 0; x <= genOptions.chunkWidth; x++)
        {
            for (int y = 0; y <= genOptions.chunkHeight; y++)
            {
                Vector3 pos = new Vector3(x * genOptions.meshSize, 0, y * genOptions.meshSize);
                CreateChunk(pos);
            }
        }

        // Ajuster la taille et la position du plan d'eau
        waterPlane.transform.localScale = new Vector3(genOptions.meshSize / 10 * (genOptions.chunkWidth + 2), 1, genOptions.meshSize / 10 * (genOptions.chunkHeight + 2));
        waterPlane.transform.position = new Vector3(genOptions.meshSize * genOptions.chunkWidth / 2, genOptions.waterLevel, genOptions.meshSize * genOptions.chunkHeight / 2);
    }

}

[System.Serializable]
public struct GenerationOptions
{
    [Header("   Chunk Options")]
    [Range(1, 256)]
    public int gridSize;
    [Range(0, 32)]
    public float meshSize;
    public float heightMultiplier;
    [Range(1, 10)]
    public int chunkWidth;
    [Range(1, 10)]
    public int chunkHeight;

    [Header("   Noise Options")]
    [Range(0f, 0.5f)]
    public float perlinScale;
    [Range(0, 1)]
    public float lacunarity;
    [Range(0, 1)]
    public float persistance;
    [Range(1, 8)]
    public int octaveCount;

    [Header("   Generation Options")]
    public float waterLevel;
    
    public int seed;
}
