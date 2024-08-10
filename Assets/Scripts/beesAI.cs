using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class beesAI : MonoBehaviour
{
    public float speed = 2.0f;
    public int maxCollisionsBeforeDeath = 2;
    public int damageDealt = 1;
    public float lifeLength = 60.0f;
    public float damageCooldown = 2.0f; // Cooldown time between damages

    private GameObject target;
    private float lastDamageTime = -Mathf.Infinity;

    // Start is called before the first frame update
    void Start()
    {
        // If we don't have a target, look for the player
        if (target == null)
        {
            target = GameObject.FindWithTag("Player");
        }

        // If we still don't have a target, look for a bird
        if (target == null)
        {
            target = GameObject.FindWithTag("Bird");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Reduce life by the amount of time that has passed
        lifeLength -= Time.deltaTime;

        // Did the bee run out of lifetime?
        if (lifeLength <= 0)
        {
            Die();
        }
        else
        {
            // Move towards the target
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Did we enter the trigger of the target?
        if (collider.gameObject == target && Time.time - lastDamageTime >= damageCooldown)
        {
            ApplyCollisionEffect();
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject == target && Time.time - lastDamageTime >= damageCooldown)
        {
            ApplyCollisionEffect();
        }
    }

    void ApplyCollisionEffect()
    {
        maxCollisionsBeforeDeath--;
        target.SendMessage("ApplyDamage", damageDealt, SendMessageOptions.DontRequireReceiver);
        lastDamageTime = Time.time; // Set the last damage time to now

        if (maxCollisionsBeforeDeath <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        Destroy(gameObject, 0.5f);
    }
}
