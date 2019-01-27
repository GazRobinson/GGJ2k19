using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubGame : MonoBehaviour {
    protected SubGameManager SGM;
    public void StartGame(SubGameManager sgm) {
        SGM = sgm;
        OnStartGame();
    }
    protected virtual void OnStartGame() {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public void DoUpdate () {
        OnUpdate();
	}
    protected virtual void OnUpdate() {

    }
}
