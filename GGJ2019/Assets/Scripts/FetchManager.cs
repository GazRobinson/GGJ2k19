using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchManager : MonoBehaviour {
    [SerializeField]
    GameObject targetObject;
    [SerializeField]
    GameObject ownerObject;
    int stageTarget;
    System.Action<int> endTarget;

    bool complete = false;
    float range = 0.5f;

    public void Init(Thing t, System.Action<int> nextStage) {
        targetObject = GameObject.Find( t.target );
        ownerObject = GameObject.Find( t.owner );
        stageTarget = t.next;
        endTarget = nextStage;
    }

    void Succeed() {
        complete = true;
        endTarget( stageTarget );
    }

    public void Cleanup() {
        print( "Cleanup fetchmanager" );
        Destroy( this );
    }

    private void Update() {
        if ( !complete && Vector3.SqrMagnitude( targetObject.transform.position - ownerObject.transform.position ) < ( range * range ) ) {
            Debug.Log( ownerObject.name + " complete!" );
            Succeed();
        }
    }


    private void OnDrawGizmos() {
            Gizmos.color = new Color( 0.0f, 1.0f, 0.0f, 0.25f );
        Gizmos.DrawSphere( ownerObject.transform.position, range );
    }
}
