using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ConfiguracionCuenta : MonoBehaviour {


	public GameObject antiguaPass;
	public GameObject nuevaPass;

	public void ActualizarPassword(){

		if (antiguaPass.GetComponent<InputField>().text=="" && nuevaPass.GetComponent<InputField>().text=="") {
			ShowMessage.showMessageText ("No se permiten campos vacios",MessageType.Error,4);
			return;
		}
		if (antiguaPass.GetComponent<InputField>().text=="") {
			ShowMessage.showMessageText ("El campo de la antigua clave no puede estar vacia",MessageType.Error,4);
			return;
		} 
		if (nuevaPass.GetComponent<InputField> ().text=="") {
			ShowMessage.showMessageText ("El campo de la nueva clave no puede estar vacia",MessageType.Error,4);
			return;
		}
		StartCoroutine ("WaitingForRequest");
	}

	IEnumerator WaitingForRequest(){
		if (Application.internetReachability == NetworkReachability.NotReachable) {//si no hay internet
			GameObject.Find ("TextoErrores").GetComponent<Text> ().text = "Ha ocurrido un error en el servidor intentelo mas tarde";
			yield break;
		} else {
			WWWForm forma = new WWWForm ();
			forma.AddField ("id",Usuario.UsuarioConectado.Id);
			forma.AddField ("passwordvieja", antiguaPass.GetComponent<InputField>().text);
			forma.AddField ("passwordnueva", nuevaPass.GetComponent<InputField> ().text);
			WWW request = new WWW ("https://battleship3d.herokuapp.com/actualizar_password", forma);

			yield return request;
			if (request.error==null) {
                ShowMessage.showMessageText("Cambio de clave exitoso");
			} else {
				Debug.Log (request.text);
			}
		}

	}

	public void OnRegresarButtonClick(){
		SceneManager.LoadScene ("MenuPrincipal");
	}
}
