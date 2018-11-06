using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public float maxSpeed = 100;
    public float jumpTakeOffSpeed = 100;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rigidbody;

    private bool crouch;
    private bool shoot;
    private bool droite;

    public GameObject bullet;
    public float bulletSpeed; // 0.5f est bien

    // Use this for initialization
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        droite = true;
    }


    protected override void ComputeVelocity()
    {
        Vector2 move = Vector2.zero;

        move.x = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && grounded) // Grounded : pour sauter il faut que le player soit au sol => pas de double saut
        {
            velocity.y = jumpTakeOffSpeed; // Fait monter = saut
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f; // Fait re dessandre = fin du saut
            }
        }

        if (Input.GetButtonDown("Fire1")) // Shoot
        {
            var position = this.transform.position;
            if (droite == true)
            {
                bullet.GetComponent<ShootManagement>().direction = bulletSpeed;
            } else
            {
                bullet.GetComponent<ShootManagement>().direction = -bulletSpeed;
            }
            Instantiate(bullet, position, new Quaternion());
            StartCoroutine(valueShoot(move.x));
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            crouch = true;
            // Ne marche pas A faire ! :
            /*
            GetComponent<BoxCollider2D>().offset.Equals(new Vector2(0.52f, 0.26f));
            GetComponent<BoxCollider2D>().size.Set(2.8f, 4.25f);
            Debug.Log("Offset :"+GetComponent<BoxCollider2D>().offset);*/
            // Regler le box collider Offset x : 0.52 et y : 0.26 Size x : 2.8 et y : 4.25
        }
        else crouch = false;
        
        /* Si flip.x = true -> gauche else droite */
        if (move.x >= 0.00f)
        {
            droite = true;
            spriteRenderer.flipX = false;
        }
        else if (move.x < 0.00f)
        {
            droite = false;
            spriteRenderer.flipX = true;
        }

        Debug.Log("droite : " + droite);


        animator.SetBool("Ground", grounded);
        animator.SetFloat("Speed", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetBool("Crouch", crouch); 
        animator.SetBool("Shoot", shoot); // Peut etre mettre dans un Update ? pour un shoot continue

        targetVelocity = move * maxSpeed; // Fait avancer
    }



    IEnumerator valueShoot(float direction)
    {
        shoot = true;
        yield return new WaitForSeconds(1.0f);
        shoot = false;
    }
}