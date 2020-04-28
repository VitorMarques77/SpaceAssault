using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // moving the spaceship in the middle of the level
    [SerializeField] float secondsToStart = 3f;
    [SerializeField] float speed = 5f;
    [SerializeField] float positionToStartY = -6f;
    [SerializeField] float positionToEndY = 11.5f;
    [SerializeField] Coroutine coroutine = default;
    [SerializeField] AudioClip shipEnterScene = default;
    [SerializeField] float speedEndLevel = 10f;

    //Animation for start the level
    [SerializeField] TextMeshProUGUI startLevel = default;

    //cached references
    Animator myAnim = default;
    Player player;
    EnemySpawner enemySpawner;
    MoveMobile moveMobile;

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        moveMobile = FindObjectOfType<MoveMobile>();
        myAnim = startLevel.GetComponent<Animator>();

        // disabling the player and the enemy so it's not possible to move or the enemies to be
        // spawned
        player.enabled = false;
        enemySpawner.enabled = false;
        moveMobile.enabled = false;
        AudioSource.PlayClipAtPoint(shipEnterScene, Camera.main.transform.position);

    }

    // Update is called once per frame
    void Update()
    {

        if (enemySpawner.LevelComplete())
        {
            coroutine = StartCoroutine(PlayerMovingOutTheScene());
        }
        else
        {
            coroutine = StartCoroutine(PlayerMovingIntoTheScene());
        }

    }

    // coroutine responsible to move the player in the middle of the scene
    IEnumerator PlayerMovingIntoTheScene()
    {

        if (player.transform.position.y < positionToStartY)
        {
            player.transform.Translate(Vector2.up * speed * Time.deltaTime);

        }
        yield return new WaitForSeconds(secondsToStart);

        // stoping the animation and enabling the player and the enemy spawner

        myAnim.SetBool("EndAnim", true);

        if (player != null && moveMobile != null && enemySpawner != null)
        {
            player.enabled = true;
            enemySpawner.enabled = true;
            moveMobile.enabled = true;
        }
    }

    IEnumerator PlayerMovingOutTheScene()
    {
        startLevel.text = "Level Complete!";
        myAnim.SetBool("EndAnim", false);
        player.enabled = false;
        enemySpawner.enabled = false;

        yield return new WaitForSeconds(secondsToStart);

        if (player.transform.position.y < positionToEndY)
        {
            player.transform.Translate(Vector2.up * speedEndLevel * Time.deltaTime);
        }
        else
        {
            FindObjectOfType<SceneLoader>().LoadWinScreen();
        }

    }

}
