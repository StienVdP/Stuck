using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatformBehaviour : MonoBehaviour {
	// délais avant de tomber et de réapparaitre
	public float fallDelay;
	public float spawnDelay;

	// composants de la plateforme
	private Rigidbody2D rb2d;
	private BoxCollider2D boxCollider;

	// position initiale pour la réapparition
	private Vector3 initialPos;

	private bool isFalling;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.isKinematic = true;
		boxCollider = GetComponent<BoxCollider2D>();
		initialPos = transform.position;
		isFalling = false;
	}
	
	// Update is called once per frame
	void Update () {
		// pour éviter la plateforme qui glisse vers le bas
		if (!isFalling)
			transform.position = initialPos;
	}

	private void OnCollisionEnter2D(Collision2D col){
		// lors d'une collision avec le joueur, la plateforme tombera au bout d'un délai puis réapparaitra plus tard
		if (col.gameObject.tag == "Player"){
			StartCoroutine(fall());
			StartCoroutine(respawn());
		}
	}

	IEnumerator fall(){
		yield return new WaitForSeconds(fallDelay);
		isFalling = true;
		rb2d.isKinematic = false; // on soumet la plateforme à la gravité
		GetComponent<Collider2D>().isTrigger = true;
		yield return 0;
	}

	IEnumerator respawn()
    {
        yield return new WaitForSeconds(spawnDelay);
		isFalling = false;
		// on réinitialise toutes les caractéristiques
        rb2d.isKinematic = true;
        boxCollider.isTrigger = false;
        transform.position = initialPos;
        rb2d.velocity = Vector2.zero;
    }

}
