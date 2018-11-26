using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsInfo : MonoBehaviour {

    public string entre; // Son entré
    public string entreAccepter; // l'entre de la salle qui suit

    public GameObject[] prefabsEnnemis; // Tableau qui contient les ennemies en prefab

    //public Vector2[] tabDoors; // tableau de portes // Gauche // Droite // Haut // Bas

	// Use this for initialization
	void Start () {
        GameObject go = GameObject.Find("SpawnZone");

        Vector3 position = go.GetComponent<Transform>().position;
        //Instantiate(prefabsEnnemis[0], position, Quaternion.identity);
              

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}