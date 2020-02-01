using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{    
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private GameObject[] nodes;
    [SerializeField] private float[] waitTime;

    [SerializeField] private GameObject[] players;
    [SerializeField] private float distanceToTarget;

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
        Vector3 target = ChooseClosest();

        if (Mathf.Pow(Mathf.Pow(target.x, 2) + Mathf.Pow(target.y, 2), -2) - Mathf.Pow(Mathf.Pow(this.transform.position.x, 2) + Mathf.Pow(this.transform.position.y, 2), -2) < distanceToTarget)
            chosenMovement = 1;
        else
            chosenMovement = 0;
    }

    private void Chase()
    {

    }

    private Vector3 ChooseClosest()
    {
        Vector3 closestPlayer = new Vector3(100, 100, 100);

        foreach (var a in players)
        {
            if (Mathf.Pow(Mathf.Pow(closestPlayer.x, 2) + Mathf.Pow(closestPlayer.y, 2), -2) - Mathf.Pow(Mathf.Pow(a.transform.position.x, 2) + Mathf.Pow(a.transform.position.y, 2), -2) > 0)
            {
                closestPlayer = a.transform.position;
            }
        }
        return closestPlayer;
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
