﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour {

	public Button load;
	public Button menu;
    private GameObject gameManager;
    private GameManager gameManagerScript;
	// Use this for initialization
	void Start () {
		load.onClick.AddListener(loadOnClick);
		menu.onClick.AddListener(menuOnClick);
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void loadOnClick(){
		int lvl = gameManagerScript.getLevel();
		gameManagerScript.setHealth(gameManagerScript.getMaxHealth());
		if (lvl == 0)
			StartCoroutine(changeScene("Tuto"));
		else 
        	StartCoroutine(changeScene("IATest"));
    }

	void menuOnClick(){
        SceneManager.LoadScene("Menu");
    }

	IEnumerator changeScene(string levelName) {
	yield return new WaitForSeconds(0.1f);
	SceneManager.LoadScene(levelName);
    }
}
