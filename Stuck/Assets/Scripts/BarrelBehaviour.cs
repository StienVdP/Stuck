using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelBehaviour : MonoBehaviour {

    private Animator animator;

    // Use this for initialization
    void Start ()
    {
        animator = this.GetComponent<Animator>();
        animator.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Bullet")
        {
            // Explosion
            animator.enabled = true;
            animator.SetBool("explode", true);
        }
    }

    // Animation Event
    void OnEndOfExplosion()
    {
        Destroy(gameObject); 
    }
}
