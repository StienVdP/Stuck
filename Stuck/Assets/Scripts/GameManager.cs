using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	// le joueur et son animator
	public GameObject player;
	private Animator animator;

	// variable qui détermine si le gameManager est déjà créé
	private static bool created = false;	

	// caractéristiques du joueur
	private int health;
	private int maxHealth;
	private int damage;

	// état du jeu
	private int level;

	// compétences du joueur
	private bool doubleJump;
	private bool wallJump;
	private bool shield;
    private bool shoot;
	private bool dash;
	private bool tp;
	private bool armor;
	private bool heal;

	// caractéristiques des ennemis
	private int destoyerHealth;
	private int gunnerHealth;

	// variable qui contient les étapes précédentes de la génération procédurale
    private Random.State oldstate;

    private int initInt;

    // Use this for initialization
    void Awake () {
		// si le gameManager n'était pas déjà créé, on le met en DontDestroyOnLoad pour le garder dans toutes les scènes
		if (!created)
        {
            DontDestroyOnLoad(gameObject);
            created = true;
		}

        oldstate = Random.state; // crée et stock le random.state lors de la création du niveau
        
		// initialisation de toutes les variables
		maxHealth = 100;
		health = 100;
		level = 0;
		damage = 25;
		destoyerHealth = 200;
		gunnerHealth = 100;
		doubleJump = true;
		wallJump = false;
		shield = false;
		armor = false;
		heal = false;

		// récupération de l'animator
		animator = player.GetComponent<Animator>();
		
	}

	// getter/setter de la vie du joueur
	public int getHealth(){ return health; }
	public void setHealth(int l){
		if (l <= 0){
			health = 0;
			StartCoroutine(Die());
		}
		else 
			health = l;
	}

	// getter/setter des dommages du joueur
	public int getDamage(){ return damage; }
	public void setDamage(int d){ damage = d; }

	// getter/setter de la vie maximum du joueur
	public int getMaxHealth(){ return maxHealth; }
	public void setMaxHealth(int ml){ maxHealth = ml; }

	// getter/setter du niveau de la partie
	public int getLevel(){ return level; }
	public void increaseLevel(){ level += 1; }
	public void setLevel(int i){ level = i; }

	// getter/setter de la compétence doubleJump
	public bool isDoubleJumpOn(){ return doubleJump; }
	public void doubleJumpOn(){ doubleJump = true; }
	public void doubleJumpOff(){ doubleJump = false; }

	// getter/setter de la compétence wallJump
	public bool isWallJumpOn(){ return wallJump; }
	public void wallJumpOn(){ wallJump = true; }
	public void wallJumpOff(){ wallJump = false; }

	// getter/setter de la compétence shield
	public bool isShieldOn(){ return shield; }
	public void shieldOn(){ shield = true; }
	public void shieldOff(){ shield = false; }

	// getter/setter de la compétence shoot
	public bool isShootOn(){ return shoot; }
	public void shootOn(){ shoot = true; }
	public void shootOff(){ shoot = false; }

	// getter/setter de la compétence dash
	public bool isDashOn(){ return dash; }
	public void dashOn(){ dash = true; }
	public void dashOff(){ dash = false; }

	// getter/setter de la compétence tp
	public bool isTpOn(){ return tp; }
	public void tpOn(){ tp = true; }
	public void tpOff(){ tp = false; }

	// getter/setter de la compétence armor
	public bool isArmorOn(){ return armor; }
	public void armorOn(){ armor = true; }
	public void armorOff(){ armor = false; }

	// getter/setter de la compétence heal
	public bool isHealOn(){ return heal; }
	public void healOn(){ heal = true; }
	public void healOff(){ heal = false; }

	// getter/setter de la vie des ennemis
	public int getDestroyerHealth(){ return destoyerHealth; }
	public void setDestroyerHealth(int h){ destoyerHealth = h; }


	public int getGunnerHealth(){ return gunnerHealth; }
	public void setGunnerHealth(int h){ gunnerHealth = h; }

	// remise à zéro des compétences
	public void resetAll(){
		tp = false;
		dash = false;
		wallJump = false;
		shield = false;
		heal = false;
		armor = false;
		shoot = false;
	}

	// coroutine de changement de scène
	IEnumerator changeScene(string levelName) {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(levelName);
    }

	// coroutine de la mort du joueur
	private IEnumerator Die()
 	{
		animator.Play("Dead_Player");
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length - 0.3f);
		StartCoroutine(changeScene("gameOver"));
 	}

	// récupération de la génération procédurale
    public Random.State getState()
    {
        return initInt;
    }

    public void setNewState()
    {
        initInt += 1;
    }
}
