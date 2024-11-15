using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExtandTerrain : MonoBehaviour
{
    public int side;
    CameraController cameraController;
    public GameObject widthPrice;
    public GameObject lengthPrice;

    private int currentLevel = 0;
    public int maxLevel = 3;

    public List<int> prices;

    public void SetSide(int side)
    {
        if (currentLevel >= maxLevel) return;
        this.side = side;
        if(side == 0 )
        {
            widthPrice.SetActive(false);
            lengthPrice.GetComponent<TextMeshProUGUI>().text = prices[currentLevel].ToString();
        }
        else
        {
            lengthPrice.SetActive(false);
            widthPrice.GetComponent<TextMeshProUGUI>().text = prices[currentLevel].ToString();
        }

    }

    public void ExtandMap()
    {
        if (!VitalisManager.instance.CanBuy(prices[currentLevel])) return;

        VitalisManager.instance.RemoveVitalis(prices[currentLevel]);

        if(currentLevel < maxLevel)
        {
            if (!cameraController) cameraController = Camera.main.GetComponent<CameraController>();
            TerrainGenerator.instance.AddTerrain(side);
            cameraController.SetMaxDistances();
            currentLevel++;
        }

        if (currentLevel >= maxLevel)
        {
            this.gameObject.SetActive(false);
        }


        SetSide(side);
    }
}
