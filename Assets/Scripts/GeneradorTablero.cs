using UnityEngine;
using UnityEngine.Networking;

public class GeneradorTablero : NetworkBehaviour {

    [SerializeField]
    private Vector2 tamano = new Vector2(10, 10);
    [SerializeField]
    private BoxCollider zonaDelMarPrefab;
    private string tagZonasDelMar = "ZonaDelMar";

	// Use this for initialization
	void Start () {

        GenerarTablero();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void GenerarTablero()
    {
        GameObject tablero = new GameObject("Tablero");

        for (int fila = 0; fila < tamano.y; fila++)
        {
            string filaCoord = Coordenada.FILAS[((int)tamano.y - 1) - fila];
            for (int columna = 0; columna < tamano.x; columna++)
            {
                Vector3 posicionZona = new Vector3(zonaDelMarPrefab.size.x * columna, 0, zonaDelMarPrefab.size.z * fila);
                BoxCollider zonaDelMar = Instantiate<BoxCollider>(zonaDelMarPrefab, posicionZona, Quaternion.identity);
                zonaDelMar.transform.SetParent(tablero.transform);
                zonaDelMar.tag = tagZonasDelMar;
                Coordenada coordenadaZona = new Coordenada(filaCoord, columna);
                zonaDelMar.GetComponent<ZonaDelMar>().coordenada = coordenadaZona;
                zonaDelMar.gameObject.name = coordenadaZona.ToString();
                zonaDelMar.transform.localScale = new Vector3(0.95f, 1, 0.95f);
            }
        }
    }

    public void RetirarTablero()
    {
        GameObject[] tablero = GameObject.FindGameObjectsWithTag(tagZonasDelMar);
        foreach (GameObject zonaDelMar in tablero)
        {
            Destroy(zonaDelMar);
        }
    }

    public override void OnStartServer()
    {
        Debug.Log("J:OnStartServer");
        base.OnStartServer();
        GenerarTablero();
    }
}
