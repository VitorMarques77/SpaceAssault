using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // script that hold the health of an enemy

    

    // variables to make the enemy shoot between a certain time
    [Header("Enemy Stats")]
    [SerializeField] float health = 500f;
    [SerializeField] int scoreValue = 58;
    [SerializeField] float[] powerUpSpawnRate = default;
    [SerializeField] GameObject[] powerUpPrefab = default;

    [Header("Enemy Shooting")]
    private float shootCounter = default;
    [SerializeField] float minTimeBetweenShoots = 0.2f;
    [SerializeField] float maxTimeBetweenShoots = 3f;
    [SerializeField] GameObject laserPrefab = default;
    [SerializeField] float projectileSpeed = 5f;
    
    


    [Header("Particles and Sounds")]
    [SerializeField] GameObject explosionParticles = default;
    [SerializeField] float durationOfExplosion = 1f;
    [SerializeField] AudioClip explosionAudio = default;
    [SerializeField] [Range(0, 1)] float explosionVolume = 1f;
    [SerializeField] AudioClip laserAudio = default;
    [SerializeField] [Range(0, 1)] float laserVolume = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        shootCounter = Random.Range(minTimeBetweenShoots, maxTimeBetweenShoots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        // decreasing the shoot counter based in the time that passed in frames
        shootCounter -= Time.deltaTime;
        // if the time reaches 0 then it shoots
        if(shootCounter <= 0f)
        {
            Shoot();
            AudioSource.PlayClipAtPoint(laserAudio, Camera.main.transform.position, laserVolume);
            // reset the shoot counter passing a new random value
            shootCounter = Random.Range(minTimeBetweenShoots, maxTimeBetweenShoots);
        }
    }

    private void Shoot()
    {
        // instantiate the prefab enemy laser 
        GameObject laser = Instantiate(laserPrefab,
                transform.position,
                Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
    }

    // when something hit the enemy we get the damage dealer component to know how much
    // damage that object will do
    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    // here we decrease the amount of damage from our enemy and if reaches 0 or below we destroy
    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddScore(scoreValue);
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionParticles, transform.position, transform.rotation);
        Destroy(explosion, durationOfExplosion);
        AudioSource.PlayClipAtPoint(explosionAudio, Camera.main.transform.position, explosionVolume);

        // Selecting a power up from a array for the enemy spawn
        int powerUp = Random.Range(0, powerUpPrefab.Length);

        // Storing in a variable a number between the powerUpSpawnRate and 1f
        float spawnPowerUp = (float) System.Math.Round(Random.Range(powerUpSpawnRate[powerUp], 1f), 1);

        // spawn power up at a percentage
        // if the random number we picked is equal to powerUpSpawnRate then we spawn the power up
        if (spawnPowerUp == powerUpSpawnRate[powerUp])
            {
                Instantiate(powerUpPrefab[powerUp], transform.position, Quaternion.identity);
            }       

    }
}
