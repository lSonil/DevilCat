using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D body;

    float horizontal;
    float vertical;

    public static Movement instance;
    public GameObject itemProx = null;
    public GameObject itemHold = null;
    public float runSpeed = 20.0f;
    Room room;
    public Room IsInRoom()
    {
        return room;
    }
    public void SetRoom(Room r)
    {
        room = r;
    }
    private void Awake()
    {
        instance = this;
        //Cursor.visible = false;
    }
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!KeyboardSystem.online)
        {
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
    }

    private void FixedUpdate()
    {
        if (!KeyboardSystem.online)
        {
            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
        }
        else
        {
            body.velocity = new Vector2(0f, 0f);
        }
    }
}
