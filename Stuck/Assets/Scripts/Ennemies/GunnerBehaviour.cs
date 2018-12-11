using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBehaviour : MonoBehaviour {

    public GameObject bullet;
	public float bulletSpeed; // 0.5f est bien
    private bool shoot;
	public float left_Bottom;
	public float right_Up;

	private SpriteRenderer spriteRenderer;
    private Animator animator;
	
    public float moveSpeed;
	private float maxRight;
	private float maxLeft;
    private bool moveRight;
	private int sens;
	private bool limit;

	private float timeStampPause;
	private float timeStampShoot;
	private bool pause;
	private bool isShooting;
	private LayerMask playerMask;
	
    private GameObject gameManager;
    private GameManager gameManagerScript;

	private float health;
	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

		moveRight = true;
		sens = 1;
		pause = false;
		limit = false;
		isShooting = false;
		health = gameManagerScript.getGunnerHealth();

		maxRight = transform.position.x + right_Up;
		maxLeft = transform.position.x - left_Bottom;

		playerMask = LayerMask.GetMask("Player");
	}
	
	// Update is called once per frame
	void Update () {
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

	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "BlueBullet")
        {
			health -= gameManagerScript.getDamage();
			gameObject.GetComponent<Animation>().Play("Gunner_Damage");
			if (health < 0){
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
		if (timeStampPause <= Time.time){
			if (limit){
				moveRight = !moveRight;
				Flip();
				limit = false;
			}
			pause = false;
		}
		if (!pause){
			if (moveRight){
					transform.position = new Vector3(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
				}

				else{
					transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);
				}
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
