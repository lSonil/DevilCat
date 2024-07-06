using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public List<Interaction> intList;
    bool available;
    int counter=0;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            intList.Add(child.GetComponent<Interaction>());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (available)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                intList[counter].Interact();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                counter++;
                if(counter>=intList.Count)
                {
                    counter = 0;
                }
            }
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
    // Start is called before the first frame update
}
