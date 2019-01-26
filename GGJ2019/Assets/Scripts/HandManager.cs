using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
    public ControllerManager c_manager;

    private Transform LeftHand, RightHand, ThirdHand;
    private float stickOffset = 0.04f, padOffset = 0.04f;
    Transform rig;
    Transform StickAnchor;
    Transform DPadAnchor;

    public bool TouchMode = false;

    public float GrabRange = 0.2f;
    private float PawLength = 1.0f;
    private float PawYaw = 0f;
    private float PawPitch = 0f;
    Transform pawEnd;
    GameObject grabbedObject = null;

    // Use this for initialization
    void Start () {
        rig = transform.Find( "Rig" );
        ThirdHand = transform.Find( "Long_Paw" );
        pawEnd = ThirdHand.GetChild( 0 ).GetChild( 1 );
        LeftHand = rig.Find( "Paw_left" );
        RightHand = rig.Find( "Paw_right" );
        StickAnchor = rig.Find( "StickAnchor" );
        DPadAnchor = rig.Find( "PadAnchor" );
    }

	// Update is called once per frame
	void Update () {
        
        Vector2 stickInput = new Vector2( Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) );
        Vector2 dPadInput = new Vector2( Input.GetAxis( "DPad_H" ), Input.GetAxis( "DPad_V" ) );
        TouchMode = Input.GetButton( "Z" );
        if ( TouchMode ) {
            if ( Input.GetButtonDown( "R" ) ) {
                Collider[] list = Physics.OverlapSphere( pawEnd.position, GrabRange, ~(1<<8) );
                if ( list.Length > 0 ) {
                    print( "Found: " + list.Length );
                    float range = Mathf.Infinity;
                    Rigidbody rb = null;
                    
                    foreach ( Collider col in list ) {
                        rb = col.GetComponent<Rigidbody>();
                        if ( rb != null ) {
                            float dist = Vector3.Distance( col.transform.position, pawEnd.position );
                            if ( dist < range ) {
                                grabbedObject = col.gameObject;
                                range = dist;
                            }
                        }
                    }

                    if ( grabbedObject != null ) {
                        grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                       // grabbedObject.transform.SetParent( pawEnd );
                    }
                }
            }
            if ( Input.GetButtonUp( "R" ) ) {
                if ( grabbedObject != null ) {

                        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                    //grabbedObject.transform.SetParent( null );
                    grabbedObject = null;
                }
            }
            if ( grabbedObject != null ) {
                grabbedObject.transform.position = pawEnd.position;
            }

            rig.gameObject.SetActive( false );
            ThirdHand.gameObject.SetActive( true );

            if ( Input.GetButton( "C_UP" ) )
                PawLength += 1.0f * Time.deltaTime;
            if ( Input.GetButton( "C_DOWN" ) )
                PawLength = Mathf.Max( 1.0f, PawLength - 1.0f * Time.deltaTime );

            PawPitch =  Mathf.Clamp( PawPitch -stickInput.y * Time.deltaTime * 15.0f, -30f, 30f);
            PawYaw = Mathf.Clamp( PawYaw  + stickInput.x * Time.deltaTime * 15.0f, -60f, 60f );
            Quaternion rot = Quaternion.AngleAxis( PawPitch, transform.right ) * Quaternion.AngleAxis( PawYaw, transform.up );
            ThirdHand.localRotation = rot;
            ThirdHand.localScale = new Vector3( 1.0f, 1.0f, PawLength );

        }
        else {
            rig.gameObject.SetActive( true );
            ThirdHand.gameObject.SetActive( false );
          //  print( stickInput.y );
            if ( stickInput.sqrMagnitude > 0 ) {
                LeftHand.localPosition = StickAnchor.localPosition + ( Vector3 )( stickInput * stickOffset );
                c_manager.SetStick( stickInput );
                c_manager.SetDPad( Vector2.zero );
            }
            else if ( dPadInput.sqrMagnitude > 0 ) {
                LeftHand.localPosition = DPadAnchor.localPosition + ( Vector3 )( dPadInput * padOffset );
                c_manager.SetDPad( dPadInput );
                c_manager.SetStick( Vector2.zero );
            }
            else {
                LeftHand.localPosition = StickAnchor.localPosition;
                c_manager.SetStick( Vector2.zero );
                c_manager.SetDPad( Vector2.zero );

            }
            GetButtons();
        }
	}
    void GetButtons() {
        //Down
        if ( Input.GetButtonDown( "A" ) )
            c_manager.PressButton( Button.A );
        if ( Input.GetButtonDown( "B" ) )
            c_manager.PressButton( Button.B );
        if ( Input.GetButtonDown( "START" ) )
            c_manager.PressButton( Button.START );
        if ( Input.GetButtonDown( "C_UP" ) )
            c_manager.PressButton( Button.C_UP );
        if ( Input.GetButtonDown( "C_RIGHT" ) )
            c_manager.PressButton( Button.C_RIGHT );
        if ( Input.GetButtonDown( "C_DOWN" ) )
            c_manager.PressButton( Button.C_DOWN );
        if ( Input.GetButtonDown( "C_LEFT" ) )
            c_manager.PressButton( Button.C_LEFT );

        //Up
        if ( Input.GetButtonUp( "A" ) )
            c_manager.ReleaseButton( Button.A );
        if ( Input.GetButtonUp( "B" ) )
            c_manager.ReleaseButton( Button.B );
        if ( Input.GetButtonUp( "START" ) )
            c_manager.ReleaseButton( Button.START );
        if ( Input.GetButtonUp( "C_UP" ) )
            c_manager.ReleaseButton( Button.C_UP );
        if ( Input.GetButtonUp( "C_RIGHT" ) )
            c_manager.ReleaseButton( Button.C_RIGHT );
        if ( Input.GetButtonUp( "C_DOWN" ) )
            c_manager.ReleaseButton( Button.C_DOWN );
        if ( Input.GetButtonUp( "C_LEFT" ) )
            c_manager.ReleaseButton( Button.C_LEFT );
    }

    private void OnDrawGizmos() {
        if ( pawEnd != null ) {
            Gizmos.color = new Color( 1.0f, 0.0f, 0.0f, 0.25f );
            Gizmos.DrawSphere( pawEnd.position, GrabRange );
        }
    }
}
