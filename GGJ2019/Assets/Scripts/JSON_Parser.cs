using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JSON_Parser : MonoBehaviour {
    public TextAsset test;
    public Scenario sc;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetKeyDown( KeyCode.Space ) ) {
            LoadFile( test );
        }
	}
    void LoadScenario( string fileName ) {

    }
    void LoadFile(TextAsset textAsset) {
        string fullString = textAsset.text;
        // fullString = Expand( fullString );
         sc = GGJ19_Parse( fullString );
    }

    private string Expand( string inText ) {
        string outText = inText.Split( '{' )[1];
        outText = outText.Split( '}' )[0];
        return outText;
    }
    private Scenario GGJ19_Parse( string inText ) {
        Scenario scenario = JsonUtility.FromJson<Scenario>( inText );
        return scenario;
    }
}
[System.Serializable]
public class Scenario {
    public string ScenarioName = "NULL";
    public ToastType Toast;
    public Stage[] Stages;
}

[System.Serializable]
public class ToastType {
    public string Name = "NULL";
    public int Count;
    public int[] Pie;
}

[System.Serializable]
public class Stage {
    public int id = -1;
    public string Name;
    public int Number;
}

[System.Serializable]
public class Stage2 {
    public int id;
    public string completion;
    public int time;
    public Thing[] things;
}


[System.Serializable]
public class Thing {
    public string type;
    public string owner;
    public string text;
}