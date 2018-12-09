using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void playGame()
    {
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
