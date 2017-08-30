using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Nivel de dificultad
/// </summary>
public enum Dificultad
{
    Facil = 0,
    Dificil = 1
}

/// <summary>
/// El estado del jugador de la partida
/// </summary>
public enum EstadoDeLaPartida
{
    UbicandoBarcos,
    Jugando
}

/// <summary>
/// La Clase que controla y tiene toda la informacion de la partida
/// </summary>
public class GameManager : MonoBehaviour {

    /// <summary>
    /// Contiene la informacion de la partida
    /// </summary>
    [System.Serializable]
    public class Partida
    {
        [SerializeField]
        private Dificultad dificultad;
        [SerializeField]
        private string[] barcosDeLaPartida = new string[4];
        [SerializeField]
        private EstadoDeLaPartida estadoActual;

        public List<BarcoEnPartida> barcosUsuario = new List<BarcoEnPartida>();
        public List<BarcoEnPartida> barcosIA = new List<BarcoEnPartida>();

        #region Encapsulado
        public Dificultad Dificultad
        {
            get
            {
                return dificultad;
            }
        }

        public string[] BarcosDeLaPartida
        {
            get
            {
                return barcosDeLaPartida;
            }
        }

        public EstadoDeLaPartida EstadoActual
        {
            get
            {
                return estadoActual;
            }

            set
            {
                estadoActual = value;
            }
        }
        #endregion

        public Partida(Dificultad dificultad, string[] nombreBarcos)
        {
            this.dificultad = dificultad;
            this.barcosDeLaPartida = nombreBarcos;
            this.estadoActual = EstadoDeLaPartida.UbicandoBarcos;
        }
    }

    [Header("Informacion General")]
    [SerializeField]
    int cantidadBarcos1 = 5;
    [SerializeField]
    int cantidadBarcos2 = 3;
    [SerializeField]
    int cantidadBarcos3 = 2;
    [SerializeField]
    int cantidadBarcos4 = 1;
    [SerializeField]
    int bonoDestruirBarco = 200;
    int bonoGanarPartida = 1000;

    [SerializeField]
    private GameObject tableroPrefab;
    [SerializeField]
    private GameObject escenarioPrefab;

    [Header("Solo Lectura")]
    public Partida informacionDeLaPartida;

    [HideInInspector]
    public Barco barcoAUbicar;

    private bool[,] tableroUsuario = new bool[10, 10];
    public bool[,] tableroDisparosUsuario = new bool[10, 10];
    private bool[,] tableroIA = new bool[10, 10];
    private bool[,] tableroDisparosIA = new bool[10, 10];

    private bool emepezoPartida;
    public bool turnoDelUsuario;
    private int dineroGanado;

	private string status;

    #region Encapsulado
    public int CantidadBarcos1
    {
        get
        {
            return cantidadBarcos1;
        }
    }

    public int CantidadBarcos2
    {
        get
        {
            return cantidadBarcos2;
        }
    }

    public int CantidadBarcos3
    {
        get
        {
            return cantidadBarcos3;
        }
    }

    public int CantidadBarcos4
    {
        get
        {
            return cantidadBarcos4;
        }
    }

    public GameObject EscenarioPrefab
    {
        get
        {
            return escenarioPrefab;
        }
    }

    public GameObject TableroPrefab
    {
        get
        {
            return tableroPrefab;
        }
    }

