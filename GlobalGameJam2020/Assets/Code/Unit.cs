using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour {
    [SerializeField] public float speed = 4;
    [SerializeField] public float detectionRange = 3;
    [SerializeField] public float sightRange = 4;
    protected NavMeshAgent agent = null;
    protected GameObject internalObject = null;

    public enum State {
        Idle,
        Moving,
        Dying,
        Dead
    }

    protected State animationState = State.Idle;

    /*protected static GameObject[] GetUnits() {
        return GameObject.Find("/Units").transform.children;
    }*/

    public void Start() {
        agent = GetComponent<NavMeshAgent>();
        internalObject = transform.GetChild(0).gameObject;
    }

    public void Pathfind(Vector3 position) {
        agent.destination = position;
        agent.speed = speed;
    }

    public void Update() {
        /*Debug.Log(this.transform.position);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit m_HitInfo = new RaycastHit();
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
                agent.SetDestination(rayCastHit);
        }*/
    }

}