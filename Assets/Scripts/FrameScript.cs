using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameScript : MonoBehaviour
{
    public Text time;
    public Text scene;
    float actualTime = 0;
    private void Awake()
    {
        actualTime = Time.time;
    }
    private void Update()
    {
        float t = 100*(Time.time - actualTime);
        string h = ((int)t / 120).ToString();
        if (((int)t / 120 )< 10)
            h = 0 + h;
        string m = ((int)t / 60).ToString();
        if (((int)t / 60 )< 10)
            m = 0 + m;
        string s = (t % 60).ToString("f0");
        if ((t % 60) < 10)
            s = 0 + s;
        time.text = h + ":" + m + ":" + s;
    }

    public void SetScene()
    {
        scene.text = ViewSystem.instance.cameras.IndexOf(Room.mainRoom) + "/" + (ViewSystem.instance.cameras.Count-1);
    }
}