using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class AICat : MonoBehaviour
{
    public Ritual[] options;
    public int distractTime;
    Room room;
    public AIDestinationSetter aids;
    public bool distractable = true;
    bool hunting = false;
    public List<Transform> route;

    Animator anim;
    SpriteRenderer sprite;
    IEnumerator actionInProgress;
    public static AICat instance;
    Vector3 posB;
    private void Awake()
    {
        instance = this;
        foreach (Transform item in transform)
        {
            anim = item.name == "Body" ? item.GetComponent<Animator>() : anim;
            sprite = item.name == "Body" ? item.GetComponent<SpriteRenderer>() : sprite;
        }
    }
    private void Start()
    {
        aids = GetComponent<AIDestinationSetter>();
        posB = transform.position;
    }
    public void CheckForHunt()
    {
        hunting = Movement.instance.IsInRoom().name == "BedroomZone" || Movement.instance.IsInRoom().name == "BalconyZone"? hunting : true;
        if (hunting)
        {

            if (actionInProgress != null)
            {
                StopCoroutine(actionInProgress);
                ResetParameters();
            }
            Search(room, new List<Room>(), new List<List<Transform>>(), new List<Transform>(), 0, false, Movement.instance.IsInRoom().gameObject);
            StartCoroutine(Begin());
        }
    }
    public IEnumerator Begin()
    {
        while (route.Count == 0)
        {
            yield return null;
        }
        aids.target = route[0];
    }
    private void Update()
    {
        CheckForProximity();
        Move();
    }
    public void Move()
    {

        SetMovement();

        if (aids.target != null)
            if (Vector3.Distance(transform.position, aids.target.transform.position) <= 0.5f)
            {
                transform.position = aids.target.transform.position;
                Door door = aids.target.GetComponent<Door>();
                Spot spot = aids.target.GetComponent<Spot>();

                if (door != null)
                {
                    transform.position = door.con.port.position;
                    if (route.Count != 0)
                    {
                        route.RemoveAt(0);
                        if (route.Count != 0)
                            aids.target = route[0];
                        else
                        {
                            route = null;
                            aids.target = null;
                        }
                        return;
                    }
                    else
                        return;
                }
                else
                if (spot != null)
                {
                    spot = aids.target.GetComponent<Spot>();
                    actionInProgress = spot.Action();
                    StartCoroutine(actionInProgress);
                    aids.target = null;
                }
            }
    }
    public void Idle(bool state)
    {
        if (state)
        {
            anim.SetInteger("Ind", Random.Range(1, 6));
        }
        else
        {
            anim.SetBool("Int", true);
        }
        anim.SetBool("Mov", false);
    }
    public void SetMovement()
    {
        if (!anim.GetBool("Int") && posB != transform.position)
        {
            if (posB.x != transform.position.x)
                sprite.flipX = posB.x > transform.position.x ? true : false;
            posB = transform.position;
            anim.SetBool("Mov", true);
        }
    }

    public void ResetParameters()
    {
        anim.SetBool("Mov", false);
        anim.SetBool("Int", false);
        anim.SetBool("Kill", false);
        anim.SetBool("Stop", false);
        anim.SetInteger("Ind", 0);
    }
    public void CheckForProximity()
    {
        if (Vector3.Distance(Movement.instance.transform.position, transform.position) < 0.5)
        {
            Time.timeScale = 0;
            Debug.Log("GAME OVER");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Room")
        {
            room = collision.GetComponent<Room>();

            if (!hunting)
                StartCoroutine(Search());
        }
    }

    public void Search(Room actualRoom, List<Room> bannedRoom, List<List<Transform>> allRoutes, List<Transform> posibleRoute, int counter, bool foundRoute, GameObject destination)
    {
        foreach (Door pos in actualRoom.doors)
        {
            Room parent = pos.transform.parent.parent.GetComponent<Room>();
            if (parent.name == destination.name)
            {
                if (!allRoutes.Contains(posibleRoute))
                {
                    List<Transform> tempP = new List<Transform>(posibleRoute);
                    tempP.Add(Movement.instance.gameObject.transform);
                    allRoutes.Add(tempP);
                    foundRoute = true;
                }
            }
            else
            {

                List<Room> tempB = new List<Room>(bannedRoom);
                if (!bannedRoom.Contains(parent))
                {
                    tempB.Add(parent);
                }

                Room goesTo = pos.con.transform.parent.parent.GetComponent<Room>();

                if (!tempB.Contains(goesTo))
                {
                    Ban(tempB);
                    if (counter < ViewSystem.instance.cameras.Count - 1)
                    {
                        List<Transform> tempP = new List<Transform>(posibleRoute);
                        tempP.Add(pos.transform);
                        Search(goesTo, tempB, allRoutes, tempP, ++counter, false, destination);
                    }
                    else
                        return;
                }
            }
        }
        if (foundRoute)
            Choose(allRoutes);
    }
    public void Show(List<Transform> rout)
    {
        string route="";
        foreach (Transform item in rout)
        {
            route += item.name+" ";
        }
        Debug.Log(route);
    }
    public void Ban(List<Room> rout)
    {
        string route = "";
        foreach (Room item in rout)
        {
            route += item.name + " ";
        }
    }
    public void Choose(List<List<Transform>> allRoutes)
    {

        int shortestPath = allRoutes[0].Count;
        foreach (List<Transform> route in allRoutes)
        {
            if (route.Count < shortestPath)
                shortestPath = route.Count;
        }
        List<List<Transform>> shortList = new List<List<Transform>>();
        foreach (List<Transform> route in allRoutes)
        {
            if (route.Count == shortestPath)
                shortList.Add(route);
        }
        int i = Random.Range(0, shortList.Count);
        route = shortList[i];
    }

    public Room IsInRoom()
    {
        return room;
    }
    public IEnumerator Search(bool toDoor=false)
    {
        ResetParameters();
        actionInProgress = null;
        aids.target = toDoor? room.ClosestDoor():room.RandomPos();
        yield return null;
    }
    public void StopAction(GameObject distraction, bool param = false)
    {
        if (distractable)
        {
            Debug.Log("STOOOOP");
            ResetParameters();
            if (actionInProgress != null)
                StopCoroutine(actionInProgress);
            if (distraction.GetComponent<Spot>() != null)
                aids.target = distraction.transform;
            else
                StartCoroutine(Search(param));
            distractable = false;
        }
    }
    public void StartCount()
    {
        StartCoroutine(DistractDown());
    }
    public IEnumerator DistractDown()
    {
        yield return new WaitForSeconds(distractTime);
        distractable = true;
    }

}