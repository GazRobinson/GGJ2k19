using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour {
    public ControllerManager c_manager;

    private Transform LeftHand, RightHand;
    private float stickOffset = 0.04f, padOffset = 0.04f;
    Transform StickAnchor;
    Transform DPadAnchor;

	// Use this for initialization
	void Start () {
        LeftHand = transform.Find( "Paw_left" );
        RightHand = transform.Find( "Paw_right" );
        StickAnchor = transform.Find( "StickAnchor" );
        DPadAnchor = transform.Find( "PadAnchor" );
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 stickInput = new Vector2( Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) );
        Vector2 dPadInput = new Vector2( Input.GetAxis( "DPad_H" ), Input.GetAxis( "DPad_V" ) );
        print( stickInput.y );
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
}
