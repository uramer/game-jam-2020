using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private GameObject[] nodes;
    private int counter = 0;

    private void Movement()
    {
        if (Mathf.Abs(nodes[counter].transform.position.x - this.transform.position.x) >= distance && Mathf.Abs(nodes[counter].transform.position.y - this.transform.position.y) >= distance)
        {
            this.transform.position += speed * (nodes[counter].transform.position - this.transform.position).normalized * Time.deltaTime;
        }
        else
        {
            if (counter >= nodes.Length - 1)
                counter = 0;
            else
                counter++;
        }
    }

    void Update()
    {
        Movement();
    }
}
