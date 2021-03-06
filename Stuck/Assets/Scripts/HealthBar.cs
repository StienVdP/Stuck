﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    
    public GameObject healthBar;
    private GameManager gameManagerScript;
    private float hp;
    private float maxHealth;

    // Use this for initialization
    void Start () {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        maxHealth = gameManagerScript.getMaxHealth();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameManagerScript.getHealth() <= 0)
        {
            hp = 0.0f;
        }
        else if(gameManagerScript.getHealth() >= maxHealth)
        {
            hp = 1.0f;
        }
        else
        {
            hp = gameManagerScript.getHealth() / maxHealth; // 100.0f
        }
        // Mis à jour de la barre de vie
        healthBar.gameObject.transform.localScale = new Vector3(hp, 1.0f, 1.0f);
    }
}
