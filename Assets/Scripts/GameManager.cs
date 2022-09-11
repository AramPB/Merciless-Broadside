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

    public GameObject playerSpawnAera;

    [SerializeField]
    private Compass compass;

    private float changeWindStartTime;

    [SerializeField]
    private float changeWindDuration;

    public bool inPlacement;

    public enum GameState
    {
        Placement,
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
        UpdateGameState(GameState.Placement);
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
            case GameState.Placement:
                StartPlacementGameState();
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
    
    private void StartPlacementGameState()
    {
        playerSpawnAera.SetActive(true);
        inPlacement = true;
        SelectionManager.unitRTsList.Clear();
        SelectionManager.unitRTsEnemyList.Clear();
        UnitHandler.unitHandler.LoadUnits(playerUnits, playerSpawnAera.GetComponent<BoxCollider>().size, playerSpawnAera.GetComponent<BoxCollider>().center);
    }
    private void StartInGameGameState()
    {

        playerSpawnAera.SetActive(false);
        inPlacement = false;

        UnitHandler.unitHandler.UpdateStartStats(playerUnits);

        //UnitHandler.unitHandler.SetUnitStats(playerUnits);
        //UnitHandler.unitHandler.SetUnitStats(enemyUnits);


    }

    public Vector3 GetWind()
    {
        return compass.GetWindDir();
    }

    public void StartGame()
    {
        UpdateGameState(GameState.InGame);
    }

    public Vector3 GetNewPos()
    {
        return UnitHandler.unitHandler.GenerateRandomPosition(playerSpawnAera.GetComponent<BoxCollider>().size, playerSpawnAera.GetComponent<BoxCollider>().center, playerUnits);
    }
}