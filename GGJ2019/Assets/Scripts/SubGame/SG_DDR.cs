using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DDR_CMD {
    NONE = 0,
    A = 1,
    B =2,
    UP=3,
    DOWN=4,
    LEFT=5,
    RIGHT=6,
    C_UP=7,
    C_DOWN=8,
    C_LEFT=9,
    C_RIGHT=10
}

public class Note {
    public SpriteRenderer spriteRenderer;
    public int CorrectTime = -1;
    public DDR_CMD cmd;

    public Note( SpriteRenderer spr, DDR_CMD CMD, int t ) {
        spriteRenderer = spr;
        CorrectTime = t;
        cmd = CMD;
    }
}

public class SG_DDR : SubGame {
    public Sprite[] spriteList;
    private DDR_CMD[] currentTrack;
    private List<Note> noteBuffer = new List<Note>();

    private enum DDR_GAMESTATE {
        PREGAME,
        PLAY
    }
    private DDR_GAMESTATE gamestate;
    [SerializeField]
    private int gameTime = 0;

    //Settings
    private int introFrameCount = 45;

    private int exitFrameCount = 15;

    private int successRange = 5;

    protected override void OnStartGame() {
        base.OnStartGame();

        currentTrack = new DDR_CMD[1800];
        for ( int i = 0; i < 1800; i++ ) {
            currentTrack[i] = Random.Range(0,20) > 18 ? ( DDR_CMD )Random.Range( 1, 11 ) : DDR_CMD.NONE;
        }
        ChangeGameState( DDR_GAMESTATE.PLAY );
    }

    protected override void OnUpdate() {
        switch ( gamestate ) {
            case DDR_GAMESTATE.PLAY:
                GetInput();
                PlayStateUpdate();
                break;
        }
    }
    void GetInput() {
        if ( Input.GetButtonDown( "A" ) )
            CheckInput( DDR_CMD.A );
        if ( Input.GetButtonDown( "B" ) )
            CheckInput( DDR_CMD.B );
        if ( HandManager.GetStick( Direction.UP ) )
            CheckInput( DDR_CMD.UP );
        if ( HandManager.GetStick( Direction.DOWN ) )
            CheckInput( DDR_CMD.DOWN );
        if ( HandManager.GetStick( Direction.LEFT ) )
            CheckInput( DDR_CMD.LEFT );
        if ( HandManager.GetStick( Direction.RIGHT ) )
            CheckInput( DDR_CMD.RIGHT );
        if ( Input.GetButtonDown( "C_UP" ) )
            CheckInput( DDR_CMD.C_UP );
        if ( Input.GetButtonDown( "C_DOWN" ) )
            CheckInput( DDR_CMD.C_DOWN );
        if ( Input.GetButtonDown( "C_LEFT" ) )
            CheckInput( DDR_CMD.C_LEFT );
        if ( Input.GetButtonDown( "C_RIGHT" ) )
            CheckInput( DDR_CMD.C_RIGHT );
    }
    void CheckInput( DDR_CMD cmd ) {
        Note bestNote = null;
        int bestRange = 100;
        for ( int i = 0; i < noteBuffer.Count; i++ ) {
            if ( cmd == noteBuffer[i].cmd ) {
                int dist = gameTime - noteBuffer[i].CorrectTime;
                if ( Mathf.Abs(dist) < successRange ) {
                    bestNote = noteBuffer[i];
                    bestRange = dist;
                    break;
                }
            }
        }
        if ( bestNote != null ) {
            Debug.Log( "Sweet!" );

            noteBuffer.Remove( bestNote );
            Destroy( bestNote.spriteRenderer.gameObject );
        }
    }

    void PlayStateUpdate() {
        int front = gameTime + introFrameCount;
        int back = gameTime - exitFrameCount;
        if ( front < currentTrack.Length ) {
            if ( currentTrack[front] != DDR_CMD.NONE ) {
                        noteBuffer.Add( new Note( CreateNote( currentTrack[front] ), currentTrack[front], front));
            }
        }
        if ( back >= 0 ) {
            List<Note> removalList = new List<Note>();
            for ( int i = 0; i < noteBuffer.Count; i++ ) {
                if ( gameTime > noteBuffer[i].CorrectTime + exitFrameCount ) {
                    Debug.Log( "Fail" );
                    removalList.Add(noteBuffer[i]);
                }
            }
            foreach ( Note n in  removalList) {

                noteBuffer.Remove( n );
                Destroy( n.spriteRenderer.gameObject );
            }
            removalList.Clear();
        }

        RectTransform rt;
        for ( int i = 0; i < noteBuffer.Count; i++ ) {
            rt = noteBuffer[i].spriteRenderer.GetComponent<RectTransform>();
            Vector2 pos = rt.anchoredPosition;
            pos.x = Mathf.Lerp( -1f, 1f, ( float )noteBuffer[i].cmd / 10.0f );
            pos.y = Mathf.Lerp( -1.4f, 1.4f, Mathf.InverseLerp( noteBuffer[i].CorrectTime - introFrameCount, noteBuffer[i].CorrectTime + exitFrameCount, gameTime ) );
            rt.anchoredPosition = pos;
        }

        gameTime++;
    }

    SpriteRenderer CreateNote(DDR_CMD cmd) {
        GameObject go = new GameObject( "Note_" + cmd.ToString(), typeof(SpriteRenderer), typeof(RectTransform) );
      //  Debug.Log( "MAKE: " + ( int )cmd );
        go.GetComponent<SpriteRenderer>().sprite = spriteList[( int )cmd - 1];
        go.transform.SetParent( SGM.screen, false );
        go.layer = 9;

        return go.GetComponent<SpriteRenderer>();
    }

    private void ChangeGameState( DDR_GAMESTATE nextState ) {
        //Exit
        switch ( gamestate ) {
            case DDR_GAMESTATE.PLAY:

                break;
        }
        gamestate = nextState;
        //Enter
        switch ( gamestate ) {
            case DDR_GAMESTATE.PLAY:
                Debug.Log( "Begin play" );
                gameTime = -introFrameCount;
                break;
        }
    }
}
