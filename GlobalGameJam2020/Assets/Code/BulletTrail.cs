using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 targetPosition;

    private void OnCollisionEnter2D(Collision2D other)
    {
        Unit unit = other.gameObject.GetComponentInParent<Unit>();
        if (unit != null && unit.gameObject.tag == "Player"){
            unit.Die();
        }
        if(other.gameObject != transform.parent)
            Destroy(gameObject);
    }

    private void Update() {
        Vector3 delta = targetPosition - transform.position;
        if(delta.magnitude < 0.1f) Destroy(gameObject);
        //else transform.position += speed * Time.deltaTime * delta.normalized;
    }

    private void Start()
    {
        targetPosition = GetComponentInParent<GuardMovement>().target.transform.position;
        Vector3 delta = targetPosition - transform.position;
        Rigidbody2D rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(speed * rigidbody.mass * delta.normalized, ForceMode2D.Impulse);
    }
}
