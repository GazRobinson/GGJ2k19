using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Button {
    A,
    B,
    START,
    L,
    R,
    C_UP,
    C_RIGHT,
    C_DOWN,
    C_LEFT
}

public class ControllerManager : MonoBehaviour {
    public Transform controlStick;
    public Transform dPad;

    public ControllerButton A_Button;
    public ControllerButton B_Button;
    public ControllerButton Start_Button;

    public ControllerButton C_UP;
    public ControllerButton C_RIGHT;
    public ControllerButton C_DOWN;
    public ControllerButton C_LEFT;


    private float maxStickTilt = 20.0f;
    private float maxPadTilt = 5.0f;

    private Transform currentButton;
    private Vector3 restPos;
    float buttonT = 0f;

    public void SetStick( Vector2 input ) {
        controlStick.localRotation = Quaternion.Euler( maxStickTilt * -input.y, 0.0f, maxStickTilt * input.x );
    }

    public void SetDPad( Vector2 input ) {
        dPad.localRotation = Quaternion.Euler( maxPadTilt * -input.y, 0.0f, maxPadTilt * input.x );
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( currentButton != null ) {

        }
	}
    public void PressButton(Button btn) {
        switch ( btn ) {
            case Button.A:
                A_Button.Press();
                break;
            case Button.B:
                B_Button.Press();
                break;
            case Button.START:
                Start_Button.Press();
                break;
            case Button.C_UP:
                C_UP.Press();
                break;
            case Button.C_RIGHT:
                C_RIGHT.Press();
                break;
            case Button.C_DOWN:
                C_DOWN.Press();
                break;
            case Button.C_LEFT:
                C_LEFT.Press();
                break;
        }
    }
    public void ReleaseButton( Button btn ) {
        switch ( btn ) {
            case Button.A:
                A_Button.Release();
                break;
            case Button.B:
                B_Button.Release();
                break;
            case Button.START:
                Start_Button.Release();
                break;
            case Button.C_UP:
                C_UP.Release();
                break;
            case Button.C_RIGHT:
                C_RIGHT.Release();
                break;
            case Button.C_DOWN:
                C_DOWN.Release();
                break;
            case Button.C_LEFT:
                C_LEFT.Release();
                break;
        }
    }
}
