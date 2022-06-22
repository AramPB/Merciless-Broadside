using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[DefaultExecutionOrder(1)]
public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    public Transform 
        playerUnits,
        enemyUnits;

    [SerializeField]
    private Compass compass;

    private float changeWindStartTime;

    [SerializeField]
    private float changeWindDuration;

    public enum GameState
    {
        Menu,
        InGame,
        Victory,
        Lose
    }

    public GameState State;

    //public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //TMP
        UpdateGameState(GameState.InGame);
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Aqui hauria de controlar en game quan canviar el vent amb changeWindStartTime changeWindDuration i compass pasarli
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        
        //funcions en cada un
        switch (newState)
        {
            case GameState.Menu:
                StartMenuGameState();
                break;
            case GameState.InGame:
                StartInGameGameState();
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
        }

        //OnGameStateChanged?.Invoke(newState);
    }
    
    private void StartMenuGameState()
    {


    }
    private void StartInGameGameState()
    {
        UnitHandler.unitHandler.SetUnitStats(playerUnits);
        UnitHandler.unitHandler.SetUnitStats(enemyUnits);
    }

    public Vector3 GetWind()
    {
        return compass.GetWindDir();
    }
}