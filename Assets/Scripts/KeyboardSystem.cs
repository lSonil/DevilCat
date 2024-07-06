using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyboardSystem : MonoBehaviour
{
    [SerializeField] List<Room> cams;
    [SerializeField] FadeScript fade;
    [SerializeField] FrameScript frame;
    [SerializeField] CameraMovement cameraCM;
    [SerializeField] Transform player;
    [SerializeField] GameObject uiHouse;
    [SerializeField] GameObject uiInfo;
    [SerializeField] TextMeshProUGUI location;
    float counter = 0;
    bool state = false;
    bool shapes = false;
    bool changing = false;
    public static bool online = false;
    // Start is called before the first frame update
    private void Start()
    {
        frame.gameObject.SetActive(false);
        cams = ViewSystem.instance.cameras;
        foreach (Room item in cams)
        {
            counter = item.name == "LivingroomZone" ? cams.IndexOf(item) : counter;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (state)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !changing)
            {
                StartCoroutine(cameraCM.DeOffset());
                frame.gameObject.SetActive(!frame.gameObject.activeInHierarchy);
                online = !online;
                ShowShapes();
                if (!online)
                {
                    Change(ViewSystem.yourCamera);
                }
                else
                {
                    Change(counter);
                    DisplayButtons();
                }
            }
            if (online)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    ShowShapes(true);
                }
                if (!shapes)
                {
                    Actions();
                }
            }
        }
    }
    public void Actions()
    {
        // ACTION BUTTONS

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ViewSystem.instance.Lights();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            CameraMovement.instance.SetLaser(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            CameraMovement.instance.SetLaser(false);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ViewSystem.instance.SetDestination();
        }
    }
    public void DisplayButtons()
    {
        location.text = Room.mainRoom.name.Replace("Zone (Room)", "");
    }
    public void ShowShapes(bool reverse = false)
    {
        bool setTo = reverse ? !uiHouse.activeInHierarchy : false;
        uiHouse.gameObject.SetActive(setTo);
        shapes = setTo;
        uiInfo.gameObject.SetActive(!setTo);
    }
    public void Online()
    {
        float value=0;
        for (int i = 0; i < cams.Count; i++)
        {
            if (cams[i].name.Replace("Zone", "") == EventSystem.current.currentSelectedGameObject.name)
            {
                value = i;
            }
        }

        float spare = counter;
        if (!changing)
        {
            counter = Mathf.Clamp(value, 0, cams.Count - 1);
        }
        if (spare != counter)
        {
            StartCoroutine(cameraCM.DeOffset());
            changing = true;
            StartCoroutine(BlackOut(counter));
        }
        ShowShapes();
    }
    IEnumerator BlackOut(float val)
    {
        StartCoroutine(fade.Fade());
        yield return new WaitForSeconds(0.15f);
        Change(val);
        yield return new WaitForSeconds(0.15f);
        DisplayButtons();
        StartCoroutine(fade.DeFade());
        yield return new WaitForSeconds(0.15f);

        changing = false;
    }

    public void Change(float val)
    {
        if (val == ViewSystem.yourCamera)
        {
            cams[ViewSystem.yourCamera].SetOnline(player);
            return;
        }
        for (int i = 0; i < cams.Count; i++)
        {
            if (i == val)
            {
                cams[i].SetOnline(null);
            }
        }
        frame.SetScene();
    }

    public void Set()
    {
        state = !state;
    }
}