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
        enemyUnits,
        playerUI;

    public GameObject playerPanelUI;

    public GameObject playerSpawnAera;
    public GameObject enemySpawnAera;
    public GameObject mapArea;

    [SerializeField]
    private Compass compass;

    private float changeWindStartTime;

    [SerializeField]
    private float changeWindDuration;

    public bool inPlacement;

    //[SerializeField]
    //private float cannonPower, maxAngle;

    public enum GameState
    {
        Placement,
        InGame,
        Victory,
        Lose
    }

    public GameState State;

    public PauseMenuController pauseMenu;


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
        //Debug.Log(MathParabola.MaxDistance(cannonPower, maxAngle));
        //TODO: Aqui hauria de controlar en game quan canviar el vent amb changeWindStartTime changeWindDuration i compass pasarli
        if (State == GameState.InGame)
        {
            if (SelectionManager.unitRTsEnemyList.Count == 0)
            {
                pauseMenu.Victory();
                Debug.Log("WINN!!!");
            }
            if (SelectionManager.unitRTsList.Count == 0)
            {
                pauseMenu.Defeat();
                Debug.Log("LOSEE!!!");
            }
        }
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
        enemySpawnAera.SetActive(true);
        inPlacement = true;
        SelectionManager.unitRTsList.Clear();
        SelectionManager.unitRTsEnemyList.Clear();
        UnitHandler.unitHandler.LoadUnits(playerUnits, playerSpawnAera.GetComponent<BoxCollider>().size, playerSpawnAera.GetComponent<BoxCollider>().center, playerUI);
        UnitHandler.unitHandler.LoadEnemies(enemyUnits, enemySpawnAera.GetComponent<BoxCollider>().size, enemySpawnAera.GetComponent<BoxCollider>().center);
        UnitHandler.unitHandler.UpdateUnitUI(playerUnits);
    }
    private void StartInGameGameState()
    {

        playerSpawnAera.SetActive(false);
        enemySpawnAera.SetActive(false);
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

    //private void OnDrawGizmos()
    //{
        //Gizmos.DrawWireSphere(Vector3.zero, MathParabola.MaxDistance(cannonPower, maxAngle));
    //}
}