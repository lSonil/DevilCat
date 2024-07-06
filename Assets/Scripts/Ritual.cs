using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ritual : MonoBehaviour
{
    [SerializeField] Spot[] condiiton;
    bool ready = false;
    public void Check()
    {
        if(!ready)
        {
            ready = true;
            foreach (Spot spot in condiiton)
            {
                if (!spot.complete)
                    ready = false;
            }
            if (ready)
                Summon();
        }
    }

    public void Summon()
    {
        Debug.Log("aaa");
    }
}
