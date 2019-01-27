using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGameManager : MonoBehaviour {
    public SubGame m_LoadedGame;
    private bool m_PowerOn = false;
    public Transform screen; //The transfrom that screen elements are added to

    public void PowerOn() {
        if ( m_PowerOn )
            return;
        m_PowerOn = true;
        if ( m_LoadedGame != null ) {
            m_LoadedGame.StartGame(this);
        }
    }

	// Use this for initialization
	void Start () {
        PowerOn();
	}
	
	// Update is called once per frame
	void Update () {
        if ( m_PowerOn && m_LoadedGame != null) {
            m_LoadedGame.DoUpdate();
        }
	}
}
