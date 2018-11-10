using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationModel : MonoBehaviour {

    static public bool optionShoot;    // this is reachable from everywhere
    static public bool optionDoubleJump;    // this is reachable from everywhere

    void Awake()
    {
        optionShoot = false;
        optionDoubleJump = false;
    }
    
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
