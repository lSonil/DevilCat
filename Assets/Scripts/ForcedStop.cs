using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcedStop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Distraction")
        {
            AICat.instance.StopAction(collision.gameObject);
        }
    }
}
