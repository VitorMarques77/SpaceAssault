using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lives : MonoBehaviour
{
    [SerializeField] int livesCount = 3;
    [SerializeField] Sprite[] livesCountSprites = default;

    Sprite livesChangeSprite;


    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Sprite lives in livesCountSprites)
        {
            GameObject.FindGameObjectWithTag("Lives").GetComponent<Image>().sprite = livesCountSprites[livesCount];
        }
    }

    public int GetLivesCount()
    {
        return livesCount;
    }

    public void DecreaseLives()
    {
        livesCount--;
    }
}
