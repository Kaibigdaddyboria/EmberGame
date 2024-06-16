using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSensorScript : MonoBehaviour
{
    public bool InAttackRange = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // This happens when another collider enters the trigger collider attached to the object
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is tagged as "Player"
        if (other.gameObject.tag == "Player")
        {
            InAttackRange = true;
        }
    }

    // This happens when another collider exits the trigger collider attached to the object
    void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            InAttackRange = false;
        }
    }
}