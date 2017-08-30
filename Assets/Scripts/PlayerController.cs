using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    GameManager gameManager;

	// Use this for initialization
	void Start () {

        gameManager = GameObject.FindObjectOfType<GameManager>();
	}

    // Update is called once per frame
    void Update()
    {

        if (gameManager.informacionDeLaPartida.EstadoActual == EstadoDeLaPartida.UbicandoBarcos)
        {

            if (gameManager.barcoAUbicar)
            {
                if (Input.GetKeyDown(KeyCode.H) && gameManager.barcoAUbicar.instanciaDelBarco.orientacion == OrientacionBarco.Vertical)
                {
                    gameManager.cambiarOrientacionDelBarco(gameManager.barcoAUbicar, OrientacionBarco.Horizontal);
                }
                else
                    if (Input.GetKeyDown(KeyCode.V) && gameManager.barcoAUbicar.instanciaDelBarco.orientacion == OrientacionBarco.Horizontal)
                {
                    gameManager.cambiarOrientacionDelBarco(gameManager.barcoAUbicar, OrientacionBarco.Vertical);
                }
            }
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Q))
        {
            Camera.main.transform.Rotate(0, -40 * Time.deltaTime, 0);
        }
        else
            if (Input.GetKey(KeyCode.E))
        {
            Camera.main.transform.Rotate(0, 40 * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.R))
        {
            Camera.main.transform.Rotate(40 * Time.deltaTime, 0, 0);
        }
        else
            if (Input.GetKey(KeyCode.F))
        {
            Camera.main.transform.Rotate(-40 * Time.deltaTime, 0, 0);
        }

        Camera.main.transform.Translate(h, 0, v);

    }
}
