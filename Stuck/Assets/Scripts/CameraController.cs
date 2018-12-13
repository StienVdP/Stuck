using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player; //Référence du Player GameObject


    private Vector3 offset; // Distance entre le Player et la camera
    
    // Use this for initialization
    void Start()
    {
        //Calcul la valeur du offset à partir de la distance entre la position du Player et de la camera
        offset = transform.position - player.transform.position;
        
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Applique la nouvelle position de la camera
        transform.position = player.transform.position + offset;
    }
    
}


