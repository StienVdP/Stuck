using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManagement : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
        StartCoroutine("mouvement");
	}
	
	// Update is called once per frame
	void Update () {
		//Verifie s'il y a collision et quand ya collision on fait ce qu'il faut faire suivant l'objet collider7
        // Destroy quand il y a collision ou au bou de x seconde ou x distance
	}

    public IEnumerator creationOfEffectZone()
    {

        yield return new WaitForSeconds(1);
    }
}
