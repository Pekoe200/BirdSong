using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class birdControls : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;
    public float jumpSpeed = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space)){
            m_Rigidbody.velocity = Vector2.up * jumpSpeed;

        }
    }
}
