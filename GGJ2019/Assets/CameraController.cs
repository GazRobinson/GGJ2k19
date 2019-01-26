using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private float MaxYRotation = 90.0f;

    private Vector3 targetEulerAngles = Vector3.zero;
    private Quaternion targetQuat;
    private Quaternion currentQuat;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        TurnHead( Input.GetAxis( "Horizontal" ));
        if ( transform.eulerAngles != targetEulerAngles ) {
            //transform.localEulerAngles =  Vector3.Lerp( transform.eulerAngles, targetEulerAngles, 0.1f );
            transform.localRotation = Quaternion.Lerp( transform.localRotation, targetQuat, 0.1f );
        }
	}
    public void TurnHead( float target ) {
        targetEulerAngles = new Vector3( 0.0f, MaxYRotation * target, 0.0f );
        targetQuat = Quaternion.AngleAxis( MaxYRotation * target, Vector3.up );
    }
}
