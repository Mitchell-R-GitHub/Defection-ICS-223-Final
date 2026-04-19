using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GameplayUI : UIItem
{
    //Variables
    [SerializeField] private TextMeshProUGUI score;

    [SerializeField] private TextMeshProUGUI missileAlert;

    [SerializeField] private TextMeshProUGUI gameOver;

    [SerializeField] private GameObject radarWarningText;

    [SerializeField] private RawImage radarWarningLine;

    [SerializeField] private AudioSource audio;

    [SerializeField] private AudioClip missileWarningSound;

    [SerializeField] private AudioClip playerDeathSound;

    private bool detected = false;

    private float timeSinceLastWarning = 0;

    [SerializeField] private TextMeshProUGUI fuelText;

    [SerializeField] private Image fuelGuage;

    //Constants
    private const int WARNING_REPEAT_TIME = 2;

    void Start()
    {
        missileAlert.gameObject.SetActive(false);
        
        score.gameObject.SetActive(true);

        radarWarningText.SetActive(true);

        gameOver.gameObject.SetActive(false);

        radarWarningLine.gameObject.SetActive(true);

        fuelText.gameObject.SetActive(true);

        fuelGuage.gameObject.SetActive(true);
    }

    void Update()
    {
        if (detected && !gameOver.IsActive())
        {
            missileAlert.gameObject.SetActive(true);

            if(timeSinceLastWarning >= WARNING_REPEAT_TIME)
            {
                audio.PlayOneShot(missileWarningSound);
                
                timeSinceLastWarning = 0;
            }

            timeSinceLastWarning += Time.deltaTime;
        }
        else
        {
            missileAlert.gameObject.SetActive(false);
            
            timeSinceLastWarning = 0;
        }
    }

    void Awake()
    {
        Messenger<int>.AddListener(GameEvent.SCORE_CHANGE, OnScoreChange);

        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDeath);

        Messenger.AddListener(GameEvent.ABOVE_100_FT, OnDetected);

        Messenger.AddListener(GameEvent.BELOW_100_FT, OnHidden);

        Messenger<int>.AddListener(GameEvent.FUEL_AMOUNT_CHANGED, OnFuelAmountChanged);
    }

    void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.SCORE_CHANGE, OnScoreChange);

        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDeath);

        Messenger.RemoveListener(GameEvent.ABOVE_100_FT, OnDetected);

        Messenger.RemoveListener(GameEvent.BELOW_100_FT, OnHidden);

        Messenger<int>.RemoveListener(GameEvent.FUEL_AMOUNT_CHANGED, OnFuelAmountChanged);
    }

    private void OnScoreChange(int newScore)
    {
        SetScore(newScore);
    }

    private void SetScore(int newScore)
    {
         score.text = "Score: " + newScore.ToString();
    }

    private void OnPlayerDeath()
    {
        missileAlert.gameObject.SetActive(false);
        
        score.gameObject.SetActive(false);

        radarWarningText.SetActive(false);

        gameOver.gameObject.SetActive(true);

        radarWarningLine.gameObject.SetActive(false);

        fuelText.gameObject.SetActive(false);

        fuelGuage.gameObject.SetActive(false);

        audio.PlayOneShot(playerDeathSound);
    }

    private void OnDetected()
    {
        detected = true;
    }

    private void OnHidden()
    {
        detected = false;
    }

    private void OnFuelAmountChanged(int newFuelAmount)
    {
        float fuelAmountRepresentation = newFuelAmount / 100f;
        
        fuelGuage.fillAmount = fuelAmountRepresentation;

        fuelGuage.color = Color.Lerp(Color.red, Color.green, fuelAmountRepresentation);
    }
}
