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
        gameManagerScript.setHealth(gameManagerScript.getMaxHealth());
        gameManagerScript.resetAll();
        SceneManager.LoadScene("IATest");
    }
    public void playTutoGame()
    {
        gameManagerScript.resetAll();
        SceneManager.LoadScene("Tuto");
    }
    public void quitGame()
    {
        Application.Quit();
    }
}
