using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour
{
    public bool complete;
    public int time = 2;
    public IEnumerator Action()
    {
        AICat.instance.Idle(complete);
        if (complete)
        {
            yield return new WaitForSeconds(0);
        }
        else
        {
            yield return new WaitForSeconds(time);
            complete = true;
        }
        foreach (Ritual option in AICat.instance.options)
        {
            option.Check();
        }

        StartCoroutine(AICat.instance.Search());
        yield return new WaitForSeconds(0.1f);

        if (gameObject.tag == "Distraction")
        {
            AICat.instance.StartCount();
            yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
        }
    }
}
