using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsInfo : MonoBehaviour {

    public string entre; // Son entré
    public string entreAccepter; // l'entre de la salle qui suit

    public int size; // 10, 20, ...
    public bool isDash;
    public bool isTP;
    public bool isWallJump;
    public bool isShoot;


    public GameObject[] prefabsEnnemis; // Tableau qui contient les ennemies en prefab
    
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}