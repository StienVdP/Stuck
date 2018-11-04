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
    private bool flipSprite;
    private bool droite;
    private int compt;

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
            StartCoroutine(valueShoot(move.x));
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            crouch = true;
            GetComponent<BoxCollider2D>().offset.Equals(new Vector2(0.52f, 0.26f));
            GetComponent<BoxCollider2D>().size.Set(2.8f, 4.25f);
            Debug.Log("Offset :"+GetComponent<BoxCollider2D>().offset);
            // Regler le box collider Offset x : 0.52 et y : 0.26 Size x : 2.8 et y : 4.25
        }
        else crouch = false;

        flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f)); // Flip le sprit quand on va dans l'autre direction
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
        Debug.Log("Flip : " + flipSprite + " SpriteRenderer : "+ spriteRenderer.flipX);


        animator.SetBool("Ground", grounded);
        animator.SetFloat("Speed", Mathf.Abs(velocity.x) / maxSpeed);
        animator.SetBool("Crouch", crouch); 
        animator.SetBool("Shoot", shoot); // Peut etre mettre dans un Update ? pour un shoot continue
        Debug.Log("Shoot : " + shoot);

        targetVelocity = move * maxSpeed; // Fait avancer
    }

    IEnumerator valueShoot(float direction)
    {
        shoot = true;
        // Creat Bullet Object suivant la direction (move.x > 0.01f) ou (move.x < 0.01f)
        yield return new WaitForSeconds(1.0f);
        shoot = false;
    }
}