using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Variables
    private int score = 0;

    private bool gameStarted = false;

    private float timeElapsed = 0;

    //Constants
    private const int TIME_TILL_RESET = 5;
    
    // Update is called once per frame
    void Update()
    {
        if(!gameStarted){ return; }
        
        timeElapsed += Time.deltaTime;

        if(timeElapsed > 0.5)
        {
            score += 1;

            Messenger<int>.Broadcast(GameEvent.SCORE_CHANGE, score);

            timeElapsed = 0;
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

    private void OnPlayerDeath()
    {
        StartCoroutine(PlayerDeath());
    }

    private IEnumerator PlayerDeath()
    {
        gameStarted = false;
        
        score = 0;
        
        yield return new WaitForSeconds(TIME_TILL_RESET);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnGameStarted()
    {
        gameStarted = true;
    }
}
