using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    // variables de vitesse 
    public float maxSpeed;
    public float jumpTakeOffSpeed;

    // composants du joueur
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rigidbody;

    // états du joueur
    private bool crouch;
    private bool shoot;
    private bool droite;
    private int sens; // 1 si droite, -1 sinon
    private bool wallSliding;
    private bool wallCheckFront;
    private bool wallCheckBack;

    // variables des cooldown
    private float timeStampTp;
    private float timeStampDash;
    private float timeStampDamage;
    private float timeStampHeal;

    // caractéristiques de la balle
    public GameObject bullet;
    public float bulletSpeed; // 0.5f est bien

    // éléments extérieurs
    private LayerMask solidMask;
    private LayerMask trapMask;
    private GameObject gameManager;
    private GameManager gameManagerScript;
    public GameObject shield;

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
            if (!Input.GetButton("Fire2")){
                if  (grounded) // Grounded : pour sauter  une première fois il faut que le player soit au sol
                {
                    velocity.y = jumpTakeOffSpeed; // Fait monter = saut
                    jumpCount += 1;
                }
                else if (gameManagerScript.isDoubleJumpOn() && jumpCount<2){ 
                    velocity.y = jumpTakeOffSpeed * 1.1f; // Fait monter = saut
                    jumpCount += 1;
                }
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
        
        // quand la compétence shield est active, le clic droit enfoncé rend le shield actif et immobilise
        if (gameManagerScript.isShieldOn()){
            if (Input.GetButton("Fire2")){
                shield.GetComponent<Collider2D>().enabled = true;
                shield.GetComponent<SpriteRenderer>().enabled = true;
                timeStampDamage = Time.time + 0.2f;
                move.x = 0;
            }
            if (Input.GetButtonUp("Fire2")){
                shield.GetComponent<Collider2D>().enabled = false;
                shield.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        
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

        // quand la compétence wallJump est active, à l'aide de 4 rayons (aux genoux et aux épaules devant et derrière), on test la collision avec le mur
        if (gameManagerScript.isWallJumpOn()){
            if (!grounded){
                RaycastHit2D hit, hit2, hit3, hit4;
                hit = Physics2D.Raycast(transform.position + new Vector3(0,0.5f,0), Vector2.right * sens, 1.0f, solidMask);
                hit2 = Physics2D.Raycast(transform.position + new Vector3(0,0.5f,0), -Vector2.right * sens, 2.0f, solidMask);
                hit3 = Physics2D.Raycast(transform.position - new Vector3(0,0.5f,0), Vector2.right * sens, 1.0f, solidMask);
                hit4 = Physics2D.Raycast(transform.position - new Vector3(0,0.5f,0), -Vector2.right * sens, 2.0f, solidMask);
                wallCheckBack = (hit2.collider != null || hit4.collider != null);
                wallCheckFront = (hit.collider != null || hit3.collider != null);
                if (wallCheckFront || wallCheckBack)
                    handleWallJumping(ref move);
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
                handleDash();
            }
        }

        if (gameManagerScript.isHealOn()){
            if (Input.GetKeyDown("r")){
                handleHeal();
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
        animator.SetBool("Shoot", shoot); 

        targetVelocity = move * maxSpeed * 1.2f; // Fait avancer
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // lors d'une collision avec un piège ou un ennemi, on subit des dégâts et un knockback avec animation (suivi d'1 seconde d'invinsibilité)
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trap") || collision.gameObject.layer == LayerMask.NameToLayer("Ennemy"))
        {
            if (timeStampDamage <= Time.time){
                gameObject.GetComponent<Animation>().Play("Damage_Player");
                if (gameManagerScript.isArmorOn())
                    gameManagerScript.setHealth(gameManagerScript.getHealth() - 15);
                else 
                    gameManagerScript.setHealth(gameManagerScript.getHealth() - 20);
                timeStampDamage = Time.time + 1;
                rb2d.velocity = new Vector2 (0, 0); 
                rb2d.AddForce(new Vector3( -sens * 100, 200, 0), ForceMode2D.Impulse);
            }
        }
        // lors d'une collision avec un trampoline, le joueur est propulsé vers le haut
        if (collision.tag == "Trampoline"){
            rb2d.AddForce(new Vector3(0, 1000, 0), ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trap") || collision.gameObject.layer == LayerMask.NameToLayer("Ennemy"))
        {
            if (timeStampDamage <= Time.time){
                gameObject.GetComponent<Animation>().Play("Damage_Player");
                if (gameManagerScript.isArmorOn())
                    gameManagerScript.setHealth(gameManagerScript.getHealth() - 15);
                else
                    gameManagerScript.setHealth(gameManagerScript.getHealth() - 20);
                timeStampDamage = Time.time + 1;
                rb2d.velocity = new Vector2 (0, 0); 
                rb2d.AddForce(new Vector3( -sens * 100, 200, 0), ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        // lors d'une collision avec une balle adverse, on subit des dégâts et un knockback avec animation (suivi d'1 seconde d'invinsibilité)
        if (collision.gameObject.tag == "YellowBullet"){
            Vector2 contact = collision.contacts[0].point;
            if (timeStampDamage <= Time.time){
                gameObject.GetComponent<Animation>().Play("Damage_Player");
                if (gameManagerScript.isArmorOn())
                    gameManagerScript.setHealth(gameManagerScript.getHealth() - 20);
                else
                    gameManagerScript.setHealth(gameManagerScript.getHealth() - 25);
                timeStampDamage = Time.time + 1;
                rb2d.velocity = new Vector2 (0, 0); 
                if (contact.x < transform.position.x)
                    rb2d.AddForce(new Vector3(100, 200, 0), ForceMode2D.Impulse);
                else 
                    rb2d.AddForce(new Vector3( -100, 200, 0), ForceMode2D.Impulse);
            }
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

    // Le joueur est repoussé à l'opposé du mur tout en sautant.Un double saut est possible après un wallJump
    private void handleWallJumping(ref Vector2 move) {
        rb2d.velocity = new Vector2(rb2d.velocity.x, -0.1f);
        wallSliding = true;
        if (Input.GetButtonDown("Jump")) {
            if (droite && wallCheckFront){
                rb2d.AddForce(new Vector3( -400, 0, 0), ForceMode2D.Impulse);
                Flip();
            }
            else if (!droite && wallCheckBack){
                rb2d.AddForce(new Vector3( -400, 0, 0), ForceMode2D.Impulse);
            }
            else if (!droite && wallCheckFront){
                rb2d.AddForce(new Vector3( 400, 0, 0), ForceMode2D.Impulse);
                Flip();
            }
            else if (droite && wallCheckBack){
                rb2d.AddForce(new Vector3( 400, 0, 0), ForceMode2D.Impulse);
            }
            velocity.y = jumpTakeOffSpeed * 1.2f;
            jumpCount = 1;
        }
    }

    // on utilise 3 rayons (pieds, centre et tête) pour ne pas se téléporter dans le mur, on se téléporte jusqu'au mur
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
            timeStampDamage = Time.time + 0.3f;
            timeStampTp = Time.time + 2;
        }
    }

    // le joueur est propulsé vers l'avant
    private void handleDash(){
        if (timeStampDash <= Time.time){
            rb2d.AddForce(new Vector3(sens * 1000, 0, 0), ForceMode2D.Impulse);
            timeStampDamage = Time.time + 0.3f;
            timeStampDash = Time.time + 2;
        }
    }

    // le joueur peut se redonner de la vie
    private void handleHeal(){
        if (timeStampHeal <= Time.time){
            gameManagerScript.setHealth(gameManagerScript.getHealth() + 30);
            timeStampHeal = Time.time + 40;
        }
    }

    // pour l'animation de shoot
    IEnumerator valueShoot()
    {
        shoot = true;
        yield return new WaitForSeconds(1.0f);
        shoot = false;
    }

}