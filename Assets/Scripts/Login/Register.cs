using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class Register : MonoBehaviour {

	public GameObject nombre;
	public GameObject apellido;
	public GameObject username;
	public GameObject correo;
	public GameObject password;

	//dispara el evento registrar al accionar el boton registrar
	public void Registrar(){

		if (nombre.GetComponent<InputField>().text=="" && apellido.GetComponent<InputField>().text=="" && username.GetComponent<InputField>().text=="" && correo.GetComponent<InputField>().text=="" && password.GetComponent<InputField>().text=="") {
			ShowMessage.showMessageText ("No se permiten campos vacios",MessageType.Error,4);
			return;
		}
		if (nombre.GetComponent<InputField>().text=="") {
			ShowMessage.showMessageText ("El campo del nombre no puede estar vacio",MessageType.Error);
		
			return;
		} 
		if (apellido.GetComponent<InputField> ().text=="") {
			ShowMessage.showMessageText ("El campo del apellido no puede estar vacio",MessageType.Error);
		
			return;
		}
		if (username.GetComponent<InputField>().text=="") {
			ShowMessage.showMessageText ("El campo del username no puede estar vacio",MessageType.Error);
		
			return;
		} 
		if (correo.GetComponent<InputField> ().text=="") {
			ShowMessage.showMessageText ("El campo del correo no puede estar vacio",MessageType.Error);
		
			return;
		}
		if (password.GetComponent<InputField> ().text=="") {
			ShowMessage.showMessageText ("El campo de la clave no puede estar vacio",MessageType.Error);

			return;
		}
		StartCoroutine("WaitForRequest");
	}

	//Esto invoca la peticion de registrar el nuevo usuario en el sistema
	IEnumerator WaitForRequest()
	{
		if (Application.internetReachability == NetworkReachability.NotReachable) {//si no hay internet
			
			ShowMessage.showMessageText ("Se perdio la conexion a internet no se ha podido registrar",MessageType.Error);
			yield break;
		} else {
			string name = nombre.GetComponent<InputField> ().text;
			string lastname = apellido.GetComponent<InputField>().text;
			string user = username.GetComponent<InputField> ().text;
			string email = correo.GetComponent<InputField> ().text;
			string pass = password.GetComponent<InputField> ().text;
			WWWForm forma = new WWWForm ();
			forma.AddField ("nombre", name);
			forma.AddField ("apellido",lastname);
			forma.AddField ("username",user);
			forma.AddField ("email",email);
			forma.AddField ("password",pass);
			WWW registrar= new WWW("https://battleship3d.herokuapp.com/crear_usuario",forma);
			Debug.Log (user + ";" + email + ";" + pass);
			yield return registrar;

			//control de errores
			if(registrar.error == null)
			{
				Debug.Log("Registro exitoso: " + registrar.text);

			} 
			else 
			{
				ShowMessage.showMessageText("El nombre de usuario ya existe", MessageType.Error,4);
				Debug.Log("WWW Error: "+ registrar.error);
			}    
		}

	}
}
