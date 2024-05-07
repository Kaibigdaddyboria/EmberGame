using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximitySensorScript : MonoBehaviour
{
    public bool InRange = false;
    public float playerx = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            InRange = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerx = collision.transform.position.x;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            InRange = false;
        }
    }
}
