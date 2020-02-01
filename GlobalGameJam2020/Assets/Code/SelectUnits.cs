using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnits : MonoBehaviour
{

    private List<GameObject> selected;
    private Vector2 startPos, startMousePos;

    // Start is called before the first frame update
    void Start()
    {
        selected = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) {
            startMousePos = Input.mousePosition;
            startPos = Camera.main.ScreenToWorldPoint(new Vector3(startMousePos.x, startMousePos.y, -Camera.main.transform.position.z));
        }
        if(Input.GetMouseButtonUp(0)) {
            Vector2 pos = Input.mousePosition;
            Vector2 endPos = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, -Camera.main.transform.position.z));

            UpdateSelected(startPos, endPos);
        }
        if(Input.GetMouseButtonDown(1)) {
            RaycastHit rayCastHit = new RaycastHit();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out rayCastHit)) {
                foreach(GameObject obj in selected) {
                    Unit unit = obj.GetComponent<Unit>();
                    unit.Pathfind(rayCastHit.point);
                }
            }
        }
        //MoveSelected();
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
            if(minX <= pos.x && pos.x <= maxX && minY <= pos.y && pos.y <= maxY) {
                selected.Add(unit);
            }
        }

        // Check if we clicked on one unit
        if(selected.Count == 0) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null && hit.collider.gameObject.tag == "Player") {
                selected.Add(hit.collider.gameObject);
            }
            else {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                hit = Physics2D.Raycast(ray.origin, ray.direction);
                if (hit.collider != null && hit.collider.gameObject.tag == "Player") {
                    selected.Add(hit.collider.gameObject);
                }
            }
        }
    }

    void MoveSelected() {
        foreach(GameObject unit in selected) {
            unit.transform.Translate(new Vector2(1, 0) * Time.deltaTime);
        }
    }

    List<GameObject> GetSelected() {
        return selected;
    }
}
