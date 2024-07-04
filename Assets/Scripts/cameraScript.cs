using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    public GameObject bird;
    private Vector3 offset;

    void Start()    {
        offset = transform.position - bird.transform.position;        
    }

    void LateUpdate()
    {
        transform.position = bird.transform.position + offset;
    }
}
