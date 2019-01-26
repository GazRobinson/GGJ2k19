using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {
    public static UIManager Instance;
	// Use this for initialization
	void Awake () {
        Instance = this;
	}

    public void AddChild( GameObject go ) {
       // go.transform.SetParent( transform );
    }

	// Update is called once per frame
	void Update () {
		
	}
}
