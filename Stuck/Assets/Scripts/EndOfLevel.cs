using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfLevel : MonoBehaviour {

    public GameObject exitDoor;
    private Animator animation;
    public string nextLevelName;

    // Use this for initialization
    void Start () {
        animation = exitDoor.GetComponent<Animator>();
        animation.enabled = false;
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
            if (GameObject.Find("GameManager").GetComponent<GameManager>().getLevel() == 5)
            {
                StartCoroutine(changeScene("Win"));
            }
            else
            {
                StartCoroutine(changeScene(nextLevelName));

            }
        }
    }

    IEnumerator changeScene(string levelName) {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(levelName);
    }
}
