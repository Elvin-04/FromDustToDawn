using TMPro;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class UIManager : MonoBehaviour
{
    [Header("Vitalis")]
    public TextMeshProUGUI vitalisAmount;

    [Header("Oxygen")]
    public TextMeshProUGUI oxygenAmount;

    [Header("BuildingPanel")]
    public GameObject buildingPanel;
    public TextMeshProUGUI titleBuildingPanel;
    public Transform panelContent;
    public GameObject ContentPrefab;
    

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        buildingPanel.SetActive(false);
        UpdateUIVitalisAmount();
    }

    public void UpdateUIVitalisAmount()
    {
        vitalisAmount.text = VitalisManager.instance.GetVitalis().ToString();
    }

    public void CloseBuildingPanel()
    {
        DestroyAllchild(panelContent);
        buildingPanel.SetActive(false);
    }

    public void OpenBuildingPanel(string title)
    {
        CloseBuildingPanel();
        buildingPanel.SetActive(true);
        titleBuildingPanel.text = title;
        PlacableType type = title.Contains("Plants") ? PlacableType.PLANTS : PlacableType.ALIVE;
        List<Placable> list = PlacableManager.instance.GetPlacabelList(type);

        foreach (Placable placable in list)
        {
            CreateContentElement(placable);
        }
    }

    private void DestroyAllchild(Transform t)
    {
        foreach (Transform child in t)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void CreateContentElement(Placable p)
    {
        GameObject instance = Instantiate(ContentPrefab, panelContent.transform);
        BuildingPanelContent content = instance.GetComponent<BuildingPanelContent>();
        content.placable = p;
        content.SetDatasOnUI();
    }

    public void UpdateUIOxygen(int current, int max)
    {
        oxygenAmount.text = current + " / " + max;
    }
}

public enum PlacableType
{
    PLANTS,
    ALIVE
}
