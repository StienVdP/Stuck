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
    private int sens;
    private bool wallSliding;
    private float timeStampTp;
    private float timeStampDash;
    private float timeStampDamage;

    public GameObject bullet;
    public float bulletSpeed; // 0.5f est bien
    private bool wallCheckFront;
    private bool wallCheckBack;

    private LayerMask solidMask;
    private LayerMask trapMask;
    private GameObject gameManager;
    private GameManager gameManagerScript;

    // Use this for initialization
    void Awake()
    {
        solidMask = LayerMask.GetMask("Solid");
        trapMask = LayerMask.GetMask("Trap");

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        droite = true;
        sens = 1;
        gameManager = GameObject.Find("GameManager");
        gameManagerScript = gameManager.GetComponent<GameManager>();
    }
    
    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump")){
            if  (grounded) // Grounded : pour sauter il faut que le player soit au sol => pas de double saut
            {
                velocity.y = jumpTakeOffSpeed; // Fait monter = saut
                jumpCount += 1;
            }
            else if (gameManagerScript.isDoubleJumpOn() && jumpCount<2){
                velocity.y = jumpTakeOffSpeed * 1.1f; // Fait monter = saut
                jumpCount += 1;
            }
        } 
         
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.3f; // Fait re descendre = fin du saut
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
                Instantiate(bullet, position + new Vector3(sens * 1.5f,0,0), new Quaternion());
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

        if (gameManagerScript.isWallJumpOn()){
            if (!grounded){
                RaycastHit2D hit, hit2;
                hit = Physics2D.Raycast(transform.position + new Vector3(0,0.5f,0), Vector2.right * sens, 1.0f, solidMask);
                hit2 = Physics2D.Raycast(transform.position + new Vector3(0,0.5f,0), -Vector2.right * sens, 2.0f, solidMask);
                wallCheckBack = hit2.collider != null;
                wallCheckFront = hit.collider != null;
                //if (droite && Input.GetAxis("Horizontal") > 0.1f || !droite && Input.GetAxis("Horizontal") < 0.1f){
                    if (wallCheckFront || wallCheckBack)
                        handleWallJumping(ref move);
                //}
            }
            if (grounded){
                wallCheckBack = false;
                wallCheckFront = false;
            }
            if (!wallCheckBack && !wallCheckFront){
                wallSliding = false;
            }
        }
        
        if (gameManagerScript.isTpOn()){
            if (Input.GetKeyDown("left shift")){
                handleTp();
            }
        }

        if (gameManagerScript.isDashOn()){
            if (Input.GetKeyDown("left shift")){
                handleDash(ref move);
            }
        }

        if (droite){
            sens = 1;
        }
        else {
            sens = -1;
        }

        animator.SetBool("Ground", grounded);
        animator.SetFloat("Speed", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetBool("Crouch", crouch); 
        animator.SetBool("Shoot", shoot); // Peut etre mettre dans un Update ? pour un shoot continue

        targetVelocity = move * maxSpeed * 1.2f; // Fait avancer
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trap") || collision.gameObject.layer == LayerMask.NameToLayer("Ennemy"))
        {
            if (timeStampDamage <= Time.time){
                gameObject.GetComponent<Animation>().Play("Damage_Player");
                gameManagerScript.setHealth(gameManagerScript.getHealth() - 35);
                timeStampDamage = Time.time + 1;
            }
            //StartCoroutine(knockBack(0.01f, 100, transform.position));
            rb2d.velocity = new Vector2 (0, 0); 
            rb2d.AddForce(new Vector3( -sens * 100, 200, 0), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.tag == "YellowBullet"){
            Vector2 contact = collision.contacts[0].point;
            if (timeStampDamage <= Time.time){
                gameObject.GetComponent<Animation>().Play("Damage_Player");
                gameManagerScript.setHealth(gameManagerScript.getHealth() - 25);
                timeStampDamage = Time.time + 1;
            }
            //StartCoroutine(knockBack(0.5f, 5, transform.position));
            rb2d.velocity = new Vector2 (0, 0); 
            if (contact.x < transform.position.x)
                rb2d.AddForce(new Vector3( 1 * 100, 200, 0), ForceMode2D.Impulse);
            else 
                rb2d.AddForce(new Vector3( -1 * 100, 200, 0), ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        // Switch la direction de l'anim en changeant la valeur du Scale (1 ou -1)
        droite = !droite;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        if (droite)
            transform.position = new Vector3(transform.position.x + 0.9f, transform.position.y, transform.position.z);
        else 
            transform.position = new Vector3(transform.position.x - 0.9f, transform.position.y, transform.position.z);
    }

    private void handleWallJumping(ref Vector2 move) {
        rb2d.velocity = new Vector2(rb2d.velocity.x, -0.1f);
        wallSliding = true;
        if (Input.GetButtonDown("Jump")) {
            if (droite && wallCheckFront){
                move.x = -10.0f;
            }
            else if (!droite && wallCheckBack){
                move.x = -10.0f;
            }
            else if (!droite && wallCheckFront){
                move.x = 10.0f;
            }
            else if (droite && wallCheckBack){
                move.x = 10.0f;
            }
            velocity.y = jumpTakeOffSpeed * 1.2f;
            jumpCount = 1;
        }
    }

    private void handleTp(){
        if (timeStampTp <= Time.time){
            RaycastHit2D hit, hit2, hit3;
            hit = Physics2D.Raycast(transform.position, Vector2.right * sens, 15.0f, solidMask);
            hit2 = Physics2D.Raycast(transform.position - new Vector3(0,2,0), Vector2.right * sens, 15.0f, solidMask);
            hit3 = Physics2D.Raycast(transform.position + new Vector3(0,2,0), Vector2.right * sens, 15.0f, solidMask);

            // If it hits something...
            if (hit.collider != null || hit2.collider != null || hit3.collider != null)
            {
                float distance = Mathf.Min(new float[]{hit.distance, hit2.distance, hit3.distance});
                transform.position = transform.position + new Vector3(distance * sens,0,0);
            }
            else
                transform.position = transform.position + new Vector3(15 * sens,0,0);
            timeStampTp = Time.time + 2;
        }
    }

    private void handleDash(ref Vector2 move){
        if (timeStampDash <= Time.time){
            move.x *= 25.0f;
            timeStampDash = Time.time + 2;
        }
    }

    IEnumerator valueShoot()
    {
        shoot = true;
        yield return new WaitForSeconds(1.0f);
        shoot = false;
    }

    IEnumerator knockBack(float knockDur, float knockPwr, Vector3 knockDir){
        float timer = 0;
        while (knockDur > timer){
            timer += Time.deltaTime;
            rb2d.velocity = new Vector2 (0, 0); 
            rb2d.AddForce(new Vector3( -sens * 3 * knockPwr, knockPwr, 0), ForceMode2D.Impulse);
            //transform.position = new Vector3(knockDir.x * -sens, knockDir.y + knockPwr, knockDir.z) * Time.deltaTime;
            //velocity = new Vector2(-sens, knockPwr) * Time.deltaTime;
        }
        yield return 0;
    }
}