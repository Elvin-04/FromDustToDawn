using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    public static OxygenManager instance;

    public int maximumOxygen {  get; private set; }
    public int currentOxygen { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UIManager.instance.UpdateUIOxygen(currentOxygen, maximumOxygen);
    }

    public void AddMaximumOxygen(int amount)
    {
        maximumOxygen += amount;
        currentOxygen += amount;
        UIManager.instance.UpdateUIOxygen(currentOxygen, maximumOxygen);
    }

    public void RemoveCurrentOxygen(int amount)
    {
        currentOxygen -= amount;
        UIManager.instance.UpdateUIOxygen(currentOxygen, maximumOxygen);
    }

    public bool CanPlace(int price)
    {
        if (currentOxygen - price < 0)
            return false;

        return true;
    }
}
