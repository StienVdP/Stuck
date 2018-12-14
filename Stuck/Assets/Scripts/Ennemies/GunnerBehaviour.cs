using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBehaviour : MonoBehaviour {
	// caractéristiques de la balle
    public GameObject bullet;
	public float bulletSpeed; // 0.5f est bien

	// actions du gunner
	public float left_Bottom;
	public float right_Up;
    public float moveSpeed;
	private float maxRight; // position initiale + right
	private float maxLeft; // position initiale + left
    private bool moveRight;
	private int sens; // 1 si moveRight, -1 sinon
	private bool limit; // si le gunner est au bout de sa zone de patrouille
    private bool shoot;
	private bool pause; // si le gunner est arrêté
	private bool isShooting;
	private bool dead;

	private float health;

	// composants du destroyer
	private SpriteRenderer spriteRenderer;
    private Animator animator;
	
	// variables de cooldown
	private float timeStampPause;
	private float timeStampShoot;

	// éléments extérieurs
	private LayerMask playerMask;
    private GameObject gameManager;
    private GameManager gameManagerScript;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

		// initialisation des variables
		moveRight = true;
		sens = 1;
		pause = false;
		limit = false;
		isShooting = false;
		dead = false;
		health = gameManagerScript.getGunnerHealth();

		maxRight = transform.position.x + right_Up;
		maxLeft = transform.position.x - left_Bottom;

		playerMask = LayerMask.GetMask("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (!dead){ // tant que le gunner est en vie, il fait ses actions
			move();
			shootHandler();

			if (moveRight){
				sens = 1;
			}
			else {
				sens = -1;
			}

			animator.SetBool("Pause", pause);
			animator.SetBool("Shoot", shoot);
		}
		
	}

	void OnTriggerEnter2D(Collider2D col)
    {
		// lors d'une collision avec une balle du joueur, le gunner subit des dégâts, mais continue ses actions
        if (col.tag == "BlueBullet")
        {
			health -= gameManagerScript.getDamage();
			gameObject.GetComponent<Animation>().Play("Gunner_Damage");
			if (health < 0){
				dead = true;
				StartCoroutine(Die());
			}
        }
    }

    private void Flip()
    {
        // Switch la direction de l'anim en changeant la valeur du Scale (1 ou -1)
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	void move(){
		// si la durée de la pause est dépassée, on regarde si on est au bout de la zone de patrouille, si oui, on change de sens, sinon on continue
		if (timeStampPause <= Time.time){
			if (limit){
				moveRight = !moveRight;
				Flip();
				limit = false;
			}
			pause = false;
		}
		if (!pause){
			// déplacement
			if (moveRight){
					transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
				}

				else{
					transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
				}
			// lorsque le gunner atteint une limite
			if (moveRight && transform.position.x > maxRight) { 
				pause = true;
				limit = true;
				timeStampPause = Time.time + 2;
			}
			else if (!moveRight && (transform.position.x < maxLeft)){
				pause = true;
				limit = true;
				timeStampPause = Time.time + 2;
			}
		}
	}

	// la vision du gunner est deux rayons devant lui depuis ses fusils, si le joueur en traverse un, le gunner tire toutes les secondes depuis chacun des fusils (donc décalage)
	void shootHandler(){
		RaycastHit2D hit;
		hit = Physics2D.Raycast(transform.position + new Vector3(0,1.0f,0) * transform.localScale.y , Vector2.right * sens, 30.0f, playerMask);
		 if (hit.collider != null){
			pause = true;
			timeStampPause = Time.time + 1;
			var position = this.transform.position;
			if (!isShooting){
				timeStampShoot = Time.time + 1.0f;
				isShooting = true;
			}

			if (timeStampShoot <= Time.time){
				if (moveRight)
				{
					bullet.GetComponent<ShootManagement>().direction = bulletSpeed;
				}
				else
				{
					bullet.GetComponent<ShootManagement>().direction = -bulletSpeed;
				}
				Instantiate(bullet, position + new Vector3(sens * 2, 1.5f * transform.localScale.y ,0), new Quaternion());
				Instantiate(bullet, position + new Vector3(-sens * 0.5f ,-0.8f * transform.localScale.y,0), new Quaternion());
				StartCoroutine("valueShoot");
				isShooting = false;
			}

		 }
	}

	IEnumerator valueShoot()
    {
        shoot = true;
        yield return new WaitForSeconds(1.0f);
        shoot = false;
    }

	private IEnumerator Die()
 	{
		animator.Play("Gunner_Dead");
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
		Destroy(gameObject);
 	}
}
