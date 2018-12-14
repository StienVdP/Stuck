using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private GameObject gameManager;
    private GameManager gameManagerScript;

    void Start(){
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();    
    }

	public void playGame()
    {
        gameManagerScript.setLevel(1);

        Debug.Log("Change de state : ");
        gameManagerScript.setNewState(); // Change de seed pour avoir un nouveau random
        gameManagerScript.setHealth(gameManagerScript.getMaxHealth());
        SceneManager.LoadScene("IATest");
    }
    public void playTutoGame()
    {
        SceneManager.LoadScene("Tuto");
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
