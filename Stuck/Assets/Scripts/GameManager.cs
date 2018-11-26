using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private static bool created = false;	
	private int life;
	private int maxLife;
	private int level;
	public bool doubleJump;
	public bool wallJump;
	public bool protect;
    public bool shoot;
	public bool dash;
	public bool tp;

	// Use this for initialization
	void Awake () {
		if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
		}
		maxLife = 100;
		life = 100;
		level = 1;
		doubleJump = false;
		wallJump = false;
		protect = false;
	}
	
	// Update is called once per frame
	void Update () {
		
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

	public int getMaxLife(){
		return maxLife;
	}
	public void setMaxLife(int ml){
		maxLife = ml;
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

	public bool isWallJumpOn(){
		return wallJump;
	}
	public void wallJumpOn(){
		wallJump = true;
	}
	public void wallJumpOff(){
		wallJump = false;
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

	public bool isDashOn(){
		return dash;
	}
	public void dashOn(){
		dash = true;
	}
	public void dashOff(){
		dash = false;
	}

	public bool isTpOn(){
		return tp;
	}
	public void tpOn(){
		tp = true;
	}
	public void tpOff(){
		tp = false;
	}

	public void gameOver(){
		StartCoroutine(changeScene("gameOver"));
	}

	IEnumerator changeScene(string levelName) {
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene(levelName);
    }

}
