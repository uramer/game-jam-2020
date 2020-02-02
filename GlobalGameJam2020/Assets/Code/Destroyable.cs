using UnityEngine;

public class Destroyable : ActivateableBehaviour {

    public override bool Activate(Unit unit) {
        if(!base.Activate(unit)) return false;
        if(unit.canDestroy)
            Destroy(gameObject);
        return true;
    }
    
}