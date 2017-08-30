using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Inicial : MonoBehaviour {


	//si presiona cualquier tecla es redirigido al sistema login
	void Update () {
		if (Input.anyKeyDown) {
			SceneManager.LoadScene ("Login");
		}
	}
}
