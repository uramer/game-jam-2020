using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 target;
    
    private void Movement()
    {
        this.transform.position += speed * (target - this.transform.position).normalized * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.gameObject.tag == "Enemy"){ }
        else
            Destroy(this);
    }

    private void Awake()
    {
        target = GetComponentInParent<GuardMovement>().target;
        transform.parent = null;
    }

    private void Update() 
    {
        Movement();
    }
}
