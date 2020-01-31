using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates
{
    private double x;
    private double y;

    public Coordinates() { }
    public Coordinates(double x, double y)
    {
        this.x = x;
        this.y = y;
    }
}

public class GuardMovement : MonoBehaviour
{
    [SerializeField] private GameObject[] nodes;
    private Coordinates[] coordinates;

    private void Awake()
    {
        coordinates = new Coordinates[nodes.Length]();
        for (int i = 0; i < nodes.Length; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
