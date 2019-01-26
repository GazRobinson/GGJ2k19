using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public TextAsset scenarioText;
    private Scenario currentScenario;
    private Stage currentStage;
    private Scenario[] scenarioList;
    private JSON_Parser json;
    private DialogueBuilder dialogueBuilder;

    private System.Action stageEnded;

    private int Stage = 0;
	// Use this for initialization
	void Start () {
        json = GetComponent<JSON_Parser>();
        dialogueBuilder = GetComponent<DialogueBuilder>();
        stageEnded += dialogueBuilder.HandleStageEnd;
        scenarioList = json.LoadScenario( scenarioText );

    }
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetKeyDown( KeyCode.Space ) ) {
            PickScenario();
            BeginScenario();
        }
	}
    void PickScenario() {
        currentScenario = scenarioList[Random.Range( 0, scenarioList.Length )];
    }
    void BeginScenario() {
        print( "Starting scenario: " + currentScenario.ScenarioName );
        Stage = 0;
        RunStage();
    }
    void RunStage() {
        print( "Run stage " + Stage );
        if ( Stage >= currentScenario.Stages.Length ) {
            Debug.Log( "End scenario" );
            return;
        }

        currentStage = currentScenario.Stages[Stage];

        //Check completion status
        switch ( currentStage.completion ) {
            case "time":
                print( "Timed" );
                Invoke( "EndStage", currentStage.time );
                break;
        }
        DoThings();
    }
    void EndStage() {
        Stage++;
        if ( stageEnded != null ) {
            stageEnded();
        }
        RunStage();
    }
    void DoThings() {
        if ( currentStage.things != null ) {
            foreach ( Thing t in currentStage.things ) {
                switch ( t.type ) {
                    case "Dialogue":
                        DialogueBox box = dialogueBuilder.Build( t );
                        box.SetText( t.text );
                        break;
                }
            }
        }
    }
}
