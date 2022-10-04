using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{//Es el Estado del juego
    menu,//Estado dentro del menu
    inGame,//Estado dentro de la partidad
    GameOver//estado de muerte

}

public class GameManager : MonoBehaviour{


    //Es el estado actual con el que comenzará el juego...
    //Comenzamos con el estado MENUU
    public GameState currentGameState = GameState.menu;

    //Es un singleton
    public static GameManager sharedInstance;

    private PlayerController controller;

    private void Awake(){
        if(sharedInstance == null) { 
            sharedInstance = this;
            
        }
    }



    // Start is called before the first frame update
    void Start(){
        controller = GameObject.Find("Player").GetComponent<PlayerController>();   
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetButtonDown("Submit")
            && currentGameState != GameState.inGame){
            StartGame();
        }else if(Input.GetKeyDown(KeyCode.P))
        {
            BackToMenu();
        }
    }

    public void StartGame(){//Inicia el juego
        SetGameState(GameState.inGame);
    }
    public void GameOver() { //Finaliza el juego
        SetGameState(GameState.GameOver);
    }

    public void BackToMenu(){//Coloca el menú
        SetGameState(GameState.menu);
    }

    private void SetGameState(GameState newGameState) { 
        if(newGameState == GameState.menu){
            //TODO: colocar la lógica del menú
        }
        else if(newGameState == GameState.inGame){
            //TODO: hay que preparar la escena para jugar
            controller.StartGame();
        }else if(newGameState == GameState.GameOver){
            //TODO: preparar el juego para el Game Over

        }

        this.currentGameState = newGameState;
    }

}
