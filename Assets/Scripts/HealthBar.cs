using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Sprite[] healthSprites = default;

    PlayerPowerUps player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerPowerUps>();
    }

    // Update is called once per frame
    void Update()
    {
        if(FindObjectOfType<PlayerPowerUps>() != null)
        {
            if (player.GetInstanceID() != FindObjectOfType<PlayerPowerUps>().GetInstanceID())
            {
                player = FindObjectOfType<PlayerPowerUps>();
            }
        }
        

        if (player.GetHealth() >= 300)
        {
            GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>().sprite = healthSprites[0];
        }
        else if (player.GetHealth() >= 200 && player.GetHealth() < 300)
        {
            GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>().sprite = healthSprites[1];
        }
        else if (player.GetHealth() >= 100 && player.GetHealth() < 200)
        {
            GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>().sprite = healthSprites[2];
        }
        else if (player.GetHealth() <= 0)
        {
            GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Image>().sprite = healthSprites[3];
        }
    }
}
