using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModuloBarcoTienda : MonoBehaviour {

	[HideInInspector]
	public TiendaUIController tienda;
    [SerializeField]
    Text textoNombreBarco;
    [SerializeField]
    private int barcoID;
    private int precio;

    public int BarcoID
    {
        get
        {
            return barcoID;
        }
		set{
			barcoID = value;
		}
    }

    public int Precio
    {
        get
        {
            return precio;
        }
		set{
			precio = value;
		}
    }

    // Use this for initialization
    void Start () {
		
        //EL precio se obtiene de la base de datos con el ID
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setNombre(string nombre)
    {
        textoNombreBarco.text = nombre;
    }
}
