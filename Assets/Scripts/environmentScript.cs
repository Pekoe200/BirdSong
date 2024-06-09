using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class environmentScript : MonoBehaviour
{
    public GameObject prefab;
    public GameObject spawnLoc;
    public float moveSpeed;
    public float spawnX;
    public float spawnY;
    public float spawnZ;

    // Start is called before the first frame update
    void Start()
    {
        //spawnY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed);
    }

    public void OnTriggerEnter(Collider collision)
    {    
        if(collision.gameObject.CompareTag("Destroy"))
        {
            Instantiate(prefab, new Vector3(spawnX,Random.Range(spawnY - 2, spawnY + 2),spawnZ), transform.rotation);
            
            Destroy(this.gameObject);
        }
    }
}
