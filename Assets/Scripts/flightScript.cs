using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.Rendering;
using UnityEngine;

public class flightScript : MonoBehaviour
{
    private Rigidbody mRigidbody;
    public birdScript bScript;
    public float defaultX;
    public float defaultY;    
    public float xSpeed;
    public float ySpeed;
    public float diveX;
    public float diveY;
    public float diveDelay;
    public float diveVelocity;
    public int action;
    public bool isDiving = false;
    public bool diveComplete = false;

    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {       
        mRigidbody.velocity = new Vector2(xSpeed, ySpeed);

        if(mRigidbody != null){
            if(Input.GetKey(KeyCode.Space) && isDiving == false){
                bScript.StartDive();
                ySpeed += 10;
                isDiving = true;
                action = 1;
                StartCoroutine(WaitCoroutine(action));               
            }
            if(Input.GetKey(KeyCode.Space) && isDiving == true){
                diveVelocity++;   
            }
            if(Input.GetKeyUp(KeyCode.Space) && isDiving == true){
                bScript.Swoop();
                ySpeed -= 2;
                isDiving = false;
                diveComplete = true;            
                action = 2;
                StartCoroutine(WaitCoroutine(action));   
            }
        }        
    }

    void Diving(){   
        bScript.Diving();
        ySpeed -= 10;     
        xSpeed -= diveX;
        ySpeed -= diveY;
    }

    void Rising(){
        xSpeed += diveX * 2;
        ySpeed += diveVelocity / 20;
        action = 3;
        StartCoroutine(WaitCoroutine(action));  
    }

    void EndRise(){
        bScript.EndSwoop();
        IdleFlight();
    }

    void IdleFlight(){
        bScript.EndSwoop();
        bScript.EndFlight();
        xSpeed = defaultX;
        ySpeed = defaultY;        

        isDiving = false;
        diveComplete = false;
        diveVelocity = 0;
    }

    IEnumerator WaitCoroutine(int action){  
            
        if(action == 1){
            yield return new WaitForSeconds(diveDelay); 
            Diving();
        }
        if(action == 2){
            yield return new WaitForSeconds(diveDelay); 
            Rising();
        }
        if(action == 3){
            
            yield return new WaitForSeconds(diveVelocity/600); 
            EndRise();
        }
    }
}
