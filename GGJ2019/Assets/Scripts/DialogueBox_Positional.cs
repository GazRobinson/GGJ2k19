using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBox_Positional : DialogueBox {
    public Transform owner;
    protected override void DoUpdate() {
        base.DoUpdate();
        Vector3 lookAtTarget = ( owner.position - Camera.main.transform.position );
        lookAtTarget.y = 0f;
        Vector3 lookAt = Camera.main.transform.forward;
        lookAt.y = 0f;
        float dot = Vector2.Dot( lookAtTarget, lookAt );
     //   Debug.Log( dot );   
        canvasGroup.alpha = Mathf.Min(1f, dot);
    }
    protected override void OnInit( DialogueManager dm ) {
        base.OnInit( dm );
        owner = dm.transform;
    }
}
