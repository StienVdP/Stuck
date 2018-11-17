using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidBehaviour : MonoBehaviour {

	private GameObject player;
	private PlayerPlatformerController playerScript;
    private GameObject gameManager;
    private GameManager gameManagerScript;
	// Use this for initialization
	void Awake () {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
		player = GameObject.Find("Player");
		playerScript = player.GetComponent<PlayerPlatformerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
			playerScript.gravityModifier = 0.2f;
        }

    }

}