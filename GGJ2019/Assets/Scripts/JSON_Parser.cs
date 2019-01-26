using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JSON_Parser : MonoBehaviour {
    public TextAsset test;
    public Scenario sc;
  
    void LoadScenario( string fileName ) {

    }
    public Scenario[] LoadScenario( TextAsset textAsset ) {
        Scenarios scs = JsonUtility.FromJson<Scenarios>( textAsset.text );
        return scs.scenarios;
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
public class Scenarios {
    public Scenario[] scenarios;
}
[System.Serializable]
public class Scenario {
    public string ScenarioName = "NULL";    
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