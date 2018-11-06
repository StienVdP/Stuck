using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

	public Button next;
	public Button exit;
	
	public ToggleGroup toggleGroupInstance;
    private GameObject gameManager;
    private GameManager gameManagerScript;

	public Toggle currentSelection{
		get { return toggleGroupInstance.ActiveToggles().FirstOrDefault() ; }
	}
	// Use this for initialization
	void Start () {
		next.onClick.AddListener(nextOnClick);
		exit.onClick.AddListener(exitOnClick);
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
	}

	void nextOnClick(){
		if (currentSelection.name == "Option 1"){
			gameManagerScript.doubleJumpOn();
		}
		if (currentSelection.name == "Option 2"){
			gameManagerScript.climbOn();
		}
		gameManagerScript.increaseLevel();
		Debug.Log(gameManagerScript.getLevel());
		gameManagerScript.setLife(100);
		StartCoroutine(changeScene("Test"));
	}

	void exitOnClick(){
		Application.Quit();
	}

	IEnumerator changeScene(string levelName) {
	yield return new WaitForSeconds(1.0f);
	SceneManager.LoadScene(levelName);
    }
	
	public void selectToggle (int id){
	var toggles = toggleGroupInstance.GetComponentsInChildren<Toggle>();
	toggles[id].isOn = true;
	}

}
