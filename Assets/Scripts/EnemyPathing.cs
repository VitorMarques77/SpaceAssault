using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{

    WaveConfig waveConfig;
    // a list of all the waypoints the enemy will pass through
    List<Transform> waypoints;
    
    int waypointIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        waypoints = waveConfig.GetWaypoints();
        // set the enemy position to the first waypoint of our list
        transform.position = waypoints[waypointIndex].transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    // here we set wich wave config we should use in the script
    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    // the path that the enemy will follow through the waypoints
    private void Move()
    {
        // here we check if our waypointIndex doesn't is larger than our list lenght
        // we put minus 1 to make sure it's relative to the index not the amount of elements
        if (waypointIndex <= waypoints.Count - 1)
        {
            // we are going to use Vector2.MoveTowards so we need the position that the enemy
            // will go, this is the next waypoint position, so we grab that from our list
            var targetPosition = waypoints[waypointIndex].transform.position;

            // next we need the speed that the enemy will move from one waypoint to another
            // also multipling by Time.deltaTime
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;

            // then we get the enemy position and define as the method Vector2.MoveTowards
            // passing the 2 variables we created above
            transform.position = Vector2.MoveTowards
                (transform.position, targetPosition, movementThisFrame);

            // here we check if the enemy reach the waypoint, if it's true then we increment
            // to go to the next waypoint
            if (transform.position == targetPosition)
            {
                waypointIndex++;
            }
        }

        // at the end of the list we reached to the last waypoint so we destroy our enemy
        else
        {
            Destroy(gameObject);
        }
    }
}
