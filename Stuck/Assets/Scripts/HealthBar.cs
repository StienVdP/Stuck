using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public const int maxHealth = 100;
    public int currentHealth = maxHealth;
    public RectTransform healthBar;
    /*
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }

        healthBar.sizeDelta = new Vector2(currentHealth, healthBar.sizeDelta.y);
    }
    */
    private GameManager gameManagerScript;

    // Use this for initialization
    void Start () {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        int health = gameManagerScript.getHealth();
        Debug.Log("Vie : " + health);
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }
}
