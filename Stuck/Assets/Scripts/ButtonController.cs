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
	private string[] Options1 = {"Double saut", "Tirer", "Santé Max +25", "Frapper"};
	private string[] Options2 = {"Escalader", "Bouclier", "Soigner", "Saut plus haut"};

	public Toggle currentSelection{
		get { return toggleGroupInstance.ActiveToggles().FirstOrDefault() ; }
	}
	// Use this for initialization
	void Start () {
		next.onClick.AddListener(nextOnClick);
		exit.onClick.AddListener(exitOnClick);
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
		choice(gameManagerScript.getLevel());
	}

	void nextOnClick(){
		int lvl = gameManagerScript.getLevel();
		if (currentSelection.GetComponentInChildren<Text>().text == Options1[lvl-1]){
			switch(lvl){
				case 1:
					gameManagerScript.doubleJumpOn();
					break;
				case 2:
					gameManagerScript.shootOn();
					break;
				case 3:
					break;
				case 4:
					break;
				case 5:
					break;
			}
		}
		if (currentSelection.GetComponentInChildren<Text>().text == Options2[lvl-1]){
			switch(lvl){
				case 1:
					gameManagerScript.climbOn();
					break;
				case 2:
					break;
				case 3:
					break;
				case 4:
					break;
				case 5:
					break;
			}
		}
		gameManagerScript.increaseLevel();
		lvl = gameManagerScript.getLevel();
		gameManagerScript.setLife(100);
		StartCoroutine(changeScene("Test"+lvl.ToString()));
	}

	void exitOnClick(){
		Application.Quit();
	}

	void choice(int lvl){
				GameObject choice1 = GameObject.Find("Option1");
				Text text1 = choice1.GetComponentInChildren<Text>();
				text1.text = Options1[lvl-1];
				GameObject choice2 = GameObject.Find("Option2");
				Text text2 = choice2.GetComponentInChildren<Text>();
				text2.text = Options2[lvl-1];
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
