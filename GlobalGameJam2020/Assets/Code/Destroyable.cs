using UnityEngine;

public class Destroyable : ActivateableBehaviour {

    public override void Activate(Unit unit) {
        base.Activate(unit);
        if(unit.canDestroy)
            Destroy(gameObject);
    }
    
}