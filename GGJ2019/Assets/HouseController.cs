using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour {
    public Transform L_Wall, R_Wall, B_Wall, Floor;
    private Vector3 L_Original, R_Original;
    public float timeMultiplier = 0.3f;
    public float breatheAmplitude = 0.1f;
	// Use this for initialization
	void Start () {
        L_Original = L_Wall.localPosition;
        R_Original = R_Wall.localPosition;
    }
	
	// Update is called once per frame
	void Update () {
        Breathe( Time.time * timeMultiplier );
	}

    void Breathe( float t ) {
        L_Wall.localPosition = L_Original + Vector3.right * Mathf.Sin( t ) * breatheAmplitude;
        R_Wall.localPosition = R_Original - Vector3.right * Mathf.Sin( t ) * breatheAmplitude;
    }
}
