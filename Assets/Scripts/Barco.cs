using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Orientaciones del Barco
/// </summary>
public enum OrientacionBarco
{
    Vertical,
    Horizontal
}


/// <summary>
/// /Contiene la informacion general del barco.
/// Esta información es dada por el desarrollador en el Inspector de Unity
/// Es de solo lectura
/// </summary>
[Serializable]
public class InformacionBarco
{
    [SerializeField]
    int id;
    [SerializeField]
    string nombre;
    [SerializeField]
    int longitud;
    [SerializeField]
    int precio;

    public int Id
    {
        get
        {
            return id;
        }
    }

    public string Nombre
    {
        get
        {
            return nombre;
        }
    }

    public int Longitud
    {
        get
        {
            return longitud;
        }
    }

    public int Precio
    {
        get
        {
            return precio;
        }
    }
}

/// <summary>
/// Contiene la informacion sobre el barco instanciado en el world space
/// Se usa está clase y no la Monobehaviour, debido a que en la segunda
/// la informacion solo existe mientras exista la instancia en el world space,
/// creando una clase no monobehaviour se puede obtener la información, aun despues
/// de destruir la instancia del barco.
/// </summary>
[Serializable]
public class InstanciaDeBarco
{
    public OrientacionBarco orientacion;
    public Coordenada coordenadaDeReferencia;
    public Coordenada[] coordenadasOcupadas;
    public int golpesRestantes;

    /// <summary>
    /// Metodo constructor de una instancia
    /// </summary>
    /// <param name="longitud">longitud del barco</param>
    /// <param name="orientacion">orientacion del barco</param>
    /// <param name="coordenadaRef">coordenada de referencia del barco</param>
    public InstanciaDeBarco(int longitud, OrientacionBarco orientacion, Coordenada coordenadaRef)
    {
        this.orientacion = orientacion;
        this.coordenadaDeReferencia = coordenadaRef;
        this.golpesRestantes = longitud;
    }
}


/// <summary>
/// Barco durante la partida, Contiene la informacion strictamente necesaria en la partida.
/// </summary>
[Serializable]
public class BarcoEnPartida
{
    public string nombre;
    public int longitud;
    public InstanciaDeBarco infoInstancia;

    public BarcoEnPartida(string nombre, int longitud, InstanciaDeBarco instancia)
    {
        this.nombre = nombre;
        this.longitud = longitud;
        this.infoInstancia = instancia;
        this.infoInstancia.golpesRestantes = longitud;
    }

    public BarcoEnPartida(string nombre, int longitud, OrientacionBarco orientacion, Coordenada coordenadaDeRef)
    {
        this.nombre = nombre;
        this.longitud = longitud;
        this.infoInstancia = new InstanciaDeBarco(longitud, orientacion, coordenadaDeRef);
    }
}

/// <summary>
/// Clase monobehaviour de los barcos, se usa cuando se están posicionando los barcos
/// </summary>
public class Barco : MonoBehaviour, IPointerClickHandler {

    [Header("Información General Sobre el Barco")]
    public InformacionBarco informacionBarco;

    [Header("El Barco en la Partida")]
    public InstanciaDeBarco instanciaDelBarco;

    public bool sePuedeUbicar;
    public bool ubicado;

    private GameManager gameManager;

    // Use this for initialization
    void Start () {

        gameManager = GameManager.FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Cuando se le hace click a un barco, solo funciona cuando se esta en la etapa de ubicando los barcos
    /// Cuando se quiere reposicionar
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (gameManager.informacionDeLaPartida.EstadoActual == EstadoDeLaPartida.UbicandoBarcos)
        {
            if (this.ubicado && gameManager.barcoAUbicar == null)
            {
                gameManager.reUbicarBarco(this);
            }
        }
    }
}
