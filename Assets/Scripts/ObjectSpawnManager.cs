using UnityEngine;

public class ObjectSpawnManager : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject gasCan;

    [SerializeField] private GameObject missile;

    private float timeElapsedGasCan = 0;

    private float timeElapsedMissile = 0;

    private bool gameRunning = false;

    private bool radarDetection = false;

    //Constants
    private const int TIME_TO_GAS_SPAWN = 10;

    private const float TIME_TO_MISSILE_SPAWN = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if (!gameRunning){return;}
        
        if (timeElapsedGasCan >= TIME_TO_GAS_SPAWN)
        {   
            
            Instantiate(gasCan, new Vector2(9, Random.Range(-4, 4)), Quaternion.identity);
            
            timeElapsedGasCan = 0;
        }

        if(radarDetection)
        {
            timeElapsedMissile += Time.deltaTime;
        }
        else
        {
            timeElapsedMissile = 0;
        }

        if(radarDetection && timeElapsedMissile >= TIME_TO_MISSILE_SPAWN)
        {
            Instantiate(missile, new Vector2(9, Random.Range(-4, 4)), Quaternion.identity);

            timeElapsedMissile = 0;
        }

        timeElapsedGasCan += Time.deltaTime;


    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.GAME_START, OnGameStart);

        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDeath);

        Messenger.AddListener(GameEvent.ABOVE_100_FT, OnDetected);

        Messenger.AddListener(GameEvent.BELOW_100_FT, OnHidden);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_START, OnGameStart);

        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDeath); 

        Messenger.RemoveListener(GameEvent.ABOVE_100_FT, OnDetected);

        Messenger.RemoveListener(GameEvent.BELOW_100_FT, OnHidden);
    }

    private void OnGameStart()
    {
        gameRunning = true;
    }

    private void OnPlayerDeath()
    {
        gameRunning = false;
    }

    private void OnDetected()
    {
        radarDetection = true;
    }

    private void OnHidden()
    {
        radarDetection = false;
    }
}
