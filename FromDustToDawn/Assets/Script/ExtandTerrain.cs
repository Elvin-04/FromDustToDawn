using UnityEngine;

public class ExtandTerrain : MonoBehaviour
{
    public int side;
    CameraController cameraController;

    public void ExtandMap()
    {
        if(!cameraController) cameraController = Camera.main.GetComponent<CameraController>();
        TerrainGenerator.instance.AddTerrain(side);
        cameraController.SetMaxDistances();
    }
}
