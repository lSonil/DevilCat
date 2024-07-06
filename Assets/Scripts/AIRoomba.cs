using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIRoomba : MonoBehaviour
{
    public AIDestinationSetter aids;
    public List<Transform> route;
    public int speed = 200;
    Room room;
    Rigidbody2D rb;
    Vector2 randomforce;
    bool turnAround = true;
    public static AIRoomba instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aids = GetComponent<AIDestinationSetter>();
    }
    public void CheckProximity()
    {
        if (room == AICat.instance.IsInRoom() && room != null)
        {
            route = null;
            aids.target = AICat.instance.transform;
        }
        else
        {
            if(route == null)
            aids.target = null;
        }
    }
    private void Update()
    {
        Move();
        CheckProximity();
    }

    public void Move()
    {
        if (aids.target != null)
        {
            if (Vector3.Distance(transform.position, aids.target.transform.position) <= 0.5f)
            {
                //transform.position = aids.target.transform.position;
                //rb.velocity = new Vector2(0, 0);
                Door door = aids.target.GetComponent<Door>();
                AICat cat = aids.target.GetComponent<AICat>();

                if (door != null)
                {
                    transform.position = door.con.port.position;
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
                if (cat != null)
                {
                    AICat.instance.StopAction(gameObject);
                }
            }
        }
        else
        {
            if (turnAround)
            {
                int x = Random.Range(-1, 2);
                int y = Random.Range(-1, 2);
                if (x == 0 && y == 0)
                {
                    int z = Random.Range(0, 2);
                    if (z == 1)
                    {
                        x = Random.Range(-1, 1);
                        x = x == 0 ? +1 : x;
                    }
                    else
                    {
                        y = Random.Range(-1, 1);
                        y = y == 0 ? +1 : y;
                    }
                }
                randomforce = new Vector2(x, y) * speed * Time.deltaTime;
                turnAround = false;
            }
            rb.AddForce(randomforce);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Room")
        {
            room = collision.GetComponent<Room>();
            return;
        }

        if (aids.target == null)
        {
            if (collision.tag == "Door")
            {
                Door door = collision.GetComponent<Door>();
                transform.position = door.con.port.position;
            }
            else
            {
                randomforce = -randomforce;
                StartCoroutine(Wait());
            }
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        turnAround = true;
    }
    public void SetDestination(GameObject dest)
    {
        Search(room, new List<string>(), new List<List<Transform>>(), new List<Transform>(),0, false, dest);
        StartCoroutine(Begin());
    }
    public IEnumerator Begin()
    {
        while (route==null)
        {
            yield return null;
        }
        aids.target = route[0];
    }
    public void Search(Room actualRoom, List<string> bannedRoom, List<List<Transform>> allRoutes, List<Transform> posibleRoute, int counter, bool foundRoute, GameObject destination)
    {
        foreach (Door pos in actualRoom.doors)
        {
            Room parent = pos.transform.parent.parent.GetComponent<Room>();
            //Debug.Log("FROM : " + parent.name);
            if (parent.name == destination.name)
            {
                if (!allRoutes.Contains(posibleRoute))
                {
                    allRoutes.Add(posibleRoute);
                    foundRoute = true;
                }
            }
            else
            {
                List<string> tempB = new List<string>(bannedRoom);
                if (!bannedRoom.Contains(parent.name))
                {
                    tempB.Add(parent.name);
                }

                Room goesTo = pos.con.transform.parent.parent.GetComponent<Room>();

                if (!tempB.Contains(goesTo.name))
                {
                    //Debug.Log(parent.name + " " + pos.name);
                    //Ban(tempB);
                    //Debug.Log(ViewSystem.instance.cameras.Count);
                    if (counter < ViewSystem.instance.cameras.Count - 1)
                    {
                        List<Transform> tempP = new List<Transform>(posibleRoute);
                        tempP.Add(pos.transform);
                        //Show(tempP, "       ROUTE : ");
                        Search(goesTo, tempB, allRoutes, tempP, ++counter, false, destination);
                    }
                    else
                        return;
                }
                //else
                //{
                //    Debug.Log("         CANT GO BACK TO : " + goesFrom + " USING " + pos + " : THAT ROUTE IS BANNED AT POSSITION " + tempB.IndexOf(goesTo.name) + " : " + tempB[tempB.IndexOf(goesTo.name) - 1]);
                //
                //}
                //Ban(tempB);
                //Debug.Log("");

            }
        }
        if (foundRoute)
            Choose(allRoutes);
    }
    public void Show(List<Transform> rout,string str)
    {
        string route = str;
        foreach (Transform item in rout)
        {
            route += item.name + " ";
        }
        Debug.Log(route);
    }
    public void Ban(List<string> rout)
    {
        string route = "    CANT GO IN : ";
        foreach (string item in rout)
        {
            route += item + " ";
        }
        Debug.Log(route);
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
        aids.target = route[0];
    }
}