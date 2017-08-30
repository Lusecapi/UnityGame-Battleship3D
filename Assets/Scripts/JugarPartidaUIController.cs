using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JugarPartidaUIController : MonoBehaviour
{

    [SerializeField]
    Text misBarcosRestantesText;
    [SerializeField]
    Text barcosRestantesEnemigo;
    private string status;

    GameManager gameManager;
    // Use this for initialization
    void Start()
    {
		AudioSource audio = GetComponent<AudioSource> ();
		audio.Play ();
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

        misBarcosRestantesText.text = string.Format("Mis Barcos Restantes: {0}", gameManager.informacionDeLaPartida.barcosUsuario.Count);
        barcosRestantesEnemigo.text = string.Format("Barcos REstantes Enemigos: {0}", gameManager.informacionDeLaPartida.barcosIA.Count);
    }

    public void OnAbandonarPartidaButtonClick()
    {
        StartCoroutine(WaitForRequest());
    }

    IEnumerator WaitForRequest()
    {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {//si no hay internet
            ShowMessage.showMessageText("Te Salvaste", MessageType.Information);
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("MenuPrincipal");

        }
        else
        {
            status = "perdio";
            WWWForm forma = new WWWForm();
            forma.AddField("id", Usuario.UsuarioConectado.Id);
            forma.AddField("estado", status);
            forma.AddField("dinero", 0);
            WWW request = new WWW("https://battleship3d.herokuapp.com/terminar_partida", forma);

            yield return request;

            string encodedString = request.text;
            if (request.error == null)
            {
                Debug.Log("DATOS EN EL SERVIDOR ACTUALIZADOS CORRECTAMENTE");
            }
            SceneManager.LoadScene("MenuPrincipal");

        }
    }
}
