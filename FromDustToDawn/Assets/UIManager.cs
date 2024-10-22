using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Vitalis")]
    public TextMeshProUGUI vitalisAmount;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateUIVitalisAmount();
    }

    public void UpdateUIVitalisAmount()
    {
        vitalisAmount.text = VitalisManager.instance.GetVitalis().ToString();
    }
}
