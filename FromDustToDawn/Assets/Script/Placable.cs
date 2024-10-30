using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Placable", order = 1)]
public class Placable : ScriptableObject
{
    [Header("Common")]
    public int price;
    public Sprite visualRepresentation;
    public GameObject objectPrefab;

    
    [Header("Vegtation")]
    public bool isVegetation;
    public int OCreated;
    public int vitalisCreated = 15;
    public float delay = 15f;

    public bool underwater;
    public bool onWater;


    [Header("Alive")]
    public bool isAlive;
    public int OUsed;
    public Comportement comportement;
}


public enum Comportement
{
    FISH
}