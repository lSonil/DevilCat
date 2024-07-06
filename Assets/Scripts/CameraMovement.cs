using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    float moveX = 0f;
    float moveY = 0f;
    public float scrollingSensitivity = 15f;
    public float movingSensitivity = 0.5f;
    public CinemachineVirtualCamera vcam;
    public Transform roomOptions;
    public GameObject laserPoint;
    [SerializeField] List<Image> rooms;
    public GameObject distractionPrefab;
    public GameObject distraction;
    public bool laserOut = false;
    public static CameraMovement instance;
    int floorCounter = 1;
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < roomOptions.childCount; i++)
        {
            foreach (Transform grandchild in roomOptions.GetChild(i))
            {
                rooms.Add(grandchild.GetComponent<Image>());
            }
        }
    }
    void Update()
    {
        if (KeyboardSystem.online)
        {
            Moving();
            Scrolling();
            if (laserOut)
                Laser();
        }
        else
        {
            var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
            transposer.m_TrackedObjectOffset = new Vector3(0, 0, 0);
            vcam.m_Lens.OrthographicSize = vcam.GetComponent<CinemachineConfiner>().m_BoundingShape2D.GetComponentInParent<Room>().viewSize;
        }
    }
    public void Scrolling()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            vcam.m_Lens.OrthographicSize -= scrollingSensitivity;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            vcam.m_Lens.OrthographicSize += scrollingSensitivity;
        }
        vcam.m_Lens.OrthographicSize = Mathf.Clamp(vcam.m_Lens.OrthographicSize, 1f, vcam.GetComponent<CinemachineConfiner>().m_BoundingShape2D.GetComponentInParent<Room>().viewSize);
    }
    public void Moving()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            moveX += Input.GetAxis("Mouse X") * movingSensitivity;
            moveY += Input.GetAxis("Mouse Y") * movingSensitivity;
            moveX = Mathf.Clamp(moveX, -3, 3);
            moveY = Mathf.Clamp(moveY, -3, 3);
            SetOffset();
        }
    }
    public void Laser()
    {
        laserPoint.transform.position = Input.mousePosition;

        if (laserPoint.GetComponent<Image>().enabled)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                distraction = Instantiate(distractionPrefab);
                laserPoint.GetComponent<Image>().color = new Color(255f, 0f, 0f, 1f);
                StartCoroutine(Distraction());
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && distraction != null)
            {
                if (AICat.instance.aids.target != distraction.transform)
                    Destroy(distraction);
                else
                    distraction = null;
                laserPoint.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
                return;
            }
        }
    }
    IEnumerator Distraction()
    {
        while (laserPoint.GetComponent<Image>().enabled)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                distraction.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 pos = distraction.transform.position;
                distraction.transform.position = new Vector3(pos.x, pos.y, 0);
            }
            yield return null;
        }
        if (distraction != null)
        {
            Destroy(distraction);
            laserPoint.GetComponent<Image>().color = new Color(255f, 255f, 255f, 1f);
            StartCoroutine(AICat.instance.Search());
            AICat.instance.StartCount();
        }
    }
    public void SetOffset()
    {
        var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_TrackedObjectOffset = new Vector3(moveX, moveY, 0);
    }
    public IEnumerator DeOffset()
    {
        yield return new WaitForSeconds(0.3f);
        var transposer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_TrackedObjectOffset = new Vector3(0, 0, 0);
        moveX = moveY = 0;
        vcam.m_Lens.OrthographicSize = vcam.GetComponent<CinemachineConfiner>().m_BoundingShape2D.GetComponentInParent<Room>().viewSize;
    }
    public void LaserVisibility(BaseEventData data = null)
    {
        laserPoint.SetActive(!laserPoint.activeInHierarchy);
    }
    public void Color()
    {
        foreach (Image child in rooms)
        {
            child.color = child.gameObject.name == EventSystem.current.currentSelectedGameObject.name ?
                            new Color32(255, 255, 0, 225) :
                            new Color32(255, 255, 255, 225);
        }
    }
    public void SetLaser(bool state)
    {
        if (state)
        {
            laserOut = false;
            laserPoint.GetComponent<Image>().enabled = laserOut;
        }
        else
        {
            laserOut = !laserOut;
            laserPoint.GetComponent<Image>().enabled = laserOut;
        }
    }

    public void Increase(int value)
    {
        floorCounter += value;
        if (floorCounter > roomOptions.childCount - 1)
            floorCounter = roomOptions.childCount-1;
        if (floorCounter < 0)
            floorCounter = 0;
        ChangeFloor();
    }
    public void ChangeFloor()
    {
        for (int i = 0; i < roomOptions.childCount; i++)
        {
            bool state = (i == floorCounter) ? true : false;
            roomOptions.GetChild(i).gameObject.SetActive(state);
        }
    }
}