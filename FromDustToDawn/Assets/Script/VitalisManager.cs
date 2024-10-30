using System.Collections;
using UnityEngine;

public class VitalisManager : MonoBehaviour
{
    public TerrainGenerator mapGen;
    public LayerMask layerMask;
    public GameObject vitalisPrefab;

    [Header("Vitalis Generation Options")]
    public int maxVitalisByChunk = 5;
    public int vitalisAddedByObject = 5;
    public float timeBetweenVitalisSpawning = 2f;
    private int currentVitalisOnMap = 0;
    private int maxVitalisOnMap;

    public static VitalisManager instance;

    private int vitalis;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxVitalisOnMap = maxVitalisByChunk * mapGen.genOptions.chunkWidth * mapGen.genOptions.chunkHeight;
        StartCoroutine(SpawningVitalis());
    }

    public int GetVitalis()
    {
        return vitalis;
    }

    public void AddVitalis(int amount)
    {
        vitalis += amount;
        UIManager.instance.UpdateUIVitalisAmount();
    }

    public void RemoveVitalis(int amount)
    {
        vitalis = vitalis - amount < 0 ? vitalis = 0 : vitalis -= amount;
        UIManager.instance.UpdateUIVitalisAmount();
    }

    IEnumerator SpawningVitalis()
    {
        yield return new WaitForSeconds(timeBetweenVitalisSpawning);
        SpawnVitalisOnMap();
        StartCoroutine(SpawningVitalis());
    }

    private void SpawnVitalisOnMap()
    {
        if(currentVitalisOnMap < maxVitalisOnMap)
        {
            Instantiate(vitalisPrefab, SpawnCoordinate() + new Vector3(0,0.2f,0), Quaternion.identity);
            currentVitalisOnMap++;
        }
        
    }

    private Vector3 SpawnCoordinate()
    {
        Vector3 startRay = new Vector3(Random.Range(2, mapGen.genOptions.meshSize * mapGen.genOptions.chunkWidth - 2), 10, Random.Range(2, mapGen.genOptions.meshSize * mapGen.genOptions.chunkHeight - 2));
        RaycastHit hit;

        if (Physics.Raycast(startRay, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            return hit.point;
        }

        return Vector3.zero;

    }

    public void CollectVitalis(GameObject vitalis)
    {
        AddVitalis(vitalisAddedByObject);
        UIManager.instance.UpdateUIVitalisAmount();
        currentVitalisOnMap--;
        Destroy(vitalis);
    }

    public bool CanBuy(int price)
    {
        if(vitalis - price < 0)
            return false;

        return true;
    }
}
