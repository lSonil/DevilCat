using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class ViewSystem : MonoBehaviour
{
    [HideInInspector]
    public List<Room> cameras;
    public GameObject mainCam;
    public static int yourCamera;
    public Material noOutline;
    public Material outline;
    public static ViewSystem instance;
    public GameObject ui;
    private void Awake()
    {
        instance = this;

        foreach (Transform child in transform)
        {
            cameras.Add(child.GetChild(0).GetComponent<Room>());
            if (child.name == "Bedroom")
            {
                yourCamera = cameras.Count - 1;
            };
        }
        ui.SetActive(true);
    }
    public void SetActiv(Room cam, Transform follow)
    {
        foreach (Room obj in cameras)
        {
            if (cam == obj)
            {
                mainCam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = cam.GetComponent<PolygonCollider2D>();
                mainCam.GetComponent<CinemachineVirtualCamera>().m_Follow = follow;
            }
        }
    }
    public void Lights()
    {
        Room.mainRoom.lightOverlay.SetActive(!Room.mainRoom.lightOverlay.activeInHierarchy);
    }

    public void SetDestination()
    {
        AIRoomba.instance.SetDestination(Room.mainRoom.gameObject);
    }
}
