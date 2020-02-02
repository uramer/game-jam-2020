﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnits : MonoBehaviour
{

    private List<GameObject> selected;
    private Vector2 startPos, startMousePos;

    void Start()
    {
        selected = new List<GameObject>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            startMousePos = Input.mousePosition;
            startPos = Camera.main.ScreenToWorldPoint(new Vector3(startMousePos.x, startMousePos.y, -Camera.main.transform.position.z)); //FIX
        }
        if(Input.GetMouseButtonUp(0) && startMousePos.y >= 50) {
            Vector2 pos = Input.mousePosition;
            Vector2 endPos = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, -Camera.main.transform.position.z));

            UpdateSelected(startPos, endPos);
        }
        if(Input.GetMouseButtonDown(1)) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            ActivateableBehaviour activateable = null;
            if(Input.GetKey(KeyCode.LeftControl)) {
                RaycastHit2D rayHit2D = Physics2D.Raycast(
                    ray.origin,
                    ray.direction,
                    Mathf.Infinity,
                    LayerMask.GetMask("Activateable")
                );
                Collider2D collider = rayHit2D.collider;
                if(collider != null) {
                    activateable = collider.gameObject.GetComponent<ActivateableBehaviour>();
                }
            }
            RaycastHit rayCastHit = new RaycastHit();
            if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit)) {
                foreach(GameObject unitObject in selected) {
                    Unit unit = unitObject.GetComponent<Unit>();
                    unit.Pathfind(rayCastHit.point);
                    unit.Activate(activateable);
                }
            }
        }
    }

    void UpdateSelected(Vector2 start, Vector2 end) {
        selected = new List<GameObject>();
        if(start == null || end == null) {
            return;
        }

        float minX = Mathf.Min(start.x, end.x);
        float maxX = Mathf.Max(start.x, end.x);
        float minY = Mathf.Min(start.y, end.y);
        float maxY = Mathf.Max(start.y, end.y);

        GameObject[] units = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject unit in units) {
            Vector2 pos = unit.transform.position;
            if(unit.GetComponent<Unit>().GetState() != Unit.State.Dead && minX <= pos.x && pos.x <= maxX && minY <= pos.y && pos.y <= maxY) {
                selected.Add(unit);
            }
        }

        // Check if we clicked on one unit
        if(selected.Count == 0) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null && hit.collider.gameObject.tag == "Player" && hit.collider.gameObject.GetComponent<Unit>().GetState() != Unit.State.Dead) {
                selected.Add(hit.collider.gameObject);
            }
            else {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider != null && hit.collider.gameObject.tag == "Player" && hit.collider.gameObject.GetComponent<Unit>().GetState() != Unit.State.Dead) {
                    selected.Add(hit.collider.gameObject);
                }
            }
        }
    }

    public List<GameObject> GetSelected() {
        return selected;
    }

    public void ClickSelect(GameObject unit) {
        if(selected.Contains(unit)) {
            selected.Remove(unit);
        }
        else {
            selected.Add(unit);
        }
    }
}
