using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameplayUI gamePlayUI;

    [SerializeField] private GameStartUI gameStartUI;

    void Start()
    {
        gameStartUI.Show();
        
        gamePlayUI.Hide();
    }
    
    void Awake()
    {
        Messenger.AddListener(GameEvent.GAME_START, OnGameStarted);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.GAME_START, OnGameStarted);
    }

    private void OnGameStarted()
    {
        gameStartUI.Hide();

        gamePlayUI.Show();
    }

    private void OnReset()
    {
        gameStartUI.Show();
        
        gamePlayUI.Hide();
    }
}
