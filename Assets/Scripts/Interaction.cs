using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    GameObject doorO;
    GameObject doorC;

    private void Awake()
    {
        doorO = transform.GetChild(0).gameObject;
        doorO.SetActive(true);
        doorC = transform.GetChild(1).gameObject;
        doorC.SetActive(false);
    }
    public void Interact()
    {
        doorC.SetActive(!doorC.activeInHierarchy);
        doorO.SetActive(!doorO.activeInHierarchy);
    }
}
