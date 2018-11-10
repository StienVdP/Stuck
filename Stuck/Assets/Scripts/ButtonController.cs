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
		choice(gameManagerScript.getLevel());
	}

	void nextOnClick(){
		int lvl = gameManagerScript.getLevel();
		if (currentSelection.GetComponentInChildren<Text>().text == "Double saut"){
			switch(lvl){
				case 1:
					Debug.Log("ici");
					gameManagerScript.shootOn();
					break;
				case 2:
					gameManagerScript.doubleJumpOn();
					break;
				case 3:
					break;
				case 4:
					break;
				case 5:
					break;
			}
		}
		if (currentSelection.GetComponentInChildren<Text>().text == "Escalader"){
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
		Debug.Log(gameManagerScript.getLevel());
		gameManagerScript.setLife(100);
		StartCoroutine(changeScene("Test"));
	}

	void exitOnClick(){
		Application.Quit();
	}

	void choice(int lvl){
		
		switch(lvl){
			case 1:
				GameObject choice1 = GameObject.Find("Option1");
				Text text1 = choice1.GetComponentInChildren<Text>();
				text1.text = "Double saut";
				GameObject choice2 = GameObject.Find("Option2");
				Text text2 = choice2.GetComponentInChildren<Text>();
				text2.text = "Escalade";
				break;
			case 2:
				GameObject choice3 = GameObject.Find("Option1");
				Text text3 = choice3.GetComponentInChildren<Text>();
				text3.text = "Frapper";
				GameObject choice4 = GameObject.Find("Option2");
				Text text4 = choice4.GetComponentInChildren<Text>();
				text4.text = "Bouclier";
				break;
			case 3:
				GameObject choice5 = GameObject.Find("Option1");
				Text text5 = choice5.GetComponentInChildren<Text>();
				text5.text = "Tirer";
				GameObject choice6 = GameObject.Find("Option2");
				Text text6 = choice6.GetComponentInChildren<Text>();
				text6.text = "Soigner";
				break;
			case 4:
				GameObject choice7 = GameObject.Find("Option1");
				Text text7 = choice7.GetComponentInChildren<Text>();
				text7.text = "Santé max +10";
				GameObject choice8 = GameObject.Find("Option2");
				Text text8 = choice8.GetComponentInChildren<Text>();
				text8.text = "Saut plus haut";
				break;
			case 5:
				GameObject choice9 = GameObject.Find("Option1");
				Text text9 = choice9.GetComponentInChildren<Text>();
				text9.text = "Dégats +10";
				GameObject choice10 = GameObject.Find("Option2");
				Text text10 = choice10.GetComponentInChildren<Text>();
				text10.text = "Vitesse +10";
				break; 
		}
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
