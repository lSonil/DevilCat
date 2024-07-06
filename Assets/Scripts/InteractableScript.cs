using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableScript : MonoBehaviour
{
    SpriteRenderer sprite;
    public GameObject distraction;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    public void Outline(bool state)
    {
        //if (state)
        //   sprite.material = ViewSystem.instance.noOutline;
        //else
        // {
        //    sprite.material = (MaterialName() == ViewSystem.instance.outline.name) ?
        //                        ViewSystem.instance.noOutline :
        //                        ViewSystem.instance.outline;
        //}
    }
    public void OnMouseDown()
    {
        if (!CameraMovement.instance.laserOut &&
            KeyboardSystem.online &&
            MaterialName() == ViewSystem.instance.outline.name)
            Debug.Log("interact");
    }
    private string MaterialName()
    {
        string material = sprite.material.name.Replace(" (Instance)", "");
        return material;
    }
}