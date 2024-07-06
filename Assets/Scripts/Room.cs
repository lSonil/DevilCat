using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Room : MonoBehaviour
{
    [SerializeField] public int viewSize;
    public static Room mainRoom;
    [HideInInspector]
    public GameObject lightOverlay;
    public List<Door> doors;
    public List<Spot> spots;

    ViewSystem system;

    private void Awake()
    {
        system = GetComponentInParent<ViewSystem>();
        foreach (Transform child in transform)
        {
            lightOverlay = (child.name == "Light") ? child.gameObject : lightOverlay;
            foreach (Transform grandchild in child)
            {
                Door door = grandchild.GetComponent<Door>();
                Spot spot = grandchild.GetComponent<Spot>();
                if (door != null)
                {
                    doors.Add(door);
                }

                if (spot != null)
                {
                    spots.Add(spot);
                }
            }
        }
        // DO NOT ERASE
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(true);
        // FOR SOME REASON, THE OBJECT CANNOT BE CLICKED UNTIL THEY ARE MOVED IN THE SCENE, OR DISABLED


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SetOnline(collision.transform);
            Movement.instance.SetRoom(this);
            if(AICat.instance!=null)
            AICat.instance.CheckForHunt();
        }
    }

    public void SetOnline(Transform target = null)
    {
        if (target == null)
            target = transform;
        mainRoom = this;
        system.SetActiv(gameObject.GetComponent<Room>(), target);
    }

    public Transform RandomPos()
    {
        int i = Random.Range(0, 2);
        if (i == 0)
        {
            int j = Random.Range(0, doors.Count);
            return doors[j].transform;
        }
        else
        {
            int j = Random.Range(0, spots.Count);

            return spots[j].transform;
        }
    }
    public Transform ClosestDoor()
    {
        Transform destination = null;
        float distance = Vector3.Distance(doors[0].transform.position, AICat.instance.transform.position);
        foreach (Door door in doors)
        {
            float newDistance = Vector3.Distance(door.transform.position, AICat.instance.transform.position);
            if (distance < newDistance)
            {
                distance = newDistance;
                destination = door.transform;
            }
        }
        return destination;
    }
}