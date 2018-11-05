using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManagement : MonoBehaviour {
    private Vector3 speed = new Vector3(0.1f, 0.0f, 0.0f);
    private float timeLeft;

    // Use this for initialization
    void Start ()
    {
        timeLeft = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position += speed; // Mouvement pour le moment que vers la droite
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) // Après X seconde elle est détruite
        {
            Destroy(gameObject);
        }
        // Il faudrait faire en fonction de la distance ou faire disparaitre dès qu'elle sort de l'écran
    }

    void OnTriggerEnter2D()
    {
        Destroy(gameObject);
    }
}
