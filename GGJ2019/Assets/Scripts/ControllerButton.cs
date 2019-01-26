using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerButton : MonoBehaviour {
    Vector3 releasePos;
    Vector3 pressPos;
	// Use this for initialization
	void Start () {
        releasePos = transform.localPosition;
        pressPos = transform.localPosition - (transform.up * 0.002f);
	}
	

    public void Press() {
        transform.localPosition = pressPos;
    }
    public void Release() {
        transform.localPosition = releasePos;
    }
}
