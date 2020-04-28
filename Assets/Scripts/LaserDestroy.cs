using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // create a trigger collider, when the laser hits it it's destroyed
        if (collision.gameObject.CompareTag("PlayerLaser") || collision.gameObject.CompareTag("EnemyLaser"))
        {
            Destroy(collision.gameObject);
        }
    }
}
