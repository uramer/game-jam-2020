using UnityEngine;

public class ActivateableBehaviour : MonoBehaviour {
    [SerializeField] public bool winCondition = false;

    public virtual void Activate(Unit unit) {
        if(winCondition) {
            LevelManager manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            manager.Win();
        }
    }
    
}