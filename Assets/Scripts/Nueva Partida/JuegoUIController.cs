using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class JuegoUIController : MonoBehaviour {

    [Header("Paneles")]
    [SerializeField]
    private GameObject panelConfPartida;
    [SerializeField]
    private GameObject panelPosicionamiento;
    [Header("Del Panel de Conf")]
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
	void Start () {

        panelConfPartida.SetActive(true);
        panelPosicionamiento.SetActive(false);
        gameManager = GameObject.FindObjectOfType<GameManager>();
        barcosRestantes.Add("Tipo 1", gameManager.CantidadBarcos1);
        barcosRestantes.Add("Tipo 2", gameManager.CantidadBarcos2);
        barcosRestantes.Add("Tipo 3", gameManager.CantidadBarcos3);
        barcosRestantes.Add("Tipo 4", gameManager.CantidadBarcos4);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Barco1ButtonClick()
    {
        if (barcosRestantes["Tipo 1"] > 0 && gameManager.barcoAUbicar == null)
        {
            barcosRestantes["Tipo 1"]--;
            gameManager.barcoAUbicar = Instantiate(Resources.Load<Barco>("Barcos/Barco1"), new Vector3(1000, 0, 1000), Quaternion.identity);
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
            gameManager.barcoAUbicar = Instantiate(Resources.Load<Barco>("Barcos/Barco2"), new Vector3(1000, 0, 1000), Quaternion.identity);
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
            gameManager.barcoAUbicar = Instantiate(Resources.Load<Barco>("Barcos/Barco3"), new Vector3(1000, 0, 1000), Quaternion.identity);
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
            gameManager.barcoAUbicar = Instantiate(Resources.Load<Barco>("Barcos/Barco4"), new Vector3(1000, 0, 1000), Quaternion.identity);
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

        gameManager.informacionDeLaPartida = new GameManager.Partida(dropDownListDificultad.value == 0? Dificultad.Facil : Dificultad.Dificil, nombreBarcos);
        panelConfPartida.SetActive(false);
        panelPosicionamiento.SetActive(true);
        Instantiate(gameManager.EscenarioPrefab);

    }

    public void OnConfirmarPosicionesButtonClick()
    {
        if(barcosRestantes["Tipo 1"]+ barcosRestantes["Tipo 2"]+ barcosRestantes["Tipo 3"]+ barcosRestantes["Tipo 4"] == 0)
        {
            //Empieza el juego
        }
        else
        {
            Debug.Log("Debe Uicar todos los barcos en el tablero");
        }
    }
}
