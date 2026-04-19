using UnityEngine;

public class Sky : MonoBehaviour
{
    //Variables
    private bool gameStarted = false;
    
    private Vector3 initialPosition = new Vector3(8.82f, 0.0f, 0f);

    //Constants
    private const int SPEED = 10;

    // Update is called once per frame
    void Update()
    {
        if(!gameStarted){return;}

        transform.Translate(Vector2.left * SPEED * Time.deltaTime);

        if (transform.position.x < -8.75)
        {
            transform.position = initialPosition;
        }
    }

    void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDeath);

        Messenger.AddListener(GameEvent.GAME_START, OnGameStarted);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDeath);

        Messenger.RemoveListener(GameEvent.GAME_START, OnGameStarted);
    }

    private void OnGameStarted()
    {
        gameStarted = true;
    }

    private void OnPlayerDeath()
    {
        gameStarted = false;
    }
}
