using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    NONE,
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public class HandManager : MonoBehaviour {
    public ControllerManager c_manager;


    public static Vector2 Stick;
    public static Vector2 DPad;

    public static Direction StickDir;
    public static Direction DPadDir;
    public static Direction LastStickDir;
    public static Direction LastDPadDir;

    public static Direction GetDirection( Vector2 input ) {
        if ( input.sqrMagnitude <= 0 )
            return Direction.NONE;
        if ( Mathf.Abs( input.x ) > Mathf.Abs( input.y ) ) {
            if ( input.x > 0 )
                return Direction.RIGHT;
            else
                return Direction.LEFT;
        }
        else {
            if ( input.y > 0 )
                return Direction.UP;
            else
                return Direction.DOWN;
        }
    }

    public static bool GetStick( Direction dir ) {
        Stick = new Vector2( Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) );
        StickDir = GetDirection( Stick );
        if ( LastStickDir == StickDir )
            return false;
        else
            return dir == StickDir;
    }

    public static bool GetDPad( Direction dir ) {
        DPadDir = GetDirection( DPad );
        if ( LastDPadDir == DPadDir )
            return false;
        else
            return dir == DPadDir;
    }

    private Transform LeftHand, RightHand, ThirdHand;
    private float stickOffset = 0.04f, padOffset = 0.04f;
    Transform rig;
    Transform StickAnchor;
    Transform DPadAnchor;

    public bool TouchMode = true;
    public float handAcceleration = 10.0f;
    public float handSpeed = 0.0f;
    public float maxHandSpeed = 30.0f;
    public float GrabRange = 0.2f;
    public float grabSpeed = 2.0f;
    private float PawLength = 1.0f;
    private float PawYaw = 0f;
    private float PawPitch = 0f;
    Transform pawEnd;
    GameObject grabbedObject = null;
    private float zPos;
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
        
        Vector2 stickInput = Stick = new Vector2( Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) );
        Vector2 dPadInput = DPad = new Vector2( Input.GetAxis( "DPad_H" ), Input.GetAxis( "DPad_V" ) );


        Vector3 pos = transform.localPosition;
        pos.z = Mathf.Lerp(pos.z, zPos, 0.08f);
        transform.localPosition = pos;

        //TouchMode = Input.GetButton( "Z" );
        if ( Input.GetButtonDown( "Z" ) )
            TouchMode = !TouchMode;
        if ( TouchMode ) {
            CameraController.targetFOV = 75f;
            zPos = -0.82f;
            if ( stickInput.sqrMagnitude <= 0.0f )
                handSpeed = 0.0f;
            else {
                handSpeed = Mathf.Min(maxHandSpeed, handSpeed + (handAcceleration * Time.deltaTime));
            }
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
            CheckHighlights();
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
                PawLength += grabSpeed * Time.deltaTime;
            if ( Input.GetButton( "C_DOWN" ) )
                PawLength = Mathf.Max( 1.0f, PawLength - grabSpeed * Time.deltaTime );

            PawPitch =  Mathf.Clamp( PawPitch -stickInput.y * Time.deltaTime * handSpeed * (2f/PawLength), -30f, 30f);
            PawYaw = Mathf.Clamp( PawYaw  + stickInput.x * Time.deltaTime * handSpeed * ( 2f / PawLength ), -60f, 60f );
            Quaternion rot = Quaternion.AngleAxis( PawPitch, transform.right ) * Quaternion.AngleAxis( PawYaw, transform.up );
            ThirdHand.localRotation = rot;
            ThirdHand.localScale = new Vector3( 1.0f, 1.0f, PawLength );

        }
        else {
            zPos = -0.35f;
            CameraController.targetFOV = 40f;
            rig.gameObject.SetActive( true );
            ThirdHand.gameObject.SetActive( false );

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

        LastStickDir = GetDirection( Stick);
        LastDPadDir = GetDirection( DPad );
	}

    void CheckHighlights() {
        GameObject go = null;
        Collider[] list = Physics.OverlapSphere( pawEnd.position, GrabRange, ~( 1 << 8 ) );
        if ( list.Length > 0 ) {
            print( "Found: " + list.Length );
            float range = Mathf.Infinity;
            Rigidbody rb = null;

            foreach ( Collider col in list ) {
                rb = col.GetComponent<Rigidbody>();
                if ( rb != null ) {
                    float dist = Vector3.Distance( col.transform.position, pawEnd.position );
                    if ( dist < range ) {
                        go = col.gameObject;
                        range = dist;
                    }
                }
            }

        }

        if ( go != null ) {
            Highlight( go.GetComponent<Renderer>() );
        }
        else {
            Highlight( null );
        }
    }
    Renderer currentHighlight;
    Color baseCol;
    void Highlight(Renderer ren) {
        if ( currentHighlight == ren )
            return;
        if ( currentHighlight != null ) {
            currentHighlight.material.color = baseCol;
            currentHighlight = null;
        }
        if ( ren != null ) {
            currentHighlight = ren;
            baseCol = currentHighlight.material.color;
            currentHighlight.material.color *= 1.5f;
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
