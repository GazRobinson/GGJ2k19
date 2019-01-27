using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour {
    protected Image bg;
    protected CanvasGroup canvasGroup;
    Text txt;
    int textSpeed = 2;
    string targetText = "";
    string currentText = "";
    int count = 0;
    bool left = true;
    float t = 0f;
    float tMult = 1f;
    bool doKill = false;
    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Init( DialogueManager dm ) {
        bg = transform.GetChild(0).GetComponent<Image>();
        bg.color = dm.textBoxColor;
        left = dm.left;
        bg.transform.localScale = new Vector3( left ? 1f : -1f, 1f, 1f );
        txt = transform.GetChild( 1 ).GetComponent<Text>();
        OnInit(dm);
    }
    protected virtual void OnInit( DialogueManager dm ) {

    }
    public void SetText( string target ) {
        count = 0;
        targetText = target;
    }
    private void Update() {
        t += Time.deltaTime * tMult;

        if (t > 1f && targetText.Length > 0 ) {
            count += textSpeed;
            txt.text = targetText.Substring( 0, Mathf.Min( count, targetText.Length ));
        }
        DoUpdate();

        if ( t < 0 && doKill ) {
            Destroy( gameObject );
        }
    }
    protected virtual void DoUpdate() {
        canvasGroup.alpha = Mathf.Min( 1f, t );
    }
    public void Kill() {
        t = 1f;
        tMult = -1f;
        doKill = true;
    }
}
