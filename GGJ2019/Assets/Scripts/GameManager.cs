﻿using System.Collections;
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
	// Use this for initialization
	void Start () {
        json = GetComponent<JSON_Parser>();
        dialogueBuilder = GetComponent<DialogueBuilder>();
        stageEnded += dialogueBuilder.HandleStageEnd;
        scenarioList = json.LoadScenario( scenarioText );

    }
	
	// Update is called once per frame
	void Update () {
        stageTime += Time.deltaTime;
        if ( Input.GetKeyDown( KeyCode.Space ) ) {
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
        if ( Stage >= currentScenario.Stages.Length ) {
            Debug.Log( "End scenario" );
            runningScenario = false;
            return;
        }
        bool good = false;
        for ( int i = 0; i < currentScenario.Stages.Length; i++ ) {

            if ( currentScenario.Stages[i].id == Stage ) {

            print( "Found stage " + Stage );
                currentStage = currentScenario.Stages[i];
                good = true;
            }
        }
        if ( !good ) {
            Debug.LogError( "Couldn't find stage ID:" + Stage.ToString() + " in " + currentScenario.ScenarioName );
        }
        stageTime = 0f;
        //currentStage = currentScenario.Stages[Stage];

        //Check completion status
        switch ( currentStage.completion ) {
            case "time":
                print( "Timed" );
                Invoke( "EndStage", currentStage.time );
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
    void EndStage() {
        Stage++;
        if ( stageEnded != null ) {
            stageEnded();
        }
        stageEnded = dialogueBuilder.HandleStageEnd;
        RunStage();
    }
    void EndStage(int nextStage) {
        Debug.Log( "Setting next stage to: " + nextStage );
        Stage = nextStage;
        if ( stageEnded != null ) {
            stageEnded();
        }
        stageEnded = dialogueBuilder.HandleStageEnd;
        RunStage();
    }
    void DoThings() {
        if ( currentStage.things != null ) {
            foreach ( Thing t in currentStage.things ) {
                switch ( t.type ) {
                    case "dialogue":
                        DialogueBox box = dialogueBuilder.Build( t );
                        box.SetText( t.text );
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
