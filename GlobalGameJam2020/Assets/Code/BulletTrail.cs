using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] private float speed;
    private Unit target;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy"){}
        else
            Destroy(this.gameObject);

        if(other.gameObject == target){}
    }

    private void Start()
    {
        target = GetComponentInParent<GuardMovement>().target;
        //transform.parent = null;
        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(
            (target.transform.position - this.transform.position).normalized * speed, ForceMode2D.Impulse);
    }
}
