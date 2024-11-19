using BehaviorTree;
using System.Collections.Generic;
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

    private TerrainGenerator terrainGen;
    private string currentPanelName;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        terrainGen = TerrainGenerator.instance;
    }

    public void OnRightClic(InputAction.CallbackContext context)
    {
        if(context.performed && previewEnable)
        {
            Destroy(preview);
            previewEnable = false;
        }
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if(context.performed && previewEnable && preview.activeSelf)
        {
            bool canPlace;

            

            if (!VitalisManager.instance.CanBuy(currentPlacable.price)) Destroy(preview);
            else if (currentPlacable.isAlive && !OxygenManager.instance.CanPlace(currentPlacable.OxygenUsed)) Destroy(preview);
            else VitalisManager.instance.RemoveVitalis(currentPlacable.price);
            previewEnable = false;

            if(currentPlacable.isVegetation && VitalisManager.instance.CanBuy(currentPlacable.price))
            {
                OxygenManager.instance.AddMaximumOxygen(currentPlacable.OxygenCreated);
                
            }
            else if(currentPlacable.isAlive && OxygenManager.instance.CanPlace(currentPlacable.OxygenUsed) && VitalisManager.instance.CanBuy(currentPlacable.price))
            {
                OxygenManager.instance.RemoveCurrentOxygen(currentPlacable.OxygenUsed);

                VitalisAutoCreator vtCreator = preview.AddComponent<VitalisAutoCreator>();
                vtCreator.StartProducing(currentPlacable.vitalisCreated, currentPlacable.delay);

                if (preview.TryGetComponent<ChickenBT>(out ChickenBT bt))
                {
                    bt.isPlaced = true;
                    bt.agent.enabled = true;
                }

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
        
        bool validHit = hit.collider != null && hit.transform.tag != "Vitalis";
        bool waterCondition = CanPlaceUnderwater(hit);

        preview.SetActive(validHit && waterCondition);

        if(!preview.activeSelf) return;

        SetPreviewPosition(hit);
        SetPreviewRotation(hit);
    }

    private void SetPreviewPosition(RaycastHit hit)
    {
        Vector3 position;
        
        position = currentPlacable.onWater
                ? new Vector3(hit.point.x, terrainGen.genOptions.waterLevel, hit.point.z)
                : hit.point;

        preview.transform.position = position;
    }

    private void SetPreviewRotation(RaycastHit hit)
    {
        Vector3 hitNormal = hit.normal;

        Quaternion rotation = currentPlacable.onWater
            ? Quaternion.identity
            : Quaternion.FromToRotation(Vector3.up, hitNormal);

        
        preview.transform.rotation = rotation;
    }

    private bool CanPlaceUnderwater(RaycastHit hit)
    {
        if (currentPlacable.onWater && hit.point.y <= terrainGen.genOptions.waterLevel
            || currentPlacable.underwater && hit.point.y <= terrainGen.genOptions.waterLevel
            || !currentPlacable.underwater && !currentPlacable.onWater && hit.point.y >= terrainGen.genOptions.waterLevel)
            return true;
        else
            return false;
    }

    private void AlivePreview()
    {
        RaycastHit hit = PlayerInputController.instance.MousePointRaycast(previewMask);
        Vector3 position;

        bool validHit = hit.collider != null && hit.transform.tag != "Vitalis";
        if(currentPlacable.isUnderwaterAnimal)
            preview.SetActive(hit.point.y < terrainGen.genOptions.waterLevel && validHit);
        else
            preview.SetActive(hit.point.y > terrainGen.genOptions.waterLevel && validHit);

        if (preview.activeSelf)
        {
            if(currentPlacable.isUnderwaterAnimal)
            {
                position.x = hit.point.x;
                position.y = ((terrainGen.genOptions.waterLevel - hit.point.y) / 2) + hit.point.y;
                position.z = hit.point.z;
            }
            else
            {
                position = hit.point;
            }

            if (!currentPlacable.isUnderwaterAnimal)
                SetPreviewRotation(hit);

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
