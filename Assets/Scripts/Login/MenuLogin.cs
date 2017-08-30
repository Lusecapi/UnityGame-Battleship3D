using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuLogin : MonoBehaviour {


	public GameObject panelLogin;
	public GameObject panelRegistro;

	///public Text mainText;
	//public Text startText;
	// Use this for initialization
	void Start () {
		panelLogin.SetActive (true);
		panelRegistro.SetActive (false);

	}

	void Update(){
		if (Input.anyKey) {
			//mainText.text = "";
			//startText.text = "";
			//panelLogin.SetActive (true);
		}
	}
	
	public void irPanelRegistro(){
		if (panelRegistro.activeInHierarchy==false) {
			panelLogin.SetActive (false);
			panelRegistro.SetActive (true);
		}
	}

	public void irPanelLogin(){
		if (panelLogin.activeInHierarchy==false) {
			panelRegistro.SetActive (false);
			panelLogin.SetActive (true);

		}
	}

}
