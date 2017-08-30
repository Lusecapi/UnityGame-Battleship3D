using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
/// <summary>
/// Clase para manajejar la logica de la interfaz de usuario de la Tienda
/// </summary>
public class TiendaUIController : MonoBehaviour {

	public event EventHandler OurEvent;
    [SerializeField]
    private GameObject[] panelesTipoBarcos;
    [SerializeField]
    Transform[] scrollsViewContents;
    [SerializeField]
    private Dropdown dropDownList;

    [SerializeField]
    GameObject moduloBarcoTiendaPrefab;

	int tamBarco;
	bool[] tiposCargados;
    void Start()
    {
		tiposCargados= new bool[4];
		tamBarco = 1;
        //Se verifica la conexion
        activarPanelTipoBarco(0);
		StartCoroutine ("WaitForRequest");

    }

    /// <summary>
    /// Para activar en pantalla el panel de tipo barco indicado
    /// </summary>
    /// <param name="indicePanel">El indice del panel</param>
    private void activarPanelTipoBarco(int indicePanel)
    {
        for (int i = 0; i < panelesTipoBarcos.Length; i++)
        {
            if(i != indicePanel)
            {
                panelesTipoBarcos[i].SetActive(false);
            }
        }
        panelesTipoBarcos[indicePanel].SetActive(true);
    }

    public void OnRegresarButtonClick()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
    

    /// <summary>
    /// Metodo del OnValueChanged del dropDownList
    /// </summary>
    public void OnDropDownLsitValueChanged()
    {
        activarPanelTipoBarco(dropDownList.value);
		switch (dropDownList.value) {
		case 0:
			tamBarco = 1;
			break;
		case 1:
			tamBarco = 2;
			break;
		case 2:
			tamBarco = 3;
			break;
		case 3:
			tamBarco = 4;
			break;
		default:
			break;
		}
		StartCoroutine ("WaitForRequest");
    }

    public void prueba(int tipo)
    {
        GameObject modulo = Instantiate<GameObject>(moduloBarcoTiendaPrefab);
        modulo.transform.SetParent(scrollsViewContents[tipo - 1]);
        modulo.transform.localScale = Vector3.one;
    }

	public void TriggerEvent()
	{
		Debug.Log ("soy feliz me dispare");
	}
	//Aqui se verifica si el id del barco, (El id esta en el gameObject.name) corresponde a un barco que
	//el usuario ya compro enontonces se inhabilita el boton haciendo la peticion al servidor
	IEnumerator WaitForRequest(){

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {//si no hay internet
            GameObject.Find("TextoErrores").GetComponent<Text>().text = "Ha ocurrido un error en el servidor vuelva mas tarde";
            yield break;
        }
        else
        {
            WWW request = new WWW("https://battleship3d.herokuapp.com/barcos?size=" + tamBarco);

            yield return request;
            Debug.Log(request.text);
            string encodedString = request.text;
            string jsonString = fixJson(encodedString);
            DecodificarBarco[] barcos = JsonHelper.FromJson<DecodificarBarco>(jsonString);

            //foreach (var barco in barcos) {
            //Debug.Log(barco.nombre+","+barco.size+","+barco.precio);
            //}
            //Ahora buscamos los barcos que ha comprado el usuario para validar

            WWW request2 = new WWW("https://battleship3d.herokuapp.com/compras_por_usuario?id=" + Usuario.UsuarioConectado.Id + "&size=" + tamBarco);

            yield return request2;
            string encodedString2 = request2.text;
            string jsonString2 = fixJson(encodedString2);

            DecodificarBarco[] barcosComprados = JsonHelper.FromJson<DecodificarBarco>(jsonString2);

            //foreach (var barcoComprado in barcosComprados) {
            //Debug.Log (barcoComprado.nombre + ",  " + barcoComprado.size + ",    "+barcoComprado.precio);
            //}

            ArrayList barcosDisponibles = new ArrayList();
            foreach (var barco in barcos)
            {
                bool encontrado = false;
                foreach (var barcoComprado in barcosComprados)
                {
                    if (barco.id.Equals(barcoComprado.id))
                    {
                        encontrado = true;
                    }
                }
                if (encontrado == false)
                {
                    barcosDisponibles.Add(barco);
                }
            }
            // en la busqueda anterior validamos los barcos que puede comprar el usuario
            //ahora los mostramos en UI y refrescamos
            if (!tiposCargados[tamBarco - 1])
            {
                foreach (var barco in barcosDisponibles)
                {
                    var barcoDecodificado = barco as DecodificarBarco;
                    Debug.Log(barcoDecodificado.nombre + ":" + barcoDecodificado.size + ":" + barcoDecodificado.precio);
                    GameObject m = Instantiate<GameObject>(moduloBarcoTiendaPrefab);
                    m.transform.localScale = Vector3.one;
                    m.transform.SetParent(scrollsViewContents[tamBarco - 1]);
                    ModuloBarcoTienda modulo = m.GetComponent<ModuloBarcoTienda>();

                    modulo.setNombre(barcoDecodificado.nombre);
                    modulo.BarcoID = barcoDecodificado.id;
                    modulo.Precio = barcoDecodificado.precio;
                    modulo.tienda = this;
                }
                tiposCargados[tamBarco - 1] = true;
            }
        }


		
	}
	string fixJson(string value)
	{
		value = "{\"Items\":" + value + "}";
		return value;
	}
}
