using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdAbilities : MonoBehaviour
{
    private Rigidbody2D mRigidbody;
    private Animator mAnimator;
    public float fallX; // X position for falling vertically
    public float fallY; // Y position for falling vertically
    public float fallDelay; // Delay for the flight/wait coroutine
    public float diveVel = 0; // Dive velocity for adjusting downward speed.
    public float velDrag; // Velocity drag for minimizing upward flight
    public bool flight = false; // Bool to check if flight was triggered


    public bool abilityActive = false;   

    // Set the rigidbody and animator
    void Start(){
        mRigidbody = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();        
    }

    void Update(){
        if(mRigidbody != null){
            // Press space bar to initiate dive
            if(Input.GetKey(KeyCode.Space) && flight == false){
                flight = true;
                SetActive();
                mAnimator.SetTrigger("StartDive");
                Diving();
                //mRigidbody.velocity = new Vector2(5,5);
                //StartCoroutine(WaitCoroutine(fallDelay));    
            }
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
            if(Input.GetKeyDown(KeyCode.DownArrow) && abilityActive == false){
                SetActive();
                mAnimator.SetTrigger("Loop");
                SetInactive();
            }
            if(Input.GetKeyDown(KeyCode.UpArrow) && abilityActive == false){
                SetActive();
                mAnimator.SetTrigger("Corkscrew");
                SetInactive();
            }
            if(Input.GetKeyDown(KeyCode.LeftArrow) && abilityActive == false){
                SetActive();
                mAnimator.SetTrigger("BankLeft");
                SetInactive();
            }
            if(Input.GetKeyDown(KeyCode.RightArrow) && abilityActive == false){
                SetActive();
                mAnimator.SetTrigger("BankRight");
                SetInactive();
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

    void Diving(){
        mAnimator.SetTrigger("Diving");
    }

    void SetActive(){
        abilityActive = true;
    }

    void SetInactive(){
        abilityActive = false;
    }

    // Wait function to delaying the dive after initial bunny hop
    IEnumerator WaitCoroutine(float delay){
        yield return new WaitForSeconds(delay);
        flight = true;  
    }
}
