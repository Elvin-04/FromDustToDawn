using UnityEngine;
using System.Collections.Generic;

using UnityEditor;
using Unity.VisualScripting.FullSerializer;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    public GameObject waterPlane;

    public GenerationOptions genOptions; 

    public static TerrainGenerator instance;

    public GameObject addTerrainButtonPrefab;

    private GameObject addButtonWidth, addButtonHeight;
    private float initSize;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        genOptions.seed = Random.Range(1, 1000000);

        GenerateAllTerrain();
    }

    public void AddTerrain(int side)
    {
        if (side == 0) genOptions.chunkWidth++;
        if (side == 1) genOptions.chunkHeight++;

        GenerateAllTerrain();
    }

    private void GenerateAllTerrain()
    {
        GenerateMap();
        UpdateChunkValues();
        CreateAddTerrainButtons();
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
        for (int x = 0; x < genOptions.chunkWidth; x++)
        {
            for (int y = 0; y < genOptions.chunkHeight; y++)
            {
                Vector3 pos = new Vector3(x * genOptions.meshSize, 0, y * genOptions.meshSize);
                CreateChunk(pos);
            }
        }

        waterPlane.transform.localScale = new Vector3(genOptions.meshSize / 10 * (genOptions.chunkWidth), 1, genOptions.meshSize / 10 * (genOptions.chunkHeight));
        waterPlane.transform.position = new Vector3(genOptions.meshSize * genOptions.chunkWidth / 2, genOptions.waterLevel, genOptions.meshSize * genOptions.chunkHeight / 2);
    }

    public void CreateAddTerrainButtons()
    {
        if(addButtonWidth == null)
            addButtonWidth = Instantiate(addTerrainButtonPrefab);
        if(addButtonHeight == null)
        {
            addButtonHeight = Instantiate(addTerrainButtonPrefab); 
            initSize = addButtonHeight.GetComponent<RectTransform>().sizeDelta.x;

        }
       

        addButtonWidth.transform.position = new Vector3(genOptions.meshSize * ((float)genOptions.chunkWidth + 0.5f) + 1, genOptions.waterLevel, (genOptions.meshSize * genOptions.chunkHeight) / 2);
        addButtonWidth.GetComponent<ExtandTerrain>().side = 0;
        RectTransform buttonWidthRT = addButtonWidth.GetComponent<RectTransform>();
        buttonWidthRT.sizeDelta = new Vector2(initSize, initSize * genOptions.chunkHeight);

        addButtonHeight.transform.position = new Vector3((genOptions.meshSize * genOptions.chunkWidth) / 2, genOptions.waterLevel, genOptions.meshSize * ((float)genOptions.chunkHeight + 0.5f) + 1);
        addButtonHeight.GetComponent<ExtandTerrain>().side = 1;
        RectTransform buttonHeightRT = addButtonHeight.GetComponent<RectTransform>();
        buttonHeightRT.sizeDelta = new Vector2(initSize * genOptions.chunkWidth, initSize);
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
