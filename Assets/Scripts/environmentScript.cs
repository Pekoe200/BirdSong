using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class environmentScript : MonoBehaviour
{
    public GameObject bird; // Used to set the bird object
    public birdControls birdControls; // Used to check current bird settings
    public float moveSpeed; // Speed to env movement
    public float speedMult; // Multiplier for when bird is diving
    public float spawnX; // Spawn locations for each axis
    public float spawnY;
    public float spawnZ;

    // Constantly moves object to the left.
    void Update(){
        transform.Translate(Vector2.left * moveSpeed);
    }

    // Looks for the bird object and gets settings 
    //Applies speed multiplier if bird is in a dive
    void FixedUpdate()
    {
        bird = GameObject.FindGameObjectWithTag("Bird");
        birdControls = bird.GetComponent<birdControls>();

        if(birdControls.flight == true){
            moveSpeed *= speedMult;
        }
        else{
            moveSpeed = .02f;
        }
    }

    // Destroys object when hitting the back wall
    public void OnTriggerEnter(Collider collision)
    {    
        if(collision.gameObject.CompareTag("Destroy"))
        {          
            Destroy(this.gameObject);
        }
    }
}
