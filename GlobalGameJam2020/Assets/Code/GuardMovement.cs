using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{    
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject[] nodes;
    [SerializeField] private float[] waitTime;

    [SerializeField] private GameObject[] players;
    [SerializeField] private float distanceToTarget;
    public Vector3 target;

    private int counter = 0;
    System.DateTime now;

    private string[] movementTypes = { "patrol", "chase" };
    private int chosenMovement = 0;

    private void Movement()
    {
        
        if (Mathf.Abs(nodes[counter].transform.position.x - this.transform.position.x) >= distance && Mathf.Abs(nodes[counter].transform.position.y - this.transform.position.y) >= distance)
        {
            this.transform.position += speed * (nodes[counter].transform.position - this.transform.position).normalized * Time.deltaTime;
            now = System.DateTime.Now.AddSeconds(waitTime[counter]);
        }
        else
        {
            if (System.DateTime.Now >= now)
            {
                if (counter >= nodes.Length - 1)
                    counter = 0;
                else
                    counter++;
            }
        }
    }

    private void Detect()
    {
        if ((target - this.transform.position).magnitude < distanceToTarget)
            chosenMovement = 1;
        else
            chosenMovement = 0;
    }

    private void Chase()
    {
        if ((target - this.transform.position).magnitude > 0.25 * distanceToTarget)
            this.transform.position += 2 * speed * (target - this.transform.position).normalized * Time.deltaTime;

        //if ((target - this.transform.position).magnitude < 0.05 * distanceToTarget)
         //   Instantiate(bullet, this.transform.position, this.transform.rotation, this.transform);   
    }

    private void ChooseClosest()
    {
        Vector3 closestPlayer = new Vector3(100, 100, 100);
        foreach (var a in players)
            if ((closestPlayer - a.transform.position).magnitude > 0)
                closestPlayer = a.transform.position;
        target = closestPlayer;
    }

    private void Update()
    {
        Detect();

        if (movementTypes[chosenMovement] == "patrol")
            Movement();
        else if (movementTypes[chosenMovement] == "chase")
            Chase();
    }
}
