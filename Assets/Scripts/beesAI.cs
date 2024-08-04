using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beesAI : MonoBehaviour
{
    public float speed = 2.0f;
    
    
    public int  maxCollisionsBeforeDeath = 2;
    public int damageDealt = 1;
    public float lifeLength = 60.0f;

    private GameObject target;
    private float lastCollisionTime = 0.0f;
    private int collisionFrames = 0;
 
    


    // Start is called before the first frame update
    void Start()
    {
        
        //if we don't have a target look for the player
        if(target == null)
        {
            target = GameObject.FindWithTag("Player");
        }

        //if we still don't have a target look for a bird

        if(target == null)
        {
            target = GameObject.FindWithTag("Bird");
        }


    }

    // Update is called once per frame
    void Update()
    {

        

        //reduce life by amount time has passed
        lifeLength -= Time.deltaTime;

        //did the bird run out of lifetime?
        if(lifeLength <=0 )
        {
            Die();
        }
        else
        {
            
            //move towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            
           
        }

    
    }

    void OnCollisionStay2D(Collision2D collider)
    {
        //did we have a collision with the target?
        if(collider.gameObject == target)
        {
            //is this the first time we've collided or has it been at least 1 second since we last collided?
            if( (lastCollisionTime == 0.0) || ( (Time.time - lastCollisionTime) >=1.0 ))
            {
                maxCollisionsBeforeDeath--;
                target.SendMessage("ApplyDamage",damageDealt,SendMessageOptions.DontRequireReceiver);
                lastCollisionTime = Time.time;
            }

            if(maxCollisionsBeforeDeath <= 0)
            {
                Die();
            }
        }
        collisionFrames++;
    }

    void Die()
    {
        GetComponent<Renderer>().material.SetColor("_Color",Color.red);
        Destroy(gameObject,0.5f);

    }
}
