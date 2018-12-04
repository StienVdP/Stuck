using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawBehaviour : MonoBehaviour {

	public GameObject axis;
    public float moveSpeed;
    public float left_bottom;
    public float right_top;
	private float maxRight;
	private float maxLeft;
    private float maxTop;
	private float maxBottom;
    bool moveRight = true;
	
    bool moveUp = false;
	// Use this for initialization
	void Start () {
		maxRight = transform.position.x + right_top;
		maxLeft = transform.position.x - left_bottom;
        maxTop = transform.position.y + right_top;
		maxBottom = transform.position.y - left_bottom;
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

            if (transform.position.x > maxRight) { 
			    moveRight = false;
            }
            if (transform.position.x < maxLeft) {
                moveRight = true;
            }
        }

        if (gameObject.tag == "VerticalSaw")
        {
            if (moveUp){
                transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
                axis.transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
            }
            else{
                transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
                axis.transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
            }

            if (transform.position.y > maxTop) { 
			    moveUp = false;
            }
            if (transform.position.y < maxBottom) {
                moveUp = true;
            }
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
