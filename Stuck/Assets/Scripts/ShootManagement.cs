using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManagement : MonoBehaviour {
    private Vector3 velocity;
    private float timeLeft;
    public float direction; // positif pour droite negatif pour gauche 

    // Use this for initialization
    void Start ()
    {
        timeLeft = 2.0f;
        velocity = new Vector3(direction, 0.0f, 0.0f);
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position += velocity; // Mouvement pour le moment que vers la droite
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) // Après X seconde elle est détruite
        {
            Destroy(gameObject);
        }
        // Il faudrait faire en fonction de la distance ou faire disparaitre dès qu'elle sort de l'écran
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Object" || col.tag == "Player" || col.tag == "Solid")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Object" || collision.gameObject.tag == "Ennemy"){
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
