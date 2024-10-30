using System.Collections;
using UnityEngine;

public class VitalisAutoCreator : MonoBehaviour
{
    private int amount;
    private float delay;

    public void StartProducing(int amount, float time)
    {
        this.amount = amount;
        this.delay = time;

        StartCoroutine(ProduceVitalis());
    }

    IEnumerator ProduceVitalis()
    {
        yield return new WaitForSeconds(delay);
        VitalisManager.instance.AddVitalis(amount);
        StartCoroutine(ProduceVitalis());
    }
}
