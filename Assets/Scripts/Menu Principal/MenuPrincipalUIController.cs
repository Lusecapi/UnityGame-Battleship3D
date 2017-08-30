using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalUIController : MonoBehaviour {

	void Start(){
		//Debug.Log (Usuario.UsuarioConectado.Nombre);
	}

    public void OnJugarButtonClick()
    {
        SceneManager.LoadScene("NuevaPartida");
    }

    public void OnTiendaButtonClick()
    {
        SceneManager.LoadScene("Tienda");
    }

	public void OnEstadisticasButtonClick(){
		SceneManager.LoadScene ("Estadisticas");
	}

	public void OnConfiguracionDeCuentaButtonClick(){
		SceneManager.LoadScene ("ConfiguracionDeCuenta");
	}

	public void OnRegresarButtonClick(){
		SceneManager.LoadScene ("Login");
	}
}
