using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensorScript : MonoBehaviour
{
    public bool InRange = false;
    public float playerx = 0;

    // Check if the object that entered the trigger is tagged as Player and sets InRange to true
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            InRange = true;
        }
    }
    // Check if the object that entered the trigger is tagged as Player and gets the x position of the player
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerx = collision.transform.position.x;
        }
    }
    // Check if the object that entered the trigger is tagged as Player and sets InRange to false
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            InRange = false;
        }
    }
}
