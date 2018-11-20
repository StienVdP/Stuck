using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsInfo : MonoBehaviour {

    public string entre; // Son entré
    public string entreAccepter; // l'entre de la salle qui suit

	// Use this for initialization
	void Start () {
        Debug.Log("Entre : " + entre + " et l entre de la salle qui suit : " + entreAccepter);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}