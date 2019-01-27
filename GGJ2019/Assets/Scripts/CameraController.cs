using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static float targetFOV = 75f;
    private float startFOV = 75f;


    private float MaxYRotation = 90.0f;

    private Vector3 targetEulerAngles = Vector3.zero;
    private Quaternion targetQuat;
    private Quaternion currentQuat;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( !Mathf.Approximately(Camera.main.fieldOfView, targetFOV )) {
            Camera.main.fieldOfView = Mathf.Lerp( Camera.main.fieldOfView, targetFOV, 0.1f );
        }
       TurnHead( Input.GetAxis( "DPad_H" ));
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
