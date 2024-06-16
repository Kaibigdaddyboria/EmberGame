using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed;
    public Vector3 Offset;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the new position for the camera by adding the offset to the player's position
        Vector3 newPos = player.position + Offset;

        // Moves the camera from its current position to the new position
        transform.position = Vector3.Lerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}