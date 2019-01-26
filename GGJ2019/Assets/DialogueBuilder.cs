using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBuilder : MonoBehaviour {
    public DialogueBox boxPrefab;

    public List<DialogueBox> currentBoxes = new List<DialogueBox>();

    public void Build( Thing t ) {
        GameObject go = GameObject.Find( t.owner );
        DialogueManager dm = go.GetComponent<DialogueManager>();

        DialogueBox db = MakeBox( dm );
        UIManager.Instance.AddChild( db.gameObject );
        currentBoxes.Add( db );
    }
    public DialogueBox MakeBox( DialogueManager dm ) {
        DialogueBox db = Instantiate<DialogueBox>( boxPrefab );
        db.Init( dm );
        return db;
    }

    public void HandleStageEnd() {
        foreach ( DialogueBox db in currentBoxes ) {
            Destroy( db.gameObject );
        }
        currentBoxes.Clear();
    }
}
