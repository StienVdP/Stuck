using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour {

    void Awake()
    {
        Debug.Log("optionShoot est a "+ ApplicationModel.optionShoot);
        Debug.Log("optionDoubleJump est a " + ApplicationModel.optionShoot);
        if (ApplicationModel.optionShoot == true)
        {
            //
            Debug.Log("optionShoot a été debloquer !");

        }
        if(ApplicationModel.optionDoubleJump == true)
        {
            //
            Debug.Log("optionDoubleJump a été debloquer !");
        }
    }

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
