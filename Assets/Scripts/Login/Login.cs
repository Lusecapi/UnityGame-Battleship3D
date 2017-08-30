using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Login : MonoBehaviour {

    [SerializeField]
	private InputField userName;
    [SerializeField]
	public InputField password;

	DecodificarJSON deco;

	public void login(){
		if (userName.text=="" && password.text=="") {
			ShowMessage.showMessageText ("No se permiten campos vacios",MessageType.Error,4);
			return;
		}
		if (userName.text=="") {
			ShowMessage.showMessageText ("El campo del username clave no puede estar vacia",MessageType.Error,4);
			return;
		}
		if (password.text=="") {
			ShowMessage.showMessageText ("El campo de la clave no puede estar vacia",MessageType.Error,4);
			return;
		}
		StartCoroutine (WaitForRequest());
	}
	//hace la solicitud de login
	IEnumerator WaitForRequest(){

		if (Application.internetReachability == NetworkReachability.NotReachable) {//si no hay internet
            ShowMessage.showMessageText("NO hay conexion a internet Intente jugar mas tarde", MessageType.Error);
			yield break;
		} else {
			
			WWWForm forma = new WWWForm ();
			forma.AddField ("username",userName.text);
			forma.AddField ("password", password.text);
			WWW logear= new WWW("https://battleship3d.herokuapp.com/verificar_usuario",forma);

			yield return logear;

			string encodedString = logear.text;

			if (logear.error==null) {
                //decouser=DecodificarUsuario.CreateFromJSON (encodedString);
                Usuario.UsuarioConectado = JsonUtility.FromJson<Usuario>(encodedString);
				SceneManager.LoadScene ("MenuPrincipal");
			} else {
				string myString = "Creedenciales Invalidas";
				deco= DecodificarJSON.CreateFromJSON (encodedString);
				Debug.Log (logear.error);
                ShowMessage.showMessageText(myString, MessageType.Error);
		
			}
		}



	}

}
