using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 1;

    private Vector2 goal = new Vector2(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Change direction
        if (Input.GetMouseButtonDown(0)) {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null) {
                goal = hit.point;
            }
        }

        // Movement
        float distance = Mathf.Min((goal - (Vector2)this.transform.position).magnitude, speed * Time.deltaTime);
        transform.Translate((goal - (Vector2)this.transform.position).normalized * distance);
    }
}

