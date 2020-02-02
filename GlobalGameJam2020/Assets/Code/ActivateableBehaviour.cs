using UnityEngine;

public class ActivateableBehaviour : MonoBehaviour {
    [SerializeField] public bool winCondition = false;
    [SerializeField] public bool complex = false;

    public virtual bool Activate(Unit unit) {
        if(complex && !unit.canActivateComplex) return false;
        if(winCondition) {
            LevelManager.Get().Win();
        }
        return true;
    }
    
}