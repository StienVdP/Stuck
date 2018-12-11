using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour {

    public GameObject exitDoor;
    private Animator animation;
    public string nextLevelName;
    private GameObject gameManager;
    private GameManager gameManagerScript;

    // Use this for initialization
    void Start () {
        animation = exitDoor.GetComponent<Animator>();
        animation.enabled = false;
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            // Animation ouverture de porte
            animation.enabled = true;
            // Changement de scène
            if (gameManagerScript.getLevel() == 5)
            {
                gameManagerScript.increaseLevel();
                StartCoroutine(changeScene("Win"));
            }
            else
            {
                StartCoroutine(changeScene(nextLevelName));

            }
        }
    }

    IEnumerator changeScene(string levelName) {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(levelName);
    }
}
