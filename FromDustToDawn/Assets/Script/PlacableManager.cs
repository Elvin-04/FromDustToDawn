using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacableManager : MonoBehaviour
{

    public static PlacableManager instance;

    public List<Placable> PlantsList;
    public List<Placable> AliveList;

    private bool previewEnable = false;
    private GameObject preview;
    private Placable currentPlacable;

    public LayerMask previewMask;
    public Material redMaterial;

    public TerrainGenerator terrainGen;
    string currentPanelName;
    private void Awake()
    {
        instance = this;
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if(context.performed && previewEnable && preview.activeSelf)
        {
            if(!VitalisManager.instance.CanBuy(currentPlacable.price)) Destroy(preview);
            else VitalisManager.instance.RemoveVitalis(currentPlacable.price);
            previewEnable = false;

            if(currentPlacable.isVegetation)
            {
                OxygenManager.instance.AddMaximumOxygen(currentPlacable.OCreated);
                VitalisAutoCreator vtCreator = preview.AddComponent<VitalisAutoCreator>();
                vtCreator.StartProducing(currentPlacable.vitalisCreated, currentPlacable.delay);
            }
            else if(currentPlacable.isAlive)
            {
                OxygenManager.instance.RemoveCurrentOxygen(currentPlacable.OUsed);

            }


            UIManager.instance.OpenBuildingPanel(currentPanelName);
            
        }
    }

    public List<Placable> GetPlacabelList(PlacableType type)
    {
        if(type == PlacableType.PLANTS) return PlantsList;

        return AliveList;
    }

    private void Update()
    {
        if(!previewEnable) return;

        if (currentPlacable.isVegetation)
        {
            VegetationPreview();
        }
        else if(currentPlacable.isAlive)
        {
             AlivePreview();
        }
    }

    private void VegetationPreview()
    {
        RaycastHit hit = PlayerInputController.instance.MousePointRaycast(previewMask);
        Vector3 position;
        Vector3 hitNormal = hit.normal;

        bool validHit = hit.collider != null && hit.transform.tag != "Vitalis";
        bool waterCondition;

        if (currentPlacable.onWater && hit.point.y <= terrainGen.genOptions.waterLevel
            || currentPlacable.underwater && hit.point.y <= terrainGen.genOptions.waterLevel
            || !currentPlacable.underwater && !currentPlacable.onWater && hit.point.y >= terrainGen.genOptions.waterLevel)
            waterCondition = true;
        else 
            waterCondition = false;


        preview.SetActive(validHit && waterCondition);

        if (preview.activeSelf)
        {
            position = currentPlacable.onWater
                ? new Vector3(hit.point.x, terrainGen.genOptions.waterLevel, hit.point.z)
                : hit.point;

            Quaternion rotation = currentPlacable.onWater
                ? Quaternion.identity
                : Quaternion.FromToRotation(Vector3.up, hitNormal);

            preview.transform.position = position;
            preview.transform.rotation = rotation;
        }
    }

    private void AlivePreview()
    {
        RaycastHit hit = PlayerInputController.instance.MousePointRaycast(previewMask);
        Vector3 position;

        bool validHit = hit.collider != null && hit.transform.tag != "Vitalis";
        preview.SetActive(hit.point.y < terrainGen.genOptions.waterLevel && validHit);

        if (preview.activeSelf)
        {
            position.x = hit.point.x;
            position.y = ((terrainGen.genOptions.waterLevel - hit.point.y) / 2) + hit.point.y;
            position.z = hit.point.z;

            preview.transform.position = position;
        }
    }

    public void OnSelectPlacable(Placable p)
    {
        currentPlacable = p;
        preview = Instantiate(currentPlacable.objectPrefab);
        previewEnable = true;
        currentPanelName = UIManager.instance.titleBuildingPanel.text;
        UIManager.instance.CloseBuildingPanel();
    }
}
