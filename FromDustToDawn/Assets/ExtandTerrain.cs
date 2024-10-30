using UnityEngine;

public class ExtandTerrain : MonoBehaviour
{
    public int side;

    public void ExtandMap()
    {
        TerrainGenerator.instance.AddTerrain(side);
    }
}
