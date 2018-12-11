using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

	public Button next;
	public Button menu;
	
	public ToggleGroup toggleGroupInstance;
    private GameObject gameManager;
    private GameManager gameManagerScript;
	private string[] Options1 = {"WallJump", "Dash", "Max health +30", "Shield"};
	private string[] Options2 = {"Shoot", "Teleportation", "Armor", "Heal"};

	public Toggle currentSelection{
		get { return toggleGroupInstance.ActiveToggles().FirstOrDefault() ; }
	}
	// Use this for initialization
	void Start () {
		next.onClick.AddListener(nextOnClick);
		menu.onClick.AddListener(menuOnClick);
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
		choice(gameManagerScript.getLevel());
	}

	void nextOnClick(){
		int lvl = gameManagerScript.getLevel();
		if (currentSelection.GetComponentInChildren<Text>().text == Options1[lvl-1]){
			switch(lvl){
				case 1:
					gameManagerScript.wallJumpOn();
					break;
				case 2:
					gameManagerScript.dashOn();
					break;
				case 3:
					gameManagerScript.setMaxHealth(gameManagerScript.getMaxHealth() + 30);
					break;
				case 4:
					gameManagerScript.shieldOn();
					break;
				case 5:
					break;
			}
		}
		if (currentSelection.GetComponentInChildren<Text>().text == Options2[lvl-1]){
			switch(lvl){
				case 1:
					gameManagerScript.shootOn();
					break;
				case 2:
					gameManagerScript.tpOn();
					break;
				case 3:
					gameManagerScript.armorOn();
					break;
				case 4:
					gameManagerScript.healOn();
					break;
				case 5:
					break;
			}
		}
		gameManagerScript.increaseLevel();
		gameManagerScript.setHealth(gameManagerScript.getMaxHealth());
		StartCoroutine(changeScene());
	}

	void menuOnClick(){
		SceneManager.LoadScene("Menu");
	}

	void choice(int lvl){
				GameObject choice1 = GameObject.Find("Option1");
				Text text1 = choice1.GetComponentInChildren<Text>();
				text1.text = Options1[lvl-1];
				GameObject choice2 = GameObject.Find("Option2");
				Text text2 = choice2.GetComponentInChildren<Text>();
				text2.text = Options2[lvl-1];
	}
	IEnumerator changeScene() {
	yield return new WaitForSeconds(0.3f);
        /*** Changement pour la IA de la map ***/
        SceneManager.LoadScene("IATest");

    }

    public void selectToggle (int id){
	var toggles = toggleGroupInstance.GetComponentsInChildren<Toggle>();
	toggles[id].isOn = true;
	}

}
