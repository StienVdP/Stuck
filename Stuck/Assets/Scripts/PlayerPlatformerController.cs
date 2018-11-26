using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public float maxSpeed;
    public float jumpTakeOffSpeed;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rigidbody;

    private bool crouch;
    private bool shoot;
    private bool droite;
    private bool wallSliding;

    public GameObject bullet;
    public float bulletSpeed; // 0.5f est bien
    public bool wallCheck;
    public Transform wallCheckPoint;


    private GameObject gameManager;
    private GameManager gameManagerScript;

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        droite = true;
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }
    
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && !wallSliding){
            if  (grounded) // Grounded : pour sauter il faut que le player soit au sol 
            {
                velocity.y = jumpTakeOffSpeed; // Fait monter = saut
                jumpCount += 1;
            }
            else if (gameManagerScript.isDoubleJumpOn() && jumpCount<2){
                velocity.y = jumpTakeOffSpeed * 1.1f; // Fait monter = saut
                jumpCount += 1;
            }
        } 
         
        else if (Input.GetButtonUp("Jump") && !wallSliding)
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.3f; // Fait re dessandre = fin du saut
            }
        }

        if (Input.GetButtonDown("Fire1")) // Shoot
        {
            if(gameManagerScript.isShootOn() == true) // Si l'option 1 est choisi on tire
            {
                var position = this.transform.position;
                if (droite == true)
                {
                    bullet.GetComponent<ShootManagement>().direction = bulletSpeed;
                }
                else
                {
                    bullet.GetComponent<ShootManagement>().direction = -bulletSpeed;
                }
                Instantiate(bullet, position, new Quaternion());
                StartCoroutine("valueShoot");
            }
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            crouch = true;
            move.x = 0; // Empêche de bouger quand il se baisse
        }
        else crouch = false;
        
        // S'il va vers la droite et que l'anim est direction gauche
        if (move.x > 0.00f && !droite)
        {
            Flip();
        }
        // L'inverse s'il va vers la gauche et que l'anim est direction droite
        else if (move.x < 0.00f && droite)
        {
            Flip();
        }

        if (gameManagerScript.isClimbOn()){
            if (!grounded){
                wallCheck = Physics2D.OverlapCircle(wallCheckPoint.position, 0.1f, 9);
                if (droite && Input.GetAxis("Horizontal") > 0.1f || !droite && Input.GetAxis("Horizontal") < 0.1f){
                    if (wallCheck)
                        handleWallSliding();
                }
            }
            if (!wallCheck)
                wallSliding = false;
        }
        
        if (gameManagerScript.isDashOn()){
            if (Input.GetKeyDown("left shift")){
                handleDash();
            }
        }

        animator.SetBool("Ground", grounded);
        animator.SetFloat("Speed", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetBool("Crouch", crouch); 
        animator.SetBool("Shoot", shoot); // Peut etre mettre dans un Update ? pour un shoot continue

        targetVelocity = move * maxSpeed * 1.2f; // Fait avancer

        /*if (grounded){
            //RaycastHit2D hit = Physics2D.Raycast(this.transform.position, -Vector2.up, 0.1f);
            RaycastHit2D[] hits = new RaycastHit2D[2];
            int h = Physics2D.RaycastNonAlloc(transform.position, -Vector2.up, hits);
            if (hit){
                this.rb2d.velocity.Set(this.rb2d.velocity.x - hit.normal.x * 0.6f, this.rb2d.velocity.y);
                Vector3 pos = transform.position;
                Debug.Log(hit.normal);
                float angle = Mathf.Abs(Mathf.Atan2(hit.normal.x, hit.normal.y)*Mathf.Rad2Deg);
                Debug.Log(angle);
                pos.y +=  - hit.normal.x * Mathf.Abs(this.rb2d.velocity.x) * Time.deltaTime * (this.rb2d.velocity.x - hit.normal.x > 0? 1 : -1);
                transform.position = pos;
            }
            if (h>1){
                float angle = Mathf.Abs(Mathf.Atan2(hits[1].normal.x, hits[1].normal.y)*Mathf.Rad2Deg);
                if (angle > 0 || angle <0){
                    this.rb2d.velocity.Set(this.rb2d.velocity.x - hits[1].normal.x * 0.6f, this.rb2d.velocity.y);
                    Vector3 pos = transform.position;
                    pos.y +=  - hits[1].normal.x * Mathf.Abs(this.rb2d.velocity.x) * Time.deltaTime * (this.rb2d.velocity.x - hits[1].normal.x > 0? 1 : -1);
                    transform.position = pos;
                }
            }
        }*/
    }

    private void Flip()
    {
        // Switch la direction de l'anim en changeant la valeur du Scale (1 ou -1)
        droite = !droite;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void handleWallSliding() {
        velocity.y = -0.2f;
        wallSliding = true;
        if (Input.GetButtonDown("Jump")) {
            if (droite) {
                Flip();
                transform.position = transform.position + new Vector3(-2.5f,0,0);
                velocity.y = jumpTakeOffSpeed * 1.2f; // Fait monter = saut
                jumpCount += 1;
            }
            else {
                Flip();
                transform.position = transform.position + new Vector3(2.5f,0,0);
                velocity.y = jumpTakeOffSpeed * 1.2f; // Fait monter = saut
                jumpCount += 1;
            }
        }
    }

    private void handleDash(){
        if (droite){
            RaycastHit2D hit, hit2, hit3;
            hit = Physics2D.Raycast(transform.position, Vector2.right, 5.0f, 9);
            hit2 = Physics2D.Raycast(transform.position - new Vector3(0,2,0), Vector2.right, 5.0f, 9);
            hit3 = Physics2D.Raycast(transform.position + new Vector3(0,2,0), Vector2.right, 5.0f, 9);

            // If it hits something...
            if (hit.collider != null || hit2.collider != null || hit3.collider != null)
            {
                float distance = Mathf.Min(new float[]{hit.distance, hit2.distance, hit3.distance});
                transform.position = transform.position + new Vector3(distance,0,0);
            }
            else
                transform.position = transform.position + new Vector3(5,0,0);
        }
        else {
            RaycastHit2D hit, hit2, hit3;
            hit = Physics2D.Raycast(transform.position, Vector2.left, 5.0f, 9);
            hit2 = Physics2D.Raycast(transform.position - new Vector3(0,2,0), Vector2.left, 5.0f, 9);
            hit3 = Physics2D.Raycast(transform.position + new Vector3(0,2,0), Vector2.left, 5.0f, 9);

            // If it hits something...
            if (hit.collider != null || hit2.collider != null || hit3.collider != null)
            {
                float distance = Mathf.Min(new float[]{hit.distance, hit2.distance, hit3.distance});
                transform.position = transform.position + new Vector3(-distance,0,0);
            }
            else
                transform.position = transform.position + new Vector3(-5,0,0);
        }
    }

    IEnumerator valueShoot()
    {
        shoot = true;
        yield return new WaitForSeconds(1.0f);
        shoot = false;
    }

}