using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {
    
    public GameObject healthBar;
    private GameManager gameManagerScript;
    private float hp;

    // Use this for initialization
    void Start () {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameManagerScript.getHealth() <= 0)
        {
            hp = 0.0f;
        }
        else if(gameManagerScript.getHealth() >= 100)
        {
            hp = 1.0f;
        }
        else
        {
            hp = gameManagerScript.getHealth() / 100.0f;
        }
        healthBar.gameObject.transform.localScale = new Vector3(hp, 1.0f, 1.0f);
    }
}
