using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour {
    [SerializeField] public float speed = 4;
    [SerializeField] public float detectionRange = 3;
    [SerializeField] public float sightRange = 4;
    [SerializeField] public float activateDelay = 1f;
    [SerializeField] public float activateDistance = 1.5f;
    [SerializeField] public bool canDestroy = false;
    [SerializeField] public bool canActivateComplex = false;
    [SerializeField] public bool essential = false;
    protected NavMeshAgent agent = null;

    protected GameObject internalGameObject = null;

    protected ActivateableBehaviour activateTarget = null;

    private float activateWait = 0f;

    public enum State {
        Idle,
        Moving,
        Activating,
        Dead
    }

    protected State state = State.Idle;

    protected float DistanceFromMe(GameObject a) {
        return (a.transform.position - this.transform.position).magnitude;
    }

    protected float DistanceFromMe(MonoBehaviour a) {
        return (a.transform.position - this.transform.position).magnitude;
    }

    protected void Start() {
        agent = GetComponent<NavMeshAgent>();
        internalGameObject = transform.GetChild(0).gameObject;
    }

    protected void Update() {
        if(state == State.Dead) return;
        if(activateTarget != null) {
            //Debug.Log($"{DistanceFromMe(activateTarget)} {activateWait} {Time.deltaTime}");
            if(state == State.Activating) {
                if(activateWait <= 0) {
                    activateTarget.Activate(this);
                    state = State.Idle;
                    agent.isStopped = false;
                    activateTarget = null;
                }
                activateWait -= Time.deltaTime;
            }
            else {
                if(DistanceFromMe(activateTarget) < activateDistance) {
                    state = State.Activating;
                    activateWait = activateDelay;
                    agent.isStopped = true;
                }
            }
        }
    }

    public void Pathfind(Vector3 position) {
        /*if(state == State.Activating || state == State.Dying || state == State.Dead) {
            return;
        }*/
        agent.destination = position;
        agent.speed = speed;
    }

    public void Activate(ActivateableBehaviour activateable) {
        if(state != State.Activating)
            activateTarget = activateable;
    }

    public State GetState() {
        return state;
    }

    public GameObject GetInternalGameObject() {
        return internalGameObject;
    }

    public void Die() {
        state = State.Dead;
        internalGameObject.GetComponent<Rigidbody2D>().simulated = false;
        internalGameObject.GetComponent<SpriteRenderer>().enabled = false;
        agent.isStopped = true;
        if(essential) {
            LevelManager.Get().Lose();
        }
    }

}