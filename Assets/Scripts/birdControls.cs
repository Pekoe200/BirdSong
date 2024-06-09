using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdControls : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public float moveSpeed;
    public float jumpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Rigidbody.velocity = transform.right * moveSpeed;

        if(Input.GetKey(KeyCode.Space)){
            m_Rigidbody.AddForce(Vector2.up * jumpSpeed);

        }
    }
}
