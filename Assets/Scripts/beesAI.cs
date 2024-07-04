using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beesAI : MonoBehaviour
{
    public float speed = 1.0f;
    public GameObject target;
    
    public int  maxCollisionsBeforeDeath = 2;
    public int damageDealt = 1;
    public float lifeLength = 10.0f;

  
    public float lastCollisionTime = 0.0f;
    public int collisionFrames = 0;
    private Rigidbody rigidBody;
    


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
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

    void OnTriggerStay(Collider collider)
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
