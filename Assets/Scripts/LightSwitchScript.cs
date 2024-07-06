using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitchScript : MonoBehaviour
{
    bool available;
    [HideInInspector]
    public GameObject lightOverlay;

    private void Awake()
    {
        foreach (Transform child in transform.parent.parent.parent.transform)
        {
            lightOverlay = (child.name == "Light") ? child.gameObject : lightOverlay;
        }
    }
    private void Update()
    {
        if(available&&Input.GetKeyDown(KeyCode.Space))
        {
            lightOverlay.SetActive(!lightOverlay.activeInHierarchy);
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            available = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            available = false;
    }
}
