using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		
	}
	
    public void OnExitButtonDown()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
