using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // speed the background will scroll
    [SerializeField] float backgroundScrollSpeed = 0.5f;

    // variable to be able to acess the material of the background
    Material myMaterial;

    // the vector we will pass in the offset of the material
    Vector2 offSet;


    // Start is called before the first frame update
    void Start()
    {
        // get the material from the renderer
        myMaterial = GetComponent<Renderer>().material;
        // initialize the offset with the backgroundScrollSpeed as the Y
        offSet = new Vector2(0, backgroundScrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        // updating the offset of the material with the offset we created
        // multiply by Time.deltaTime because it's a movement
        myMaterial.mainTextureOffset += offSet * Time.deltaTime;   
    }
}
