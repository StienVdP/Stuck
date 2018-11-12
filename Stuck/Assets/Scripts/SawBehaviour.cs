using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBehaviour : MonoBehaviour {

	public GameObject axis;
    public float moveSpeed;
	private float maxRight;
	private float maxLeft;
    bool moveRight = true;
	
    bool moveUp = false;
	// Use this for initialization
	void Start () {
		maxRight = transform.position.x + 4.5f;
		maxLeft = transform.position.x - 4.5f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.forward * -15);
        if (gameObject.tag == "HorizontalSaw")
        {
            if (moveRight){
                transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
				axis.transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
			}

            else{
                transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
				axis.transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
			}
        }

        if (gameObject.tag == "VerticalSaw")
        {
            if (moveUp)
                transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
            else
                transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
		}
		if (transform.position.x > maxRight || transform.position.x < maxLeft){
			moveRight = !moveRight;
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HorizontalSlider")
        {
            moveRight = !moveRight;
        }

        if (collision.gameObject.tag == "VerticalSlider")
        {
            moveUp = !moveUp;
        }
	}
}
