using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float xPadding = 1f;
    [SerializeField] float yPaddingBottom = 1f;
    [SerializeField] float yPaddingUpper = 1f;
    [SerializeField] AudioClip levelCompleteSound = default;

    [Header("Audio")]
    [SerializeField] AudioClip explosionAudio = default;
    [SerializeField] [Range(0, 1)] float explosionVolume = 0.8f;
    [SerializeField] AudioClip laserAudio = default;
    [SerializeField] [Range(0, 1)] float laserVolume = 0.5f;

    PlayerPowerUps playerPU;
    Lives lives;
    Coroutine firingCoroutine; // variable to store the coroutine and check if it's null or not

    [Header("Projectile")]
    [SerializeField] GameObject laserPrefab = default;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileMiddleOffset = 1f;
    [SerializeField] float projectileSideOffset = 0.5f;
    public bool powerUpCollected = false;
    public int powerUpLaserCount = 0;

    // boundaries
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    // Start is called before the first frame update
    void Start()
    {
        playerPU = FindObjectOfType<PlayerPowerUps>();
        lives = FindObjectOfType<Lives>();
        FindObjectOfType<LevelManager>().enabled = false;
        SetUpMoveBoundaries();
        PlayerPrefs.SetInt("LastScene", SceneManager.GetActiveScene().buildIndex);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();

        if (FindObjectOfType<EnemySpawner>().LevelComplete() && FindObjectsOfType<Enemy>().Length <= 0)
        {
            foreach (GameObject laser in GameObject.FindGameObjectsWithTag("EnemyLaser"))
            {
                Destroy(laser);
            }
            Camera.main.GetComponent<AudioSource>().mute = true;
            FindObjectOfType<LevelManager>().enabled = true;
            AudioSource.PlayClipAtPoint(levelCompleteSound, Camera.main.transform.position);
            StopAllCoroutines();
        }
    }

    public void IncreaseFiringRate(float increaseValue)
    {
        if (playerPU.GetIncreaseFiringRate() > 0.1f)
        {
            playerPU.IncreaseFiringRate(increaseValue);
        }

    }

    private void Shoot()
    {
        // if the player press space the ship will shoot
        if (Input.GetButtonDown("Fire1"))
        {
            // check if the coroutine it's different from null, if is then stop it
            if (firingCoroutine != null)
            {
                StopCoroutine(firingCoroutine);
                firingCoroutine = StartCoroutine(ShootContinuously());
            }
            // if its null then start the coroutine
            else if (firingCoroutine == null)
            {
                firingCoroutine = StartCoroutine(ShootContinuously());
            }

        }
        // if we release the space button stop the coroutine
        else if (Input.GetButtonUp("Fire1") && firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
        }
    }

    // coroutine responsible to if we keep spacebar pressed then the ship will
    // continuously firing
    IEnumerator ShootContinuously()
    {
        // put a while here just to make sure the coroutine will continuously happening
        while (true)
        {

            // instantites 3 differents shoots depending how much power up you collected
            if (!playerPU.GetPowerUpCollected())
            {
                GameObject laser = Instantiate(laserPrefab,
                    new Vector2(transform.position.x, transform.position.y + projectileMiddleOffset),
                    Quaternion.identity) as GameObject;
                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            }


            if (playerPU.GetPowerUpCollected() && playerPU.GetPowerUpLaserCount() == 1)
            {
                GameObject laser = Instantiate(laserPrefab,
                new Vector2(transform.position.x, transform.position.y + projectileMiddleOffset),
                Quaternion.identity) as GameObject;
                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

                GameObject laser1 = Instantiate(laserPrefab,
                    new Vector2(transform.GetChild(0).transform.position.x,
                    transform.GetChild(0).transform.position.y + projectileSideOffset),
                    Quaternion.identity) as GameObject;
                laser1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            }
            else if (playerPU.GetPowerUpCollected() && playerPU.GetPowerUpLaserCount() >= 2)
            {

                GameObject laser = Instantiate(laserPrefab,
                new Vector2(transform.position.x, transform.position.y + projectileMiddleOffset),
                Quaternion.identity) as GameObject;
                laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

                GameObject laser1 = Instantiate(laserPrefab,
                    new Vector2(transform.GetChild(0).transform.position.x,
                    transform.GetChild(0).transform.position.y + projectileSideOffset),
                    Quaternion.identity) as GameObject;
                laser1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

                GameObject laser2 = Instantiate(laserPrefab,
                    new Vector2(transform.GetChild(1).transform.position.x,
                    transform.GetChild(1).transform.position.y + projectileSideOffset),
                    Quaternion.identity) as GameObject;
                laser2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            }

            AudioSource.PlayClipAtPoint(laserAudio, Camera.main.transform.position, laserVolume);

            // here we set the time to wait after we finish execution the coroutine 
            // before starting again, to simplify it's define the fire rate of the ship
            yield return new WaitForSeconds(playerPU.GetIncreaseFiringRate());
        }
    }
    // the move method it's define by getting the input in the horizontal and vertical axis
    // multiply they by Time.deltaTime to make sure it's run the same on every computer
    // and multiply by speed to define a speed for the ship
    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        // Here we declare the new position of the player on the X axis bounding this with
        // a min and a max value for the X axis that the player can go
        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);

        // Here we declare the new position of the player on the Y axis bounding this with
        // a min and a max value for the Y axis that the player can go
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        // Here we define the new position of the player passing the XPos and the YPos as parameters
        transform.position = new Vector2(newXPos, newYPos);
    }

    // this method will make sure that the player don't go offscreen
    // will set the boundaries of the game
    private void SetUpMoveBoundaries()
    {
        // first we get the main camera of the game
        Camera gameCamera = Camera.main;

        // then we use the method ViewportToWorldPoint, this method will convert the
        // to the camera making the bottom-left of the viewport is (0,0) and 
        // the top-right is (1,1), after setting the minimum for the x Axis as the 0 we add some
        // padding 
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xPadding;

        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xPadding;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yPaddingBottom;

        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yPaddingUpper;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
       playerPU.DecreaseHealth(damageDealer.GetDamage());
        damageDealer.Hit();
        if (playerPU.GetHealth() <= 0)
        {
            Die();
            if (lives.GetLivesCount() >= 1)
            {
                lives.DecreaseLives();
                FindObjectOfType<SceneLoader>().LoadCurrentScene();
                playerPU.ResetPowerUps();

            }
            else if (lives.GetLivesCount() < 1)
            {
                FindObjectOfType<SceneLoader>().LoadGameOver();
            }

        }
    }

    private void Die()
    {
        Destroy(gameObject);
        AudioSource.PlayClipAtPoint(explosionAudio, Camera.main.transform.position, explosionVolume);
    }

    public void ShootMobile()
    {
        // check if the coroutine it's different from null, if is then stop it
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = StartCoroutine(ShootContinuously());
        }
        // if its null then start the coroutine
        else if (firingCoroutine == null)
        {
            firingCoroutine = StartCoroutine(ShootContinuously());
        }
    }

    public void StopShootMobile()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
        }

    }
}
