using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shieldBehaviour : MonoBehaviour {
	public GameObject player;
    private GameObject gameManager;
    private GameManager gameManagerScript;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();

		gameObject.GetComponent<Collider2D>().enabled = false;
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManagerScript.isProtectOn())
			transform.position = player.transform.position + new Vector3(-0.5f * player.transform.localScale.x, 1.4f, 0);
	}
}
