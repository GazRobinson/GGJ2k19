using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDR_Message : MonoBehaviour {
    public float maxTime = 0.7f;
    float t = 0.0f;
    private SpriteRenderer spriteRenderer;
    public void Show(Vector3 pos) {
        transform.position = pos;
        t = maxTime;
    }
	// Use this for initialization
	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        t -= Time.deltaTime;
        if ( t >= 0f ) {
            Color col = spriteRenderer.color;
            col.a = Mathf.Lerp( 1f, 0f, Mathf.InverseLerp( maxTime, 0f, t ) );
            spriteRenderer.color = col;
        }
	}
}
