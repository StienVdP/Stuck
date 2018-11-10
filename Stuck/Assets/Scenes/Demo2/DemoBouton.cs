using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoBouton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnOption1Click()
    {
        ApplicationModel.optionShoot = true;
        SceneManager.LoadScene("Test2");
    }

    public void OnOption2Click()
    {
        ApplicationModel.optionDoubleJump = true;
        SceneManager.LoadScene("Test2");
    }
}
