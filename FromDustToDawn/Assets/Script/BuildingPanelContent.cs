using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BuildingPanelContent : MonoBehaviour
{
    public Placable placable;
    public RawImage representationImage;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI OCreated;
    public TextMeshProUGUI Price;


    public void SetDatasOnUI()
    {
        if(placable.visualRepresentation != null)
            representationImage.texture = placable.visualRepresentation;
        this.Name.text = placable.name;
        OCreated.text = "O² created : " + placable.OxygenCreated.ToString();
        Price.text = placable.price.ToString();
    }

    public void OnClick()
    {
        PlacableManager.instance.OnSelectPlacable(placable);
    }
}
