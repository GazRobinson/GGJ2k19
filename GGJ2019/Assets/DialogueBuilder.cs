using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBuilder : MonoBehaviour {
    public DialogueBox boxPrefab;
    public DialogueBox_Positional boxPrefab_Positional;

    public List<DialogueBox> currentBoxes = new List<DialogueBox>();

    public DialogueBox Build( Thing t ) {
        GameObject go = GameObject.Find( t.owner );
        DialogueManager dm = go.GetComponent<DialogueManager>();

        DialogueBox db = MakeBox( dm );
        UIManager.Instance.AddChild( db.gameObject );
        currentBoxes.Add( db );

        return db;
    }
    public DialogueBox MakeBox( DialogueManager dm ) {
        DialogueBox db;
        if ( !dm.positional ) {
            db = Instantiate<DialogueBox>( boxPrefab, UIManager.Instance.transform );
        }
        else {
            db = Instantiate<DialogueBox_Positional>( boxPrefab_Positional, UIManager.Instance.transform );
        }
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
