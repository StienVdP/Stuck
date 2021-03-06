﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformeBehaviour : MonoBehaviour {

    public GameObject player;
    public bool grouned;

    public float moveSpeed;
    private bool moveRight = false;
    private bool moveUp = false;

    // Use this for initialization
    void Start () {
		player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "HorizontalPlateform")
        {
            if (moveRight)
                transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);

            else
                transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }

        if (gameObject.tag == "VerticalPlateform")
        {
            if (moveUp)
                transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
            else
                transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
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
        
        if (collision.gameObject.tag == "Player")
        {
            player.transform.parent = transform;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            player.transform.parent = null;
        }
    }
}
