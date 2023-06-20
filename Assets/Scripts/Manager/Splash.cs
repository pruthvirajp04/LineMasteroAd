using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Splash : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(GotoNext());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator GotoNext()
    {
        yield return new WaitForSeconds(0.25f);
        SceneManager.LoadScene(1);
    }
}
