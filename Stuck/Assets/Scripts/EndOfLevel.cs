using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfLevel : MonoBehaviour {

    public GameObject exitDoor;

    // Use this for initialization
    void Start () {
        Debug.Log("Start exit door");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            Debug.Log("OnCollisionEnter2D with : " + col.name);
            // Animation ouverture de porte
            // Changement de scène
        }
    }
}
