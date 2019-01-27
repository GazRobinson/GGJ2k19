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
    private System.Action ConfirmPressed;
    private float stageTime = 0;
    private float targetTime = 0;
    private int Stage = 0;
    bool runningScenario = false;


    public float GameTimer = 0f;

	// Use this for initialization
	void Start () {
        json = GetComponent<JSON_Parser>();
        dialogueBuilder = GetComponent<DialogueBuilder>();
        stageEnded += dialogueBuilder.HandleStageEnd;
        scenarioList = json.LoadScenario( scenarioText );

    }
	
	// Update is called once per frame
	void Update () {
        GameTimer += Time.deltaTime;
        stageTime += Time.deltaTime;
        if ( Input.GetKeyDown( KeyCode.Space ) || GameTimer > Random.Range(10f, 15f) ) {
            PickScenario();
            BeginScenario();
        }
        if ( stageTime > targetTime && Input.GetButton( "A" ) ) {
            if ( ConfirmPressed != null ) {
                ConfirmPressed();
            }
        }
	}
    void PickScenario() {
        currentScenario = scenarioList[Random.Range( 0, scenarioList.Length )];
    }
    bool BeginScenario() {
        if ( runningScenario )
            return false;
        print( "Starting scenario: " + currentScenario.ScenarioName );
        runningScenario = true;
        Stage = 0;
        RunStage();
        return true;
    }
    void RunStage() {
        print( "Attempting Run stage " + Stage );

        bool good = false;
        if ( !waterfall ) {
            for ( int i = 0; i < currentScenario.Stages.Length; i++ ) {

                if ( currentScenario.Stages[i].id == Stage ) {

                    print( "Found stage " + Stage );
                    currentStage = currentScenario.Stages[i];
                    good = true;
                }
            }
        }
        else {
            if ( Stage >= currentScenario.Stages.Length ) {
                Debug.Log( "End scenario" );
                GameTimer = 0f;
                runningScenario = false;
                return;
            }
            good = true;
            currentStage = currentScenario.Stages[Stage];
        }
        if ( !good ) {
            Debug.LogError( "Couldn't find stage ID:" + Stage.ToString() + " in " + currentScenario.ScenarioName );
            runningScenario = false;
            return;
        }
        stageTime = 0f;
        //currentStage = currentScenario.Stages[Stage];

        //Check completion status
        switch ( currentStage.completion ) {
            case "time":
                print( "Timed" );
                timer = currentStage.time;
                //Invoke( "EndStage", currentStage.time );
                break;
            case "action":

                break;
            case "confirm":
                ConfirmPressed += EndStage;
                targetTime = currentStage.time;
                break;
        }
        DoThings();
    }
    bool waterfall = false;
    void EndStage() {
        waterfall = true;
        Stage++;
        if ( stageEnded != null ) {
            stageEnded();
        }
        stageEnded = dialogueBuilder.HandleStageEnd;
        RunStage();
    }
    void EndStage(int nextStage) {
        waterfall = false;
        Debug.Log( "Setting next stage to: " + nextStage );
        Stage = nextStage;
        if ( stageEnded != null ) {
            stageEnded();
        }
        stageEnded = dialogueBuilder.HandleStageEnd;
        RunStage();
    }
    float timer = 0f;
    void StartTimer() {
        
        Invoke( "EndStage", currentStage.time );
    }
    void DoThings() {
        if ( currentStage.things != null ) {
            foreach ( Thing t in currentStage.things ) {
                switch ( t.type ) {
                    case "dialogue":
                        DialogueBox box = dialogueBuilder.Build( t );
                        box.SetText( t.text );
                        if(currentStage.completion == "time")
                            box.LookedAt += StartTimer;
                        break;
                    case "fetch":
                        FetchManager fm = gameObject.AddComponent<FetchManager>();
                        fm.Init( t, EndStage );
                        stageEnded += fm.Cleanup;
                        break;
                }
            }
        }
    }
}
