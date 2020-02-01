using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : Unit
{    
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float[] waitTime;

    [SerializeField] private float shotDelay;
    [SerializeField] private float waypointThreshold = 0.1f;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float shootDistance;
    public Unit target;
    private int currentWaypoint = 0;
    private float patrolWaitTime = 0;
    private float shotWaitTime = 0;
    private enum State {
        Patrol,
        Chase,
        Shoot,
        Return
    }

    private State state = State.Patrol;

    private void Start() {
        patrolWaitTime = waitTime[currentWaypoint];
    }

    private void MoveTo(GameObject target) {
        Pathfind(target.transform.position);
        //this.transform.position += speed * Time.deltaTime * (target.transform.position - this.transform.position).normalized;
    }

    private float DistanceFromMe(GameObject a) {
        return (a.transform.position - this.transform.position).magnitude;
    }

    private float DistanceFromMe(Unit a) {
        return (a.transform.position - this.transform.position).magnitude;
    }

    private Unit ChooseClosest()
    {
        Unit closest = null;
        float closestDistance = 0f;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject obj in gameObjects)
        {
            if(obj == this.gameObject) continue;
            float distance = DistanceFromMe(obj);
            Unit unit = obj.GetComponent<Unit>();
            if(closest == null || closestDistance > distance) {
                closest = unit;
                closestDistance = distance;
            }
        }
            
        return closest;
    }

    private void Detect()
    {
        target = ChooseClosest();
        if (DistanceFromMe(target.gameObject) < sightRange + target.detectionRange)
            state = State.Chase;
    }

    private void Patrol()
    {
        if (DistanceFromMe(waypoints[currentWaypoint]) >= waypointThreshold)
        {
            MoveTo(waypoints[currentWaypoint]);
        }
        else
        {
            if(patrolWaitTime <= 0) {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length)
                    currentWaypoint = 0;
                patrolWaitTime = waitTime[currentWaypoint];
            }
            else patrolWaitTime -= Time.deltaTime;
        }
    }

    private void Chase()
    {
        if (DistanceFromMe(target) > shootDistance * 0.8)
            MoveTo(target.gameObject);
        else {
            shotWaitTime = shotDelay;
            state = State.Shoot;
        }
    }

    private void Shoot() {
        shotWaitTime -= Time.deltaTime;
        if(shotWaitTime <= 0) {
            if (DistanceFromMe(target) < shootDistance) 
                Instantiate(
                    bullet,
                    this.transform.position + 0.1f * (target.transform.position - this.transform.position),
                    this.transform.rotation,
                    this.transform
                );

            state = State.Return;
        }
    }

    private void Return() {
        int nearestWaypoint = 0;
        float minDistance = DistanceFromMe(waypoints[nearestWaypoint]);
        for(int i = 1; i < waypoints.Length; i++) {
            float distance = DistanceFromMe(waypoints[i]);
            if(distance < minDistance) {
                nearestWaypoint = i;
                minDistance = distance;
            }
        }
        currentWaypoint = nearestWaypoint;

        state = State.Patrol;
    }

    private void Update()
    {
        Debug.Log(state);
        switch(state) {
            case State.Patrol:
                Patrol();
                Detect();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Shoot:
                Shoot();
                break;
            case State.Return:
                Return();
                break;
        }
    }
}
