using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Placable", order = 1)]
public class Placable : ScriptableObject
{
    [Header("Common")]
    public int price;
    public Texture visualRepresentation;
    public GameObject objectPrefab;

    
    [Header("Vegtation")]
    public bool isVegetation;
    public int OxygenCreated;

    public bool underwater;
    public bool onWater;


    [Header("Alive")]
    public bool isAlive;
    public int OxygenUsed;
    public bool isUnderwaterAnimal;
    public int vitalisCreated = 15;
    public float delay = 15f;
}