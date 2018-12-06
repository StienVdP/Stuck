using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	private static bool created = false;	
	public int health;
	private int maxHealth;
	private int damage;
	private int level;
	public bool doubleJump;
	public bool wallJump;
	public bool protect;
    public bool shoot;
	public bool dash;
	public bool tp;
	private int destoyerHealth;
	private int destroyerDamage;
	private int gunnerHealth;
	private int gunnerDamage;
	private int sawDamage;
	private int flameDamage;
	private int laserDamage;
    private Random.State oldstate;
	public GameObject player;
	private Animator animator;

    // Use this for initialization
    void Awake () {
		if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
		}
            oldstate = Random.state; // crée et stock le random.state lors de la création du niveau
        
		maxHealth = 100;
		health = 100;
		level = 1;
		damage = 25;
		destroyerDamage = 30;
		destoyerHealth = 200;
		gunnerHealth = 100;
		gunnerDamage = 20;
		flameDamage = 35;
		laserDamage = 40;
		sawDamage = 35;
		doubleJump = false;
		wallJump = false;
		protect = false;

		animator = player.GetComponent<Animator>();
		
	}

	// Update is called once per frame
	void Update () {

    }

	public int getHealth(){
		return health;
	}
	public void setHealth(int l){
		if (l <= 0){
			health = 0;
			StartCoroutine(Die());
		}
		else 
			health = l;
	}

	public int getDamage(){
		return damage;
	}
	public void setDamage(int d){
		damage = d;
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

	public int getDestroyerHealth(){
		return destoyerHealth;
	}
	public void setDestroyerHealth(int h){
		destoyerHealth = h;
	}

	public int getDestroyerDamage(){
		return destroyerDamage;
	}
	public void setDestroyerDamage(int d){
		destroyerDamage = d;
	}

	public int getGunnerHealth(){
		return gunnerHealth;
	}
	public void setGunnerHealth(int h){
		gunnerHealth = h;
	}

	public int getGunnerDamage(){
		return gunnerDamage;
	}
	public void setGunnerDamage(int d){
		gunnerDamage = d;
	}

	public int getFlameDamage(){
		return flameDamage;
	}
	public void setFlameDamage(int d){
		flameDamage = d;
	}

	public int getLaserDamage(){
		return laserDamage;
	}
	public void setLaserDamage(int d){
		laserDamage = d;
	}

	public int getSawDamage(){
		return sawDamage;
	}
	public void setSawDamage(int d){
		sawDamage = d;
	}

	public void gameOver(){
		StartCoroutine(changeScene("gameOver"));
	}

	IEnumerator changeScene(string levelName) {
		yield return new WaitForSeconds(0.1f);
		SceneManager.LoadScene(levelName);
    }

	private IEnumerator Die()
 	{
		animator.Play("Dead_Player");
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length - 0.3f);
		gameOver();
 	}

    public Random.State getState()
    {
        return oldstate;
    }

}