    #endregion

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (emepezoPartida)
        {
            if (turnoDelUsuario)
            {
                Debug.Log("Es turno del usuario");
            }
            else
            {
                Debug.Log("Es turno de la IA");
                Coordenada coordDisparo = seleccionarCoordenadaIA();
                Disparar(coordDisparo);
            }
        }
    }

    /// <summary>
    /// Para generar una coordenada aletaria
    /// </summary>
    /// <returns>Una cordenada aleatoria</returns>
    Coordenada seleccionarCoordenadaIA()
    {
        Coordenada coordSalida = new Coordenada("A", 0);
        if(informacionDeLaPartida.Dificultad == Dificultad.Facil)
        {
            do
            {
                //Buscamos una zona donde no haya tirado antes
                coordSalida = new Coordenada(Coordenada.FILAS[Random.Range(0, 10)], Random.Range(0, 10));
            } while (tableroDisparosIA[coordSalida.FilaToNum(),coordSalida.Columna]);
        }
        else
            if(informacionDeLaPartida.Dificultad == Dificultad.Dificil)
        {
            do
            {
                //Buscamos una zona donde no haya tirado antes
                coordSalida = new Coordenada(Coordenada.FILAS[Random.Range(0, 10)], Random.Range(0, 10));
            } while (tableroDisparosIA[coordSalida.FilaToNum(), coordSalida.Columna]);
        }
        return coordSalida;
    }

    /// <summary>
    /// Realiza un disparo hacia el tablero del oponente
    /// </summary>
    /// <param name="coordDisparo">La coordenada dode se va a disparar</param>
    public void Disparar(Coordenada coordDisparo)
    {
        if (turnoDelUsuario)
        {
            tableroDisparosUsuario[coordDisparo.FilaToNum(), coordDisparo.Columna] = true;
            
            //if Hit
            if (tableroIA[coordDisparo.FilaToNum(), coordDisparo.Columna])
            {
                //buscamos el barco que golpie;
                BarcoEnPartida barcoGolpeado = getBarcoGolpeado(coordDisparo);
                barcoGolpeado.infoInstancia.golpesRestantes--;
                if(barcoGolpeado.infoInstancia.golpesRestantes == 0)
                {
                    //Se destuye el barco
                    informacionDeLaPartida.barcosIA.Remove(barcoGolpeado);
                    Debug.Log("Barco Enemigo Destruido");
                    dineroGanado += bonoDestruirBarco;
                }
                GameObject.Find("Escenario/TableroEnemigo/" + coordDisparo).GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                GameObject.Find("Escenario/TableroEnemigo/" + coordDisparo).GetComponent<Renderer>().material.color = Color.red;
            }
        }
        else
        {
            tableroDisparosIA[coordDisparo.FilaToNum(), coordDisparo.Columna] = true;

            //if hit
            if (tableroUsuario[coordDisparo.FilaToNum(), coordDisparo.Columna])
            {
                BarcoEnPartida barcoGolpeado = getBarcoGolpeado(coordDisparo);
                barcoGolpeado.infoInstancia.golpesRestantes--;
                if (barcoGolpeado.infoInstancia.golpesRestantes == 0)
                {
                    //Se destuye el barco
                    informacionDeLaPartida.barcosUsuario.Remove(barcoGolpeado);
                    Debug.Log("Barco Usuario DEstruido");
                }
                GameObject.Find("Escenario/TableroJugador/" + coordDisparo).GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                GameObject.Find("Escenario/TableroJugador/" + coordDisparo).GetComponent<Renderer>().material.color = Color.red;
            }
        }


        //Verificamos las condiciones de victoria o derr0ta
        if (informacionDeLaPartida.barcosIA.Count == 0)
        {
            Debug.Log("Se acabo la partida, has ganado");
            dineroGanado += bonoGanarPartida;
			status="gano";
			StartCoroutine (WaitForRequest());
            SceneManager.LoadScene("MenuPrincipal");
        }
        else
            if (informacionDeLaPartida.barcosUsuario.Count == 0)
        {
            Debug.Log("Se acabo la patida, ha ganado la maquina");
			status="perdio";
			StartCoroutine (WaitForRequest());
            SceneManager.LoadScene("MenuPrincipal");
        }
        else
        {
            turnoDelUsuario = !turnoDelUsuario;
        }
    }

    /// <summary>
    /// Obetener el barco golpeado por un disparo
    /// </summary>
    /// <param name="coordenada">La cordenada del disparo</param>
    /// <returns>El barco golpeado</returns>
    private BarcoEnPartida getBarcoGolpeado(Coordenada coordenada)
    {
        if (turnoDelUsuario)
        {
            for (int i = 0; i < informacionDeLaPartida.barcosIA.Count; i++)
            {
                BarcoEnPartida barco = informacionDeLaPartida.barcosIA[i];
                for (int j = 0; j < barco.infoInstancia.coordenadasOcupadas.Length; j++)
                {
                    //SI este es el barco que golpeamos
                    if(barco.infoInstancia.coordenadasOcupadas[j].Equals(coordenada))
                    {
                        return barco;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < informacionDeLaPartida.barcosUsuario.Count; i++)
            {
                BarcoEnPartida barco = informacionDeLaPartida.barcosUsuario[i];
                for (int j = 0; j < barco.infoInstancia.coordenadasOcupadas.Length; j++)
                {
                    if (barco.infoInstancia.coordenadasOcupadas[j].Equals(coordenada))
                    {
                        return barco;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Metodo para notificiar que se va a empezar la partida
    /// </summary>
    public void SetForMatch()
    {
        informacionDeLaPartida.EstadoActual = EstadoDeLaPartida.Jugando;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SetPartida;
    }

    /// <summary>
    /// Prepara la escena de la partida una vez se han escogido todas las configuraciones de la partida
    /// y se han posicionado los barcos del jugador
    /// </summary>
    /// <param name="arg0"></param>
    /// <param name="arg1"></param>
    private void SetPartida(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        InstanciarMisBarcos();
        InstanciarBarcosIA();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= SetPartida;
        turnoDelUsuario = Random.Range(0, 2) == 0 ? true : false;
        emepezoPartida = true;
        dineroGanado = 0;
        status = "";
    }

    /// <summary>
    /// Instanciar los barcos en el world space y mostrarlos en el tablero del jugador
    /// </summary>
    private void InstanciarMisBarcos()
    {
        for (int i = 0; i < informacionDeLaPartida.barcosUsuario.Count; i++)
        {
            BarcoEnPartida barco = informacionDeLaPartida.barcosUsuario[i];
            Vector3 posicion = GameObject.Find("Escenario/TableroJugador/" + barco.infoInstancia.coordenadaDeReferencia).transform.position;
            Vector3 rotacion = barco.infoInstancia.orientacion == OrientacionBarco.Vertical ? Vector3.zero : new Vector3(0, 90, 0);
            GameObject b = Instantiate(Resources.Load<GameObject>("Barcos/" + barco.nombre), posicion, Quaternion.Euler(rotacion));
        }

        for (int i = 0; i < tableroUsuario.GetLength(0); i++)
        {
            for (int j = 0; j < tableroUsuario.GetLength(1); j++)
            {
                if (tableroUsuario[i, j])
                {
                    GameObject.Find("Escenario/TableroJugador/" + Coordenada.FILAS[i] + j).gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
        }
    }
	// para instanciar los barcos de la IA 
    private void InstanciarBarcosIA()
    {
        Barco b = null;
        //Instanciamos todos los barcos tipo 4
        for (int i = 0; i < cantidadBarcos4; i++)
        {
            b = Instantiate<GameObject>(Resources.Load<GameObject>("Barcos/" + informacionDeLaPartida.BarcosDeLaPartida[3]), new Vector3(10000, 10000, 10000), Quaternion.identity).GetComponent<Barco>();
            Coordenada coordenadaRandom = new Coordenada("A", 0);
            OrientacionBarco orientacionRandom = OrientacionBarco.Horizontal;
            bool posicionado = false;
            while (!posicionado)
            {
                //obtenemos una orientacion y coordenada random
                orientacionRandom = Random.Range(0, 2) == 0 ? OrientacionBarco.Horizontal : OrientacionBarco.Vertical;
                coordenadaRandom = new Coordenada(Coordenada.FILAS[(int)Random.Range(0, 10)], (int)Random.Range(0, 10));
                BarcoEnPartida barco = new BarcoEnPartida(b.informacionBarco.Nombre, b.informacionBarco.Longitud, orientacionRandom, coordenadaRandom);
                posicionado = VerfiUbicacionBarcoIA(barco);
            }
            Destroy(b.gameObject);
        }

        //barcos tipo 3
        for (int i = 0; i < cantidadBarcos3; i++)
        {
            b = Instantiate<GameObject>(Resources.Load<GameObject>("Barcos/" + informacionDeLaPartida.BarcosDeLaPartida[2]), new Vector3(10000, 10000, 10000), Quaternion.identity).GetComponent<Barco>();
            Coordenada coordenadaRandom = new Coordenada("A", 0);
            OrientacionBarco orientacionRandom = OrientacionBarco.Horizontal;
            bool posicionado = false;
            while (!posicionado)
            {
                //obtenemos una orientacion y coordenada random
                orientacionRandom = Random.Range(0, 2) == 0 ? OrientacionBarco.Horizontal : OrientacionBarco.Vertical;
                coordenadaRandom = new Coordenada(Coordenada.FILAS[(int)Random.Range(0, 10)], (int)Random.Range(0, 10));
                BarcoEnPartida barco = new BarcoEnPartida(b.informacionBarco.Nombre, b.informacionBarco.Longitud, orientacionRandom, coordenadaRandom);
                posicionado = VerfiUbicacionBarcoIA(barco);
            }
            Destroy(b.gameObject);
        }

        //barcos tipo 2
        for (int i = 0; i < cantidadBarcos2; i++)
        {
            b = Instantiate<GameObject>(Resources.Load<GameObject>("Barcos/" + informacionDeLaPartida.BarcosDeLaPartida[1]), new Vector3(10000, 10000, 10000), Quaternion.identity).GetComponent<Barco>();
            Coordenada coordenadaRandom = new Coordenada("A", 0);
            OrientacionBarco orientacionRandom = OrientacionBarco.Horizontal;
            bool posicionado = false;
            while (!posicionado)
            {
                //obtenemos una orientacion y coordenada random
                orientacionRandom = Random.Range(0, 2) == 0 ? OrientacionBarco.Horizontal : OrientacionBarco.Vertical;
                coordenadaRandom = new Coordenada(Coordenada.FILAS[(int)Random.Range(0, 10)], (int)Random.Range(0, 10));
                BarcoEnPartida barco = new BarcoEnPartida(b.informacionBarco.Nombre, b.informacionBarco.Longitud, orientacionRandom, coordenadaRandom);
                posicionado = VerfiUbicacionBarcoIA(barco);
            }
            Destroy(b.gameObject);
        }

        //barcos tipo 1
        for (int i = 0; i < CantidadBarcos1; i++)
        {
            b = Instantiate<GameObject>(Resources.Load<GameObject>("Barcos/" + informacionDeLaPartida.BarcosDeLaPartida[0]), new Vector3(10000, 10000, 10000), Quaternion.identity).GetComponent<Barco>();
            Coordenada coordenadaRandom = new Coordenada("A", 0);
            OrientacionBarco orientacionRandom = OrientacionBarco.Horizontal;
            bool posicionado = false;
            while (!posicionado)
            {
                //obtenemos una orientacion y coordenada random
                orientacionRandom = Random.Range(0, 2) == 0 ? OrientacionBarco.Horizontal : OrientacionBarco.Vertical;
                coordenadaRandom = new Coordenada(Coordenada.FILAS[(int)Random.Range(0, 10)], (int)Random.Range(0, 10));
                BarcoEnPartida barco = new BarcoEnPartida(b.informacionBarco.Nombre, b.informacionBarco.Longitud, orientacionRandom, coordenadaRandom);
                posicionado = VerfiUbicacionBarcoIA(barco);
            }
            Destroy(b.gameObject);
        }

        for (int i = 0; i < tableroIA.GetLength(0); i++)
        {
            for (int j = 0; j < tableroIA.GetLength(1); j++)
            {
                if (tableroIA[i, j])
                {
                    GameObject.Find("Escenario/TableroEnemigo/" + Coordenada.FILAS[i] + j).gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
        }
    }

    /// <summary>
    /// Verificar la ubicacion de un barco de la IA y lo posiciona si se puede ubicar
    /// </summary>
    /// <param name="barco">El barco a ubicar</param>
    /// <returns></returns>
    private bool VerfiUbicacionBarcoIA(BarcoEnPartida barco)
    {
        if (barco.infoInstancia.orientacion == OrientacionBarco.Vertical)
        {
            //Verificamos si el barco cabe
            if (barco.infoInstancia.coordenadaDeReferencia.FilaToNum() + 1 - barco.longitud >= 0)
            {
                //verificar si no hay otro barco
                if (!verificarSuperposicionBarcoIA(barco))
                {
                    ubicarBarcoIA(barco);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else//Si el barco no cabe
            {
                return false;
            }
        }
        else
            if (barco.infoInstancia.orientacion == OrientacionBarco.Horizontal)
        {
            //Si el barco cabe
            if (barco.infoInstancia.coordenadaDeReferencia.Columna - 1 + barco.longitud <= 9)
            {
                if (!verificarSuperposicionBarcoIA(barco))
                {
                    ubicarBarcoIA(barco);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                //Si no cabe
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// Verifica si hay un barco posicionado sobre una ona del mar del barco que se quiere posicionar
    /// </summary>
    /// <param name="barco">El barco a posicionar</param>
    /// <returns>true si hay superposicion, false si no</returns>
    bool verificarSuperposicionBarcoIA(BarcoEnPartida barco)
    {
        if (barco.infoInstancia.orientacion == OrientacionBarco.Vertical)
        {
            for (int i = 0; i < barco.longitud; i++)
            {
                if (tableroIA[barco.infoInstancia.coordenadaDeReferencia.FilaToNum() - i, barco.infoInstancia.coordenadaDeReferencia.Columna])
                {
                    return true;
                }
            }
        }
        else
            if (barco.infoInstancia.orientacion == OrientacionBarco.Horizontal)
        {
            for (int i = 0; i < barco.longitud; i++)
            {
                if (tableroIA[barco.infoInstancia.coordenadaDeReferencia.FilaToNum(), barco.infoInstancia.coordenadaDeReferencia.Columna + i])
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Ubica un barco de la IA, despues que se han hecho todas las verificaciones
    /// </summary>
    /// <param name="barco"></param>
    void ubicarBarcoIA(BarcoEnPartida barco)
    {
        barco.infoInstancia.coordenadasOcupadas = new Coordenada[barco.longitud];
        if (barco.infoInstancia.orientacion == OrientacionBarco.Vertical)
        {
            for (int i = 0; i < barco.longitud; i++)
            {
                int fila = barco.infoInstancia.coordenadaDeReferencia.FilaToNum() - i;
                tableroIA[fila, barco.infoInstancia.coordenadaDeReferencia.Columna] = true;
                barco.infoInstancia.coordenadasOcupadas[i] = new Coordenada(Coordenada.FILAS[fila], barco.infoInstancia.coordenadaDeReferencia.Columna);
            }
        }
        else
            if (barco.infoInstancia.orientacion == OrientacionBarco.Horizontal)
        {
            for (int i = 0; i < barco.longitud; i++)
            {
                int columna = barco.infoInstancia.coordenadaDeReferencia.Columna + i;
                tableroIA[barco.infoInstancia.coordenadaDeReferencia.FilaToNum(), columna] = true;
                barco.infoInstancia.coordenadasOcupadas[i] = new Coordenada(barco.infoInstancia.coordenadaDeReferencia.Fila, columna);
            }
        }
        informacionDeLaPartida.barcosIA.Add(barco);
    }
    

    /// <summary>
    /// Verifica si un barco puede ser ubicado en la posicion de este y pinta enseguida
    /// con el color correspondiete las zonas del mar ocupadas por este:
    /// Verde si puede ser ubicado, Rojo si no, blanco si se va a restablecer:
    /// Se restablece cada vez que cambie la ubicacion del barco.
    /// </summary>
    /// <param name="barco">El barco que se quiere verficar su ubicacion</param>
    /// <param name="restablecerPintado">si se va a restablecer las zonas del mar coupadas (pintarlas de blanco)</param>
    public void verificarUbicacionBarco(Barco barco, bool restablecerPintado = false)
    {
        if (barco.instanciaDelBarco.orientacion == OrientacionBarco.Vertical)
        {
            //Verificamos si el barco cabe
            if (barco.instanciaDelBarco.coordenadaDeReferencia.FilaToNum() + 1 - barco.informacionBarco.Longitud >= 0)
            {
                //verificar si no hay otro barco
                int longitudDeSuperposicion;
                if (!verificarSuperposicion(barco, barco.informacionBarco.Longitud, out longitudDeSuperposicion))
                {
                    barcoAUbicar.sePuedeUbicar = true;
                    if (!restablecerPintado)
                    {
                        pintarZonaDelMar(barco, TipoDePintado.SePuedeUbicar, barco.informacionBarco.Longitud);
                    }
                    else
                    {
                        pintarZonaDelMar(barco, TipoDePintado.Restablecer, barco.informacionBarco.Longitud);
                    }
                }
                else
                {
                    barco.sePuedeUbicar = false;
                    if (!restablecerPintado)
                    {
                        pintarZonaDelMar(barco, TipoDePintado.NoSePuedeUbicar, longitudDeSuperposicion);
                    }
                    else
                    {
                        pintarZonaDelMar(barco, TipoDePintado.Restablecer, longitudDeSuperposicion);
                    }
                }
            }
            else//Si el barco no cabe
            {
                barcoAUbicar.sePuedeUbicar = false;
                int longitudUbicable = barco.instanciaDelBarco.coordenadaDeReferencia.FilaToNum() + 1;//La cantidad de cuadros a pintar verticalmente
                int longitudPintado;
                if(!verificarSuperposicion(barco, longitudUbicable, out longitudPintado))//Si no se superpone
                {
                    longitudPintado = longitudUbicable;
                }

                if (!restablecerPintado)
                {
                    pintarZonaDelMar(barco, TipoDePintado.NoSePuedeUbicar, longitudPintado);
                }
                else
                {
                    pintarZonaDelMar(barco, TipoDePintado.Restablecer, longitudPintado);
                }
            }
        }
        else
            if (barco.instanciaDelBarco.orientacion == OrientacionBarco.Horizontal)
        {
            //Si el barco cabe
            if (barco.instanciaDelBarco.coordenadaDeReferencia.Columna - 1 + barco.informacionBarco.Longitud <= 9)
            {
                int longitudDeSuperposicion;
                if(!verificarSuperposicion(barco, barco.informacionBarco.Longitud, out longitudDeSuperposicion))
                {
                    barcoAUbicar.sePuedeUbicar = true;
                    if (!restablecerPintado)
                    {
                        pintarZonaDelMar(barco, TipoDePintado.SePuedeUbicar, barco.informacionBarco.Longitud);
                    }
                    else
                    {
                        pintarZonaDelMar(barco, TipoDePintado.Restablecer, barco.informacionBarco.Longitud);
                    }
                }
                else
                {
                    barco.sePuedeUbicar = false;
                    if (!restablecerPintado)
                    {
                        pintarZonaDelMar(barco, TipoDePintado.NoSePuedeUbicar, longitudDeSuperposicion);
                    }
                    else
                    {
                        pintarZonaDelMar(barco, TipoDePintado.Restablecer, longitudDeSuperposicion);
                    }
                }
            }
            else
            {
                barcoAUbicar.sePuedeUbicar = false;
                int longitudUbicable = tableroUsuario.GetLength(1) - barco.instanciaDelBarco.coordenadaDeReferencia.Columna;
                int longitudPintado;
                if (!verificarSuperposicion(barco, longitudUbicable, out longitudPintado))//Si no se superpone
                {
                    longitudPintado = longitudUbicable;//Devuelve la cantidad de cuadros a pintar horizontalmente
                }

                if (!restablecerPintado)
                {
                    pintarZonaDelMar(barco, TipoDePintado.NoSePuedeUbicar, longitudPintado);
                }
                else
                {
                    pintarZonaDelMar(barco, TipoDePintado.Restablecer, longitudPintado);
                }
            }
        }
    }


    /// <summary>
    /// Verifica si hay el barco actual se superpone con otro barco ya ubicado previamente
    /// </summary>
    /// <param name="barco">El barco que se quiere ubicar</param>
    /// <param name="longitudSuperposicion">Devuelve la longitud desde la posicion actual del barco, hasta la posicion donde colisiona, Esto para saber la cantidad de zonas del mar a pintar</param>
    /// <returns>true si se superpone, false si no</returns>
    private bool verificarSuperposicion(Barco barco, int longitudUbicable, out int longitudSuperposicion)
    {
        if (barco.instanciaDelBarco.orientacion == OrientacionBarco.Vertical) {
            for (int i = 0; i < longitudUbicable; i++)
            {
                if (tableroUsuario[barco.instanciaDelBarco.coordenadaDeReferencia.FilaToNum() - i, barco.instanciaDelBarco.coordenadaDeReferencia.Columna])
                {
                    longitudSuperposicion = i;
                    return true;
                }
            }
        }
        else
            if(barco.instanciaDelBarco.orientacion == OrientacionBarco.Horizontal)
        {
            for (int i = 0; i < longitudUbicable; i++)
            {
                if (tableroUsuario[barco.instanciaDelBarco.coordenadaDeReferencia.FilaToNum(), barco.instanciaDelBarco.coordenadaDeReferencia.Columna + i])
                {
                    longitudSuperposicion = i;
                    return true;
                }
            }
        }
        longitudSuperposicion = -1;
        return false;
    }

    /// <summary>
    /// Esteblecer el barco en la posicion actual y actualizar el tablero
    /// </summary>
    /// <param name="barco">El barco a ubicar</param>
    public void ubicarBarco(Barco barco)
    {
        barco.instanciaDelBarco.coordenadasOcupadas = new Coordenada[barco.informacionBarco.Longitud];
        if(barco.instanciaDelBarco.orientacion == OrientacionBarco.Vertical)
        {
            for (int i = 0; i < barco.informacionBarco.Longitud; i++)
            {
                int fila = barco.instanciaDelBarco.coordenadaDeReferencia.FilaToNum() - i;
                tableroUsuario[fila, barco.instanciaDelBarco.coordenadaDeReferencia.Columna] = true;
                barco.instanciaDelBarco.coordenadasOcupadas[i] = new Coordenada(Coordenada.FILAS[fila], barco.instanciaDelBarco.coordenadaDeReferencia.Columna);
            }
        }
        else
            if(barco.instanciaDelBarco.orientacion == OrientacionBarco.Horizontal)
        {
            for (int i = 0; i < barco.informacionBarco.Longitud; i++)
            {
                int columna = barco.instanciaDelBarco.coordenadaDeReferencia.Columna + i;
                tableroUsuario[barco.instanciaDelBarco.coordenadaDeReferencia.FilaToNum(), columna] = true;
                barco.instanciaDelBarco.coordenadasOcupadas[i] = new Coordenada(barco.instanciaDelBarco.coordenadaDeReferencia.Fila, columna);
            }
        }
        informacionDeLaPartida.barcosUsuario.Add(new BarcoEnPartida(barco.informacionBarco.Nombre, barco.informacionBarco.Longitud, barco.instanciaDelBarco));
        pintarZonaDelMar(barco , TipoDePintado.NoSePuedeUbicar, barco.informacionBarco.Longitud);
        barco.ubicado = true;
        barco.gameObject.layer = LayerMask.NameToLayer("Default");
        this.barcoAUbicar = null;
    }


    /// <summary>
    /// Re seleccionar un barco previamente ubicado, para cambiar su posicion
    /// </summary>
    /// <param name="barco">El barco a reubicar</param>
    public void reUbicarBarco(Barco barco)
    {
        BarcoEnPartida barcoEnPart = null;
        //Ahora lo buscamos dentro de la lista de barcos
        for (int i = 0; i < informacionDeLaPartida.barcosUsuario.Count; i++)
        {
            if (barco.instanciaDelBarco.coordenadaDeReferencia.Equals(informacionDeLaPartida.barcosUsuario[i].infoInstancia.coordenadaDeReferencia))
            {
                barcoEnPart = informacionDeLaPartida.barcosUsuario[i];
                break;
            }
        }

        informacionDeLaPartida.barcosUsuario.Remove(barcoEnPart);
        if (barco.instanciaDelBarco.orientacion == OrientacionBarco.Vertical)
        {
            for (int i = 0; i < barco.informacionBarco.Longitud; i++)
            {
                int fila = barco.instanciaDelBarco.coordenadaDeReferencia.FilaToNum() - i;
                tableroUsuario[fila, barco.instanciaDelBarco.coordenadaDeReferencia.Columna] = false;
            }
        }
        else
            if (barco.instanciaDelBarco.orientacion == OrientacionBarco.Horizontal)
        {
            for (int i = 0; i < barco.informacionBarco.Longitud; i++)
            {
                int columna = barco.instanciaDelBarco.coordenadaDeReferencia.Columna + i;
                tableroUsuario[barco.instanciaDelBarco.coordenadaDeReferencia.FilaToNum(), columna] = false;
            }
        }
        pintarZonaDelMar(barco, TipoDePintado.Restablecer, barco.informacionBarco.Longitud);
        barco.ubicado = false;
        barco.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        this.barcoAUbicar = barco;

    }

    /// <summary>
    /// Pinta las zonas del mar donde el barco se encuentra
    /// </summary>
    /// <param name="barco">bEl barco que se va a ubicar</param>
    /// <param name="tipoDePintado">La forma de pintado (Color)</param>
    /// <param name="longitudAPintar">La cantidad de zonas del mar que se van a pintar</param>
    public void pintarZonaDelMar(Barco barco, TipoDePintado tipoDePintado, int longitudAPintar)
    {
        //(Coordenada coordenadaInicio, OrientacionBarco orientacion, int longitudAPintar, TipoDePintado tipoDePintado)
        Color colorDeLaZona;
        if (tipoDePintado == TipoDePintado.SePuedeUbicar)
        {
            colorDeLaZona = Color.green;
        }
        else
            if (tipoDePintado == TipoDePintado.NoSePuedeUbicar)
        {
            colorDeLaZona = Color.red;
        }
        else
        {
            colorDeLaZona = Color.white;
        }


        if (barco.instanciaDelBarco.orientacion == OrientacionBarco.Vertical)
        {
            for (int i = 0; i < longitudAPintar; i++)
            {
                GameObject.Find("Tablero/" + Coordenada.FILAS[barco.instanciaDelBarco.coordenadaDeReferencia.FilaToNum() - i] + barco.instanciaDelBarco.coordenadaDeReferencia.Columna).GetComponent<Renderer>().material.color = colorDeLaZona;
            }
        }
        else
            if (barco.instanciaDelBarco.orientacion == OrientacionBarco.Horizontal)
        {
            for (int i = 0; i < longitudAPintar; i++)
            {
                GameObject.Find("Tablero/" + barco.instanciaDelBarco.coordenadaDeReferencia.Fila + (barco.instanciaDelBarco.coordenadaDeReferencia.Columna + i)).GetComponent<Renderer>().material.color = colorDeLaZona;
            }
        }
    }

    /// <summary>
    /// Cambia la orientacion del barco
    /// </summary>
    /// <param name="barco">El barco que se quiere cambiar la orientacion</param>
    /// <param name="nuevaOrientacion">La nueva orientacion que se quiere</param>
    public void cambiarOrientacionDelBarco(Barco barco, OrientacionBarco nuevaOrientacion)
    {
        verificarUbicacionBarco(barco, true);

        barco.instanciaDelBarco.orientacion = nuevaOrientacion;

        if (barco.instanciaDelBarco.orientacion == OrientacionBarco.Horizontal)
        {
            barco.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else
        {
            barco.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        verificarUbicacionBarco(barco);
    }

	//para hacer la peticion de actualizar el dinero y los contadores de partidas en base de datos cuando la partida se termina
	IEnumerator WaitForRequest(){

		if (Application.internetReachability == NetworkReachability.NotReachable) {//si no hay internet
			Debug.Log("No se ha podido guardar los datos de la partida lo sentimos");
		} else {

			WWWForm forma = new WWWForm ();
			forma.AddField ("id",Usuario.UsuarioConectado.Id);
			forma.AddField ("estado",status);
			forma.AddField ("dinero", dineroGanado);
			WWW request= new WWW("https://battleship3d.herokuapp.com/terminar_partida",forma);

			yield return request;

			string encodedString = request.text;
			if (request.error==null) {
				ShowMessage.showMessageText ("DATOS EN EL SERVIDOR ACTUALIZADOS CORRECTAMENTE",MessageType.Information);
				Debug.Log("DATOS EN EL SERVIDOR ACTUALIZADOS CORRECTAMENTE");
			}

		}



	}
}
