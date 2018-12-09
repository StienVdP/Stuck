using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointControler : MonoBehaviour {

    private Vector3 cameraMove;
    public float speed;

    // Use this for initialization
    void Start () {
        cameraMove.x = transform.position.x;
        cameraMove.y = transform.position.y;
        cameraMove.z = transform.position.z;
    }

    void Update()
    {
        float bougeH = Input.GetAxis("Horizontal");
        float bougeV = Input.GetAxis("Vertical");

        if (bougeH > 0.0f)
        {
            cameraMove.x += MoveSpeed();

        }
        if (bougeH < 0.0f)
        {
            cameraMove.x -= MoveSpeed();

        }
        if (bougeV < 0.0f)
        {
            cameraMove.y += MoveSpeed();

        }
        if (bougeV > 0.0f)
        {
            cameraMove.y -= MoveSpeed();
            Debug.Log(MoveSpeed());

        }
        transform.position = cameraMove;
    }

    float MoveSpeed()
    {
        return speed * Time.deltaTime;
    }
}
