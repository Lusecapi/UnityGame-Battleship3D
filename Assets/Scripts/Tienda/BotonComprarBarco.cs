using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class BotonComprarBarco : MonoBehaviour, IPointerClickHandler {



    private ModuloBarcoTienda informacionBarco;
    [SerializeField]
    Text textoPrecio;

    // Use this for initialization
    void Start () {

		GameObject.Find ("TextoDinero").GetComponent<Text> ().text = "Dinero:" + Usuario.UsuarioConectado.Dinero;
        informacionBarco = GetComponentInParent<ModuloBarcoTienda>();
        textoPrecio.text = string.Format("Comprar ({0})", informacionBarco.Precio);
        
	}

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
		StartCoroutine ("WaitForRequest");
        Debug.Log("Comprar barco con id: " + informacionBarco.BarcoID);
    }

	// envia la peticion al servidor para comprar el barco actual
	IEnumerator WaitForRequest(){
		if (Application.internetReachability == NetworkReachability.NotReachable) {//si no hay internet

            ShowMessage.showMessageText("Ha ocurrido un error en el servidor intente comprar mas tarde", MessageType.Error, 4);
			yield break;
		} else {
			WWWForm forma = new WWWForm ();
			forma.AddField ("user_id",Usuario.UsuarioConectado.Id);
			forma.AddField ("barco_id",informacionBarco.BarcoID);
			WWW logear= new WWW("https://battleship3d.herokuapp.com/comprar_barco",forma);

			yield return logear;
			//Debug.Log (logear.text);
            //ShowMessage.showMessageText("No tienes suficiente dinero para comprar este barco", MessageType.Information);
			string encodedString = logear.text;
			if (logear.error==null) {
				if(informacionBarco.tienda != null)
				{
					informacionBarco.tienda.TriggerEvent ();
				}
				GameObject.Find ("TextoDinero").GetComponent<Text> ().text = "Dinero:" + (Usuario.UsuarioConectado.Dinero-informacionBarco.Precio);
				Debug.Log ("Felicidades Has comprado este barco");
                ShowMessage.showMessageText("Felicidades Has comprado este barco", MessageType.Confirmation);
                Destroy(informacionBarco.gameObject);
			} else {

                ShowMessage.showMessageText("No tienes suficiente dinero para comprar este barco", MessageType.Information);
                Debug.Log (logear.error);

			}
		}

	}
}
