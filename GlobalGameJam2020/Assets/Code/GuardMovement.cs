using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(NavMeshAgent))]
public class GuardMovement : Unit
{    
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject waypointsParent;

    [SerializeField] private float shotDelay;
    [SerializeField] private float waypointThreshold = 0.6f;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float shootDistance;
    [SerializeField] private float FOV = 90;

    private List<Waypoint> waypoints = new List<Waypoint>();
    public Unit target;
    private int currentWaypoint = 0;
    private float patrolWaitTime = 0;
    private float shotWaitTime = 0;
    private float radius = 0.5f;
    private enum GuardState {
        Patrol,
        Chase,
        Shoot,
        Return
    }

    private GuardState guardState = GuardState.Patrol;

    protected void Start() {
        base.Start();

        Vector3 size = internalGameObject.GetComponent<Collider2D>().bounds.size;
        radius = Math.Max(size.x, size.y);

        foreach(Transform tr in waypointsParent.transform) {
            waypoints.Add(tr.gameObject.GetComponent<Waypoint>());
        }
        patrolWaitTime = waypoints[0].waitTime;
    }

    private float DistanceFromMe(GameObject a) {
        return (a.transform.position - this.transform.position).magnitude;
    }

    private float DistanceFromMe(MonoBehaviour a) {
        return (a.transform.position - this.transform.position).magnitude;
    }

    private void PickTarget()
    {
        target = null;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
        Unit unit = null;
        List<Unit> units = new List<Unit>();
        List<float> distances = new List<float>();

        foreach (GameObject obj in gameObjects) {
            unit = obj.GetComponent<Unit>();
            if(unit == this) continue;
            units.Add(unit);
            distances.Add(DistanceFromMe(unit));
        }

        Unit fastestWithinShot = null;
        Unit closestSlow = null;
        Unit closestFast = null;
        float maxSpeed = 0f;
        float minDistanceSlow = 0f;
        float minDistanceFast = 0f;
        
        for(int i = 0; i < units.Count; i++)
        {
            unit = units[i];
            if(unit.GetState() == State.Dead) continue;
            float distance = distances[i];
            float angle = Vector3.Angle(transform.forward, unit.transform.position - transform.position);
            if(Math.Abs(angle) > FOV) continue;
            RaycastHit2D lineOfSight = Physics2D.Raycast(
                transform.position,
                unit.transform.position - transform.position,
                sightRange + unit.detectionRange,
                ~LayerMask.GetMask("EnemyInternal")
            );
            if(lineOfSight.collider == null) continue;
            if(lineOfSight.collider.gameObject != unit.GetInternalGameObject()) continue;
            if(distance < shootDistance) {
                if(fastestWithinShot == null || unit.speed > maxSpeed) {
                    fastestWithinShot = unit;
                    maxSpeed = unit.speed;
                }
            }
            else if(speed <= unit.speed) {
                if(closestSlow == null || distance < minDistanceSlow) {
                    closestSlow = unit;
                    minDistanceSlow = distance;
                }
            }
            else if(closestFast == null || distance < minDistanceFast) {
                    closestFast = unit;
                    minDistanceFast = distance;
                }
        }

        if(fastestWithinShot != null) target = fastestWithinShot;
        else if(closestSlow != null) target = closestSlow;
        else target = closestFast;
    }

    private void Detect()
    {
        PickTarget();
        if (target != null && (DistanceFromMe(target) < sightRange + target.detectionRange))
            guardState = GuardState.Chase;
    }

    private void Patrol()
    {
        //Debug.Log($"Patrol {currentWaypoint} {DistanceFromMe(waypoints[currentWaypoint])} {agent.destination}");
        if (DistanceFromMe(waypoints[currentWaypoint]) >= waypointThreshold)
        {
            Pathfind(waypoints[currentWaypoint].transform.position);
        }
        else
        {
            if(patrolWaitTime <= 0) {
                currentWaypoint++;
                if (currentWaypoint >= waypoints.Count)
                    currentWaypoint = 0;
                patrolWaitTime = waypoints[currentWaypoint].waitTime;
            }
            else patrolWaitTime -= Time.deltaTime;
        }
    }

    private void Chase()
    {
        if (DistanceFromMe(target) > shootDistance * 0.8)
            Pathfind(target.transform.position);
        else {
            shotWaitTime = shotDelay;
            guardState = GuardState.Shoot;
        }
    }

    private void Shoot() {
        shotWaitTime -= Time.deltaTime;
        if(shotWaitTime <= 0) {
            if (DistanceFromMe(target) < shootDistance) 
                Instantiate(
                    bullet,
                    transform.position + (target.transform.position - transform.position).normalized * radius,
                    transform.rotation,
                    transform
                );
            guardState = GuardState.Return;
        }
    }

    private void Return() {
        int nearestWaypoint = 0;
        float minDistance = DistanceFromMe(waypoints[nearestWaypoint]);
        for(int i = 1; i < waypoints.Count; i++) {
            float distance = DistanceFromMe(waypoints[i]);
            if(distance < minDistance) {
                nearestWaypoint = i;
                minDistance = distance;
            }
        }
        currentWaypoint = nearestWaypoint;

        guardState = GuardState.Patrol;
    }

    private void Update()
    {
        switch(guardState) {
            case GuardState.Patrol:
                Patrol();
                Detect();
                break;
            case GuardState.Chase:
                Chase();
                break;
            case GuardState.Shoot:
                Shoot();
                break;
            case GuardState.Return:
                Return();
                break;
        }
    }
}
