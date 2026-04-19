using System.Collections;
using UnityEngine;

public class GasCan : MonoBehaviour
{
    //Constants
    private const int SPEED = 10;

    void Start()
    {
        StartCoroutine("selfDestruct");
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * SPEED * Time.deltaTime);
    }

    void Awake()
    {
        //Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDeath);
    }

    void OnDestroy()
    {
        //Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDeath);
    }
    
    private IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(5);

        Destroy(this.gameObject);
    }

//     private void OnPlayerDeath()
//     {
//         Destroy(this.gameObject);
//     }
}
