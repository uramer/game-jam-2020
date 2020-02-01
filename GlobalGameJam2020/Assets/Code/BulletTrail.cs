using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] private float speed;
    private GameObject target;

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.transform.gameObject.tag == "Enemy"){ }
        else*/
        Destroy(this);
        if(other.gameObject == target)
            Destroy(target);
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
