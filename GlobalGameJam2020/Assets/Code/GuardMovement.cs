using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{    
    [SerializeField] private float speed;
    [SerializeField] private float distance;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float[] waitTime;

    [SerializeField] private float shotDelay;

    [SerializeField] private GameObject[] units;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float shootDistance;
    public GameObject target;
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

    private void MoveTo(GameObject target) {
        this.transform.position += speed * Time.deltaTime * (target.transform.position - this.transform.position).normalized;
    }

    private float DistanceFromMe(GameObject a) {
        return (a.transform.position - this.gameObject.transform.position).magnitude;
    }

    private GameObject ChooseClosest()
    {
        GameObject closest = null;
        float closestDistance = 0f;
        foreach (GameObject a in units)
        {
            if(a == this.gameObject) continue;
            float distance = DistanceFromMe(a);
            if(closest == null || closestDistance > distance) {
                closest = a;
                closestDistance = distance;
            }
        }
            
        return closest;
    }

    private void Detect()
    {
        target = ChooseClosest();
        if (DistanceFromMe(target) < chaseDistance)
            state = State.Chase;
    }

    private void Patrol()
    {
        if (DistanceFromMe(waypoints[currentWaypoint]) >= distance)
        {
            MoveTo(waypoints[currentWaypoint]);
            patrolWaitTime = waitTime[currentWaypoint];
        }
        else
        {
            if(patrolWaitTime <= 0) {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Length)
                    currentWaypoint = 0;
            }
            else patrolWaitTime -= Time.deltaTime;
        }
    }

    private void Chase()
    {
        if (DistanceFromMe(target) > shootDistance * 0.8)
            MoveTo(target);
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
