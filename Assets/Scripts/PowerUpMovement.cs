using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    [SerializeField] int increaseHealth = 100;
    [SerializeField] float speed = 2f;
    [SerializeField] GameObject shieldPrefab = default;
    [SerializeField] float increaseFiringRateValue = 0.05f;
    PlayerPowerUps player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerPowerUps>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector2.down * speed * Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameObject.CompareTag("PowerUpLaser"))
            {
                player.powerUpCollected = true;
                player.powerUpLaserCount++;
                Destroy(gameObject);
            }

            else if (gameObject.CompareTag("PowerUpHeart"))
            {
                player.IncreaseHealth(increaseHealth);
                Destroy(gameObject);
            }

            else if (gameObject.CompareTag("PowerUpShield"))
            {
                if(FindObjectOfType<Player>().gameObject.transform.Find("Shield(Clone)"))
                {
                    Destroy(gameObject);
                    return;
                }
                else
                {
                    GameObject shield = Instantiate(shieldPrefab, collision.gameObject.transform.position, Quaternion.identity);
                    shield.transform.SetParent(collision.gameObject.transform);
                    Destroy(gameObject);
                }

            }

            else if (gameObject.CompareTag("PowerUpLaserRate"))
            {
                player.IncreaseFiringRate(increaseFiringRateValue);
                Destroy(gameObject);
            }
        }
    }
}
