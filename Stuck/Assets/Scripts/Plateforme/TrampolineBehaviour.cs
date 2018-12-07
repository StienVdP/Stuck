using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineBehaviour : MonoBehaviour {

	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player"){
			animator.SetBool("action",true);
		}
	}

	private void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Player"){
			animator.SetBool("action",false);
		}
	}
}
