using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foregroundScript : MonoBehaviour
{
    Rigidbody m_Rigidbody;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
         m_Rigidbody = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Rigidbody.velocity = transform.right * moveSpeed;
        
    }
}
