using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingPanelContent : MonoBehaviour
{
    public Placable placable;
    public Image representationImage;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI OCreated;
    public TextMeshProUGUI Price;


    public void SetDatasOnUI()
    {
        //representationImage.sprite = placable.visualRepresentation;
        this.Name.text = placable.name;
        OCreated.text = "O² created : " + placable.OCreated.ToString();
        Price.text = placable.price.ToString();
    }

    public void OnClick()
    {
        PlacableManager.instance.OnSelectPlacable(placable);
    }
}
