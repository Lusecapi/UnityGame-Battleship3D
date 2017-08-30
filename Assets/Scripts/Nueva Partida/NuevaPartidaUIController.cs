using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class NuevaPartidaUIController : MonoBehaviour {

    [Header("Paneles")]
    [SerializeField]
    private GameObject panelConfPartida;
    [SerializeField]
    private GameObject panelPosicionamiento;
    [Header("Del Panel de Confirmacion")]
    [SerializeField]
    private Toggle toogleBarcoPrefab;
    [SerializeField]
    private ToggleGroup[] togleGroupTiposBarcos;
    [SerializeField]
    private Dropdown dropDownListDificultad;
    [SerializeField]
    [Header("Del Panel de Posiconamiento")]
    private Button[] botonesBarcos;


    private GameManager gameManager;
    private Dictionary<string, int> barcosRestantes = new Dictionary<string, int>();


    // Use this for initialization
    void Start() {
		
        panelConfPartida.SetActive(true);
		StartCoroutine (WaitForRequest());
        panelPosicionamiento.SetActive(false);
        gameManager = GameObject.FindObjectOfType<GameManager>();
        barcosRestantes.Add("Tipo 1", gameManager.CantidadBarcos1);
        barcosRestantes.Add("Tipo 2", gameManager.CantidadBarcos2);
        barcosRestantes.Add("Tipo 3", gameManager.CantidadBarcos3);
        barcosRestantes.Add("Tipo 4", gameManager.CantidadBarcos4);
    }

    // Update is called once per frame
    void Update() {

        //Debug.Log(togleGroupTiposBarcos[0].GetComponent<RectTransform>().sizeDelta);
	}

    public void prueba(int tipo)
    {
        Toggle tb = Instantiate(toogleBarcoPrefab);
        tb.transform.SetParent(togleGroupTiposBarcos[tipo - 1].transform);
        tb.transform.localScale = Vector3.one;
        tb.group = togleGroupTiposBarcos[tipo - 1];
        togleGroupTiposBarcos[tipo - 1].RegisterToggle(tb);
        togleGroupTiposBarcos[tipo - 1].GetComponent<RectTransform>().sizeDelta = new Vector2(0, Mathf.Abs(tb.GetComponent<RectTransform>().offsetMin.y));
        //togleGroupTiposBarcos[tipo - 1].GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        
    }

    public void Barco1ButtonClick()
    {
        if (barcosRestantes["Tipo 1"] > 0 && gameManager.barcoAUbicar == null)
        {
            barcosRestantes["Tipo 1"]--;
            gameManager.barcoAUbicar = Instantiate(Resources.Load<Barco>("Barcos/"+gameManager.informacionDeLaPartida.BarcosDeLaPartida[0]), new Vector3(1000, 0, 1000), Quaternion.identity);
            if(barcosRestantes["Tipo 1"] == 0)
            {
                botonesBarcos[0].interactable = false;
            }
        }
    }

    public void Barco2ButtonClick()
    {
        if (barcosRestantes["Tipo 2"] > 0 && gameManager.barcoAUbicar == null)
        {
            barcosRestantes["Tipo 2"]--;
            gameManager.barcoAUbicar = Instantiate(Resources.Load<Barco>("Barcos/"+ gameManager.informacionDeLaPartida.BarcosDeLaPartida[1]), new Vector3(1000, 0, 1000), Quaternion.identity);
            if (barcosRestantes["Tipo 2"] == 0)
            {
                botonesBarcos[1].interactable = false;
            }
        }
    }

    public void Barcos3ButtonClick()
    {
        if(barcosRestantes["Tipo 3"] > 0 && gameManager.barcoAUbicar == null)
        {
            barcosRestantes["Tipo 3"]--;
            gameManager.barcoAUbicar = Instantiate(Resources.Load<Barco>("Barcos/"+ gameManager.informacionDeLaPartida.BarcosDeLaPartida[2]), new Vector3(1000, 0, 1000), Quaternion.identity);
            if (barcosRestantes["Tipo 3"] == 0)
            {
                botonesBarcos[2].interactable = false;
            }
        }
    }

    public void Barcos4ButtonClick()
    {
        if(barcosRestantes["Tipo 4"] > 0 && gameManager.barcoAUbicar == null)
        {
            barcosRestantes["Tipo 4"]--;
            gameManager.barcoAUbicar = Instantiate(Resources.Load<Barco>("Barcos/"+ gameManager.informacionDeLaPartida.BarcosDeLaPartida[3]), new Vector3(1000, 0, 1000), Quaternion.identity);
            if (barcosRestantes["Tipo 4"] == 0)
            {
                botonesBarcos[3].interactable = false;
            }
        }
    }

    public void OnEmpezarPartidaButtonClick()
    {
        string[] nombreBarcos = new string[4];
        for (int i = 0; i < togleGroupTiposBarcos.Length; i++)
        {
            nombreBarcos[i] = togleGroupTiposBarcos[i].ActiveToggles().ToArray<Toggle>()[0].GetComponentInChildren<Text>().text;
            Debug.Log(nombreBarcos[i]);
        }


        //Se crea la nueva partida
        gameManager.informacionDeLaPartida = new GameManager.Partida(dropDownListDificultad.value == 0? Dificultad.Facil : Dificultad.Dificil, nombreBarcos);
        panelConfPartida.SetActive(false);
        panelPosicionamiento.SetActive(true);
        Instantiate(gameManager.TableroPrefab).name = "Tablero";

    }

	public void OnRegresarButtonClick(){
		SceneManager.LoadScene ("MenuPrincipal");
	}

    public void OnConfirmarPosicionesButtonClick()
    {
        if(barcosRestantes["Tipo 1"]+ barcosRestantes["Tipo 2"]+ barcosRestantes["Tipo 3"]+ barcosRestantes["Tipo 4"] == 0)
        {
            //Se empieza a jugar
            DontDestroyOnLoad(gameManager.gameObject);
            gameManager.SetForMatch();
            SceneManager.LoadScene("JugarPartida");
        }
        else
        {
            ShowMessage.showMessageText("Debe Uicar todos los barcos en el tablero", MessageType.Information, 4);
        }
    }
	// hace la solicitud al servidor para que le devuelva los barcos que ha comprado el usuario para usarlos luego en la partida
	IEnumerator WaitForRequest(){

		if (Application.internetReachability == NetworkReachability.NotReachable) {//si no hay internet
            ShowMessage.showMessageText("No puede usar sus barcos comprados hasta que vuelva la conexion", MessageType.Information, 4);
			yield break;
		} else {
			for (int i = 0; i <= 3; i++) {
				WWW request2= new WWW("https://battleship3d.herokuapp.com/compras_por_usuario?id="+Usuario.UsuarioConectado.Id+"&size="+(i+1));
				yield return request2;
				string encodedString2 = request2.text;
				string jsonString2=fixJson(encodedString2);
				DecodificarBarco[] barcosComprados = JsonHelper.FromJson<DecodificarBarco> (jsonString2);
				Debug.Log (request2.text);
				foreach (var barcoComprado in barcosComprados) {
					Toggle tb = Instantiate(toogleBarcoPrefab);
					tb.GetComponentInChildren<Text> ().text = barcoComprado.nombre;
					tb.transform.SetParent(togleGroupTiposBarcos[i].transform);
					tb.transform.localScale = Vector3.one;
					tb.group = togleGroupTiposBarcos[i];
					togleGroupTiposBarcos[i].RegisterToggle(tb);
					togleGroupTiposBarcos[i].GetComponent<RectTransform>().sizeDelta = new Vector2(0, Mathf.Abs(tb.GetComponent<RectTransform>().offsetMin.y));
					//Debug.Log (barcoComprado.nombre + ",  " + barcoComprado.size + ",    "+barcoComprado.precio);
				}
			}
		}

	}
	string fixJson(string value)
	{
		value = "{\"Items\":" + value + "}";
		return value;
	}
}
