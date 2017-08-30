using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Struct Coordenada (Creacion de la variable coordenada)
/// </summary>
[System.Serializable]
public struct Coordenada
{
    [SerializeField]
    private string fila;
    [SerializeField]
    private int columna;

    /// <summary>
    /// Obtener la fila, dado el equivalente numerico desde A hasta la J: [0,9]
    /// </summary>
    public static string[] FILAS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

    public Coordenada(string _fila, int _columna)
    {
        this.fila = _fila;
        this.columna = _columna;
    }

    #region Encapsulado
    public string Fila
    {
        get
        {
            return fila;
        }

        set
        {
            fila = value;
        }
    }

    public int Columna
    {
        get
        {
            return columna;
        }

        set
        {
            columna = value;
        }
    }
    #endregion

    /// <summary>
    /// Convierte la Fila de la coordenada en su equivalente numerico
    /// </summary>
    /// <returns>El equivalete numerico de la fila (incluye el 0=A)</returns>
    public int FilaToNum()
    {
        if(this.fila == "A")
        {
            return 0;
        }
        else
            if(this.fila == "B")
        {
            return 1;
        }
        else
            if(this.fila == "C")
        {
            return 2;
        }
        else
            if(this.fila == "D")
        {
            return 3;
        }
        else
            if (this.fila == "E")
        {
            return 4;
        }
        else
            if (this.fila == "F")
        {
            return 5;
        }
        else
            if (this.fila == "G")
        {
            return 6;
        }
        else
            if (this.fila == "H")
        {
            return 7;
        }
        else
            if (this.fila == "I")
        {
            return 8;
        }
        else
            if (this.fila == "J")
        {
            return 9;
        }
        else
        {
            Debug.Log("Error En la Coordenada, No Existe");
            return -1;
        }
    }

    public override string ToString()
    {
        return string.Format("{0}{1}", this.fila, this.columna);
    }
}

/// <summary>
/// Tipos o formas que se pueden pontar la zona del mar
/// </summary>
public enum TipoDePintado
{
    SePuedeUbicar,//Pinta de verde
    NoSePuedeUbicar,//Pinta de rojo
    Restablecer//Pinta de blanco
}

/// <summary>
/// Zona del mar o Tile en el World Space
/// </summary>
public class ZonaDelMar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    public Coordenada coordenada;
    private GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (gameManager.informacionDeLaPartida.EstadoActual == EstadoDeLaPartida.UbicandoBarcos)
        {
            if (gameManager.barcoAUbicar)
            {
                gameManager.barcoAUbicar.transform.position = new Vector3(this.transform.position.x, gameManager.barcoAUbicar.transform.position.y, this.transform.position.z);
                gameManager.barcoAUbicar.instanciaDelBarco.coordenadaDeReferencia = this.coordenada;
                gameManager.verificarUbicacionBarco(gameManager.barcoAUbicar);
            }
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (gameManager.informacionDeLaPartida.EstadoActual == EstadoDeLaPartida.UbicandoBarcos)
        {
            if (gameManager.barcoAUbicar)
            {
                Barco barco = gameManager.barcoAUbicar;
                gameManager.verificarUbicacionBarco(barco, true);
            }
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (gameManager.informacionDeLaPartida.EstadoActual == EstadoDeLaPartida.UbicandoBarcos)
        {
            if (gameManager.barcoAUbicar)
            {
                if (gameManager.barcoAUbicar.sePuedeUbicar)
                {
                    gameManager.ubicarBarco(gameManager.barcoAUbicar);
                }
            }
        }
        else
            if(gameManager.informacionDeLaPartida.EstadoActual == EstadoDeLaPartida.Jugando && gameManager.turnoDelUsuario)
        {
            if(transform.parent.name == "TableroEnemigo")//Quiere decir que solo podemos hacer click en las zonas del mar enemigo mientras se esta jugando
            {
                if (!gameManager.tableroDisparosUsuario[coordenada.FilaToNum(), coordenada.Columna])
                {
                    gameManager.Disparar(this.coordenada);
                }
            }
        }
    }
}
