using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdAbilities : MonoBehaviour
{
    Rigidbody2D mRigidbody;
    public float fallX; // X position for falling vertically
    public float fallY; // Y position for falling vertically
    public float fallDelay; // Delay for the flight/wait coroutine
    public float diveVel = 0; // Dive velocity for adjusting downward speed.
    public float velDrag; // Velocity drag for minimizing upward flight
    public bool flight = false; // Bool to check if flight was triggered


    public bool abilityActive = false;
    private Animator mAnimator;


    // Assigns the rigidbody for the bird so it can be controlled.
    void Start(){
        mRigidbody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        
    }

    void Update(){
        // Press space bar to start bunny hop into dive
        if(Input.GetKey(KeyCode.Space) && flight == false){
            mRigidbody.velocity = new Vector2(5,5);
            StartCoroutine(WaitCoroutine(fallDelay));    
        }
        // Holding space keeps bird in a dive and calculates downward velocity
        if(Input.GetKey(KeyCode.Space) && flight == true){
            mRigidbody.velocity = new Vector2(fallX,fallY);
            diveVel++;   
        }
        // Releasing spacebar triggers flight func
        if(Input.GetKeyUp(KeyCode.Space) && flight == true){
            Flight(diveVel);
        }
        if(mAnimator != null){
            if(Input.GetKeyDown(KeyCode.B) && abilityActive == false){
                setActive();
                mAnimator.SetTrigger("Corkscrew");
                setInactive();
            }
        }           
    }

    // Triggers upward velocity (swoop) after a dive
    void Flight(float velocity){
        mRigidbody.velocity = Vector2.up * (velocity / velDrag);

        // Reset dive velocity and flight check
        diveVel = 0;
        flight = false;
    }

    void setActive(){
        abilityActive = true;
    }

    void setInactive(){
        abilityActive = false;
    }

    // Wait function to delaying the dive after initial bunny hop
    IEnumerator WaitCoroutine(float delay){
        yield return new WaitForSeconds(delay);
        flight = true;  
    }
}
