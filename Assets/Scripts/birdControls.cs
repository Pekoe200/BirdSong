using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class birdControls : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;
    public float fallX; // X position for falling vertically
    public float fallY; // Y position for falling vertically
    public float fallDelay; // Delay for the flight/wait coroutine
    public float diveVel = 0; // Dive velocity for adjusting downward speed.
    public float velDrag; // Velocity drag for minimizing upward flight
    public bool flight = false; // Bool to check if flight was triggered


    // Assigns the rigidbody for the bird so it can be controlled.
    void Start(){
        m_Rigidbody = GetComponent<Rigidbody2D>();
        
    }

    void Update(){
        // Press space bar to start bunny hop into dive
        if(Input.GetKey(KeyCode.Space) && flight == false){
            m_Rigidbody.velocity = new Vector2(5,5);
            StartCoroutine(WaitCoroutine(fallDelay));    
        }
        // Holding space keeps bird in a dive and calculates downward velocity
        if(Input.GetKey(KeyCode.Space) && flight == true){
            m_Rigidbody.velocity = new Vector2(fallX,fallY);
            diveVel++;   
        }
        // Releasing spacebar triggers flight func
        if(Input.GetKeyUp(KeyCode.Space) && flight == true){
            Flight(diveVel);
        }   
    }

    // Triggers upward velocity (swoop) after a dive
    void Flight(float velocity){
        m_Rigidbody.velocity = Vector2.up * (velocity / velDrag);

        // Reset dive velocity and flight check
        diveVel = 0;
        flight = false;
    }

    // Wait function to delaying the dive after initial bunny hop
    IEnumerator WaitCoroutine(float delay){
        yield return new WaitForSeconds(delay);
        flight = true;  
    }
}
