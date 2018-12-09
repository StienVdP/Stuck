using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;       //Public variable to store a reference to the player game object


    private Vector3 offset;         //Private variable to store the offset distance between the player and camera


    public float speed;
    private Vector3 cameraMove;



    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;


        cameraMove.x = transform.position.x;
        cameraMove.y = transform.position.y;
        cameraMove.z = transform.position.z;

        speed = 5f;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }

    void Update()
    {
        float bougeH = Input.GetAxis("Horizontal");
        float bougeV = Input.GetAxis("Vertical");

        if (bougeH > 0.0f)
        {
            Debug.Log("BougeH > 0");
            cameraMove.x += MoveSpeed();

        }
        if (bougeH < 0.0f)
        {
            Debug.Log("BougeH < 0");
            cameraMove.x -= MoveSpeed();

        }
        if (bougeV < 0.0f)
        {

            Debug.Log("BougeV < 0");
            cameraMove.x -= MoveSpeed();

        }
        if (bougeV > 0.0f)
        {
            Debug.Log("BougeV > 0");
            cameraMove.x += MoveSpeed();
            Debug.Log(MoveSpeed());

        }
        Debug.Log(cameraMove);
        transform.position = cameraMove;
    }

    float MoveSpeed()
    {
        return speed;
        //return speed * Time.deltaTime;
    }

}


