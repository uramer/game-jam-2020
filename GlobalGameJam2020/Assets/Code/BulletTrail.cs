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
        if (unit.gameObject.tag == "Player"){
            unit.Die();
        }
        if(other.gameObject != transform.parent)
            Destroy(gameObject);
    }

    private void Update() {
        transform.position += speed * Time.deltaTime * (targetPosition - transform.position).normalized;
    }

    private void Start()
    {
        targetPosition = GetComponentInParent<GuardMovement>().target.transform.position;
    }
}
