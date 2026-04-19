using UnityEngine;

public class Player : MonoBehaviour
{
    //Variables
    private bool gameStarted = false;

    private float vertical = 0;

    private bool detectedLastFrame = false;

    private Vector2 initialPosition = new Vector2(-4, -2.5f);

    private int fuelAmount = 100;
    
    [SerializeField] private Rigidbody2D rb;

    private float timeSinceFuelDrain = 0;

    //Constants
    private const int TO_NEWTONS = 100;

    private const float FORCE_VERTICAL = 9.8f;

    private const int FUEL_DRAIN_AMOUNT = 10;

    private const int FUEL_DRAIN_TIME = 5;

    void Start()
    {
        transform.position = initialPosition;
        
        gameObject.SetActive(false);

        Messenger<int>.Broadcast(GameEvent.FUEL_AMOUNT_CHANGED, fuelAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted){ return; }

        vertical = Input.GetAxisRaw("Vertical");

        if(transform.position.y <= -5 || transform.position.y >= 5 || fuelAmount == 0)
        {
            if(gameStarted){
                PlayerDeath();
            }
        }
        
        if (transform.position.y > 0.5)
        {
            if(!detectedLastFrame){
                Messenger.Broadcast(GameEvent.ABOVE_100_FT);
                
                detectedLastFrame = true;
            }
        }
        else
        {
            if (detectedLastFrame)
            {
                Messenger.Broadcast(GameEvent.BELOW_100_FT);
                
                detectedLastFrame = false;
            }
        }

        if (timeSinceFuelDrain >= FUEL_DRAIN_TIME)
        {
            fuelAmount -= FUEL_DRAIN_AMOUNT;

            Messenger<int>.Broadcast(GameEvent.FUEL_AMOUNT_CHANGED, fuelAmount);
            
            timeSinceFuelDrain = 0;
        }

        timeSinceFuelDrain += Time.deltaTime;
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector2 (0, vertical) * FORCE_VERTICAL * TO_NEWTONS * Time.deltaTime);   
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
        gameStarted = true;
        
        gameObject.SetActive(true);
    }

    private void PlayerDeath()
    {
        Messenger.Broadcast(GameEvent.PLAYER_DEAD);
        
        gameStarted = false;

        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
        
        if(collision.gameObject.tag == "GasCan")
        {
            addFuel();
        }
        if(collision.gameObject.tag == "Missile")
        {
            PlayerDeath();
        }
    }

    private void addFuel()
    {
        if (fuelAmount + 10 <= 100)
        {
            fuelAmount += 10;
        }
        else
        {
            fuelAmount = 100;
        }

        Messenger<int>.Broadcast(GameEvent.FUEL_AMOUNT_CHANGED, fuelAmount);
    }
}
