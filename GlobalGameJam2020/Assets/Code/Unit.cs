using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour {
    [SerializeField] public float speed = 4;
    [SerializeField] public float detectionRange = 3;
    [SerializeField] public float sightRange = 4;
    protected NavMeshAgent agent = null;

    protected GameObject internalGameObject = null;

    public enum State {
        Idle,
        Moving,
        Dying,
        Dead
    }

    protected State state = State.Idle;

    protected void Start() {
        agent = GetComponent<NavMeshAgent>();
        internalGameObject = transform.GetChild(0).gameObject;
    }

    public void Pathfind(Vector3 position) {
        agent.destination = position;
        agent.speed = speed;
    }

    public State GetState() {
        return state;
    }

    public void Die() {
        state = State.Dead;
        internalGameObject.GetComponent<Rigidbody2D>().simulated = false;
        internalGameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

}