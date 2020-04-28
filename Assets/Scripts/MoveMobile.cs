using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMobile : MonoBehaviour
{

    private float deltaX, deltaY;
    private Rigidbody2D rb;
    private int currentFingerID;

    Player player;

    // Update is called once per frame
    private void Start()
    {
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if(Input.touchCount > 0)
        {

            Touch touch = Input.GetTouch(0);

            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);


            switch (touch.phase)
            {
                case TouchPhase.Began:

                    currentFingerID = touch.fingerId;
                    player.StopShootMobile();
                    deltaX = touchPos.x - transform.position.x;
                    deltaY = touchPos.y - transform.position.y;
                    player.ShootMobile();
                    break;

                case TouchPhase.Moved:

                    if(currentFingerID == touch.fingerId)
                    {
                        rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                        break;
                    }
                    else
                    {
                        break;
                    }

                case TouchPhase.Ended:
                    rb.velocity = Vector2.zero;
                    player.StopShootMobile();
                    break;
            }
        }
    }

    
}
