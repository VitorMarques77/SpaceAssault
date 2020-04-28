using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUps : MonoBehaviour
{

    [SerializeField] int health = 300;
    [SerializeField] float projectileFiringPeriod = 0.1f;
    public bool powerUpCollected = false;
    public int powerUpLaserCount = 0;

    int defHealth, defPowerUpLaserCount;
    float defProjectileFiringPeriod;
    bool defPowerUpCollected;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    private void Start()
    {
        defHealth = health;
        defProjectileFiringPeriod = projectileFiringPeriod;
        defPowerUpCollected = powerUpCollected;
        defPowerUpLaserCount = powerUpLaserCount;
    }

    // singleton pattern to make sure the score will go on through the next level
    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void DecreaseHealth(int health)
    {
        this.health -= health;
    }

    public void IncreaseHealth(int increase)
    {
        if (health < 300)
        {
            health += increase;
        }
    }

    public void IncreaseFiringRate(float increaseValue)
    {
        if (projectileFiringPeriod > 0.1f)
        {
            projectileFiringPeriod -= increaseValue;
        }

    }

    public float GetIncreaseFiringRate()
    {
        return projectileFiringPeriod;
    }

    public bool GetPowerUpCollected()
    {
        return powerUpCollected;
    }

    public void SetPowerUpCollected(bool powerUpCollected)
    {
        this.powerUpCollected = powerUpCollected;
    }

    public int GetPowerUpLaserCount()
    {
        return powerUpLaserCount;
    }

    public void SetPowerUpLaserCount(int powerUpLaserCount)
    {
        this.powerUpLaserCount = powerUpLaserCount;
    }

    public void ResetPowerUps()
    {
        Destroy(gameObject);
    }

    public void RestartPowerUps()
    {
        health = defHealth;
        projectileFiringPeriod = defProjectileFiringPeriod;
        powerUpLaserCount = defPowerUpLaserCount;
        powerUpCollected = defPowerUpCollected;
    }

}
