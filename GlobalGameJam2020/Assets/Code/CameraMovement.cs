using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private int maxHeight;
    private int maxWidth;
    [SerializeField] private float speed = 0.1f;

    private void Start() {
        Camera camera = GetComponent<Camera>();
        maxHeight = camera.pixelHeight;
        maxWidth = camera.pixelWidth;
    }

    private void Update()
    {            
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
            if (this.transform.position.y < maxHeight)
                this.transform.position += new Vector3(0, speed, 0);
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
            if (this.transform.position.x > -maxWidth)
                this.transform.position += new Vector3(-speed, 0, 0);
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
            if (this.transform.position.y > -maxHeight)
                this.transform.position += new Vector3(0, -speed, 0);
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
            if (this.transform.position.x < maxWidth)
                this.transform.position += new Vector3(speed, 0, 0);
    }
}
