using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Door : MonoBehaviour
{
    public Door con;
    GameObject frame = null;
    GameObject door = null;
    public Transform port = null;
    bool activ = false;
    public bool instantly;
    GameObject entity;

    private void Awake()
    {
        foreach (Transform item in transform)
        {
            port = item.name == "Port" ? item : port;
            frame = item.name == "Frame" ? item.gameObject : frame;
            door = item.name == "Door" ? item.gameObject : door;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || instantly)
        {
            if (activ)
            {
                entity.transform.position = con.port.transform.position;
                StartCoroutine(con.GetComponent<Door>().Boop());
                return;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            entity = collision.gameObject;
            activ = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            activ = false;
            entity = null;
        }
    }

    public IEnumerator Boop()
    {
        if (frame != null)
        {
            if (door != null)
            door.SetActive(false);
            frame.transform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.25f);
            yield return new WaitForSeconds(0.25f);
            frame.transform.DOScale(new Vector3(1f, 1f, 1f), 0.25f);
            yield return new WaitForSeconds(0.25f);
            if (door != null)
                door.SetActive(true);
        }
        yield return null;
    }
}