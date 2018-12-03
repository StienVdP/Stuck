using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private static bool created = false;	
	private int health;
	private int maxHealth;
	private int level;
	public bool doubleJump;
	public bool wallJump;
	public bool protect;
    public bool shoot;
	public bool dash;
	public bool tp;
    private Random.State oldstate;

    // Use this for initialization
    void Awake () {
		if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
		}
		maxHealth = 100;
		health = 100;
		level = 1;
		doubleJump = false;
		wallJump = false;
		protect = false;
	}

	// Update is called once per frame
	void Update () {

        Debug.Log("Level : " + level);
    }

	public int getHealth(){
		return health;
	}
	public void setHealth(int l){
		if (l <= 0){
			health = 0;
			gameOver();
		}
		else 
			health = l;
	}

	public int getMaxHealth(){
		return maxHealth;
	}
	public void setMaxHealth(int ml){
		maxHealth = ml;
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

    public Random.State getState()
    {
        return oldstate;
    }

}
