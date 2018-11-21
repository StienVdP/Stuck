using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private static bool created = false;	
	private int life;
	private int level;
	private bool doubleJump;
	private bool climb;
	private bool protect;
    private bool shoot;
    private Random.State oldstate;

	// Use this for initialization
	void Awake () {
		if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
            oldstate = Random.state; // crée et stock le random.state lors de la création du niveau
        }
		life = 100;
		level = 1;
		doubleJump = false;
		climb = false;
		protect = false;
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log("Level : " + level);
    }

	public int getLife(){
		return life;
	}
	public void setLife(int l){
		if (l <= 0){
			life = 0;
			gameOver();
		}
		else 
			life = l;
	}

	public int getLevel(){
		return level;
	}
	public void increaseLevel(){
		level += 1;
	}

	public bool isDoubleJumpOn(){
		return doubleJump;
	}
	public void doubleJumpOn(){
		doubleJump = true;
	}
	public void doubleJumpOff(){
		doubleJump = false;
	}

	public bool isClimbOn(){
		return climb;
	}
	public void climbOn(){
		climb = true;
	}
	public void climbOff(){
		climb = false;
	}

	public bool isProtectOn(){
		return protect;
	}
	public void protectOn(){
		protect = true;
	}
	public void protectOff(){
		protect = false;
	}

	public bool isShootOn(){
		return shoot;
	}
	public void shootOn(){
		shoot = true;
	}
	public void shootOff(){
		shoot = false;
	}

	public void gameOver(){
		StartCoroutine(changeScene("gameOver"));
	}
	IEnumerator changeScene(string levelName) {
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene(levelName);
    }

    public Random.State getState()
    {
        return oldstate;
    }

}
