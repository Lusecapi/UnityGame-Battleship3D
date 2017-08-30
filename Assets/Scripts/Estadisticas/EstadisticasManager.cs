using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EstadisticasManager : MonoBehaviour {

    [SerializeField]
    RectTransform PanelNombres;
    [SerializeField]
    RectTransform PanelNumeros;
    [SerializeField]
    Text textoEstadisticasPrefab;

	//para regresar a menu principal
	public void OnRegresarButtonClick(){
		SceneManager.LoadScene ("MenuPrincipal");
	}
	//para obtener el top 10 por victorias
	public void obtenerTopVictorias(){
        destruirTextosEstadisticas();
		StartCoroutine (WaitForRequest());
	}
	// para obtener el top 10 por efectividad
	public void obtenerTopEfectividad(){

        destruirTextosEstadisticas();
		StartCoroutine (WaitForRequest2());
	}

	//para refrescar si se ejecuta alguno de los dos eventos nuevamente
    private void destruirTextosEstadisticas()
    {
        GameObject[] textos = GameObject.FindGameObjectsWithTag("TextoEstadisticas");
        foreach (GameObject texto in textos)
        {
            Destroy(texto);
        }
    }

	// para mostrarlos en forma de tabla
    private void UbicarTextoEstadistica(string texto, bool esNombre)
    {
        var t = Instantiate(textoEstadisticasPrefab);
        t.transform.localScale = Vector2.one;
        if (esNombre)
        {
            t.transform.SetParent(PanelNombres);
        }
        else
        {
            t.transform.SetParent(PanelNumeros);
        }
        t.text = texto;
    }

	// hace la peticion top 10 por partidas ganadas
	IEnumerator WaitForRequest(){

		if (Application.internetReachability == NetworkReachability.NotReachable) {//si no hay internet
            ShowMessage.showMessageText("Ha ocurrido un error en el servidor vuelva mas tarde", MessageType.Error);

            yield break;
		} else {
			WWW request= new WWW("https://battleship3d.herokuapp.com/obtener_top?cantidad=10");

			yield return request;
			Debug.Log (request.text);
			string encodedString = request.text;
			string jsonString = fixJson(encodedString);
			DecodificarEstadistica[] estadisticas = JsonHelper.FromJson<DecodificarEstadistica>(jsonString);
			foreach (var estadistica in estadisticas) {

				Debug.Log(estadistica.username+","+estadistica.partidas_ganadas);

                UbicarTextoEstadistica(estadistica.username, true);
                UbicarTextoEstadistica(estadistica.partidas_ganadas.ToString(), false);
            }
		}


	}
	// hace la peticion del top 10 por efectividad
	IEnumerator WaitForRequest2(){

		if (Application.internetReachability == NetworkReachability.NotReachable) {//si no hay internet
			GameObject.Find ("TextoErrores").GetComponent<Text> ().text = "Ha ocurrido un error en el servidor vuelva mas tarde";
			yield break;
		} else {
			WWW request= new WWW("https://battleship3d.herokuapp.com/obtener_top_efectividad?cantidad=10");

			yield return request;
			Debug.Log (request.text);
			string encodedString = request.text;
			string jsonString = fixJson(encodedString);
			DecodificarEstadistica[] estadisticas = JsonHelper.FromJson<DecodificarEstadistica>(jsonString);
			foreach (var estadistica in estadisticas) {
				Debug.Log(estadistica.username+","+estadistica.efectividad);

                UbicarTextoEstadistica(estadistica.username, true);
                UbicarTextoEstadistica(estadistica.efectividad.ToString(), false);
            }
		}



	}
	//un helper para json
	string fixJson(string value)
	{
		value = "{\"Items\":" + value + "}";
		return value;
	}
}
