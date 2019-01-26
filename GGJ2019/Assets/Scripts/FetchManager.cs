using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchManager : MonoBehaviour {
    GameObject targetObject;
    GameObject ownerObject;
    int stageTarget;
    System.Action<int> endTarget;

    bool complete = false;

    public void Init(Thing t, System.Action<int> nextStage) {
        targetObject = GameObject.Find( t.target );
        ownerObject = GameObject.Find( t.owner );
        stageTarget = t.next;
        endTarget = nextStage;
    }

    void Succeed() {
        endTarget( stageTarget );
        complete = true;
    }

    public void Cleanup() {
        Destroy( this );
    }

    private void Update() {
        if ( !complete && Vector3.SqrMagnitude( targetObject.transform.position - ownerObject.transform.position ) < ( 0.5f * 0.5f ) ) {
            Debug.Log( ownerObject.name + " complete!" );
            Succeed();
        }
    }
}
