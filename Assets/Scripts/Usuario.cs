using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Usuario {

    public static Usuario UsuarioConectado { get; set; }

    [SerializeField]
    private int id;
    [SerializeField]
    private string nombre;
    [SerializeField]
    private string apellido;
    [SerializeField]
    private string username;
    [SerializeField]
    private string email;
    [SerializeField]
    private string password;
    [SerializeField]
    private int dinero = 0;
    [SerializeField]
    private int partidasJugadas = 0;
    [SerializeField]
    private int partidasGanadas = 0;

    #region Encapsulado
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

        set
        {
            nombre = value;
        }
    }

    public string Apellido
    {
        get
        {
            return apellido;
        }

        set
        {
            apellido = value;
        }
    }

    public string Username
    {
        get
        {
            return username;
        }

        set
        {
            username = value;
        }
    }

    public string Email
    {
        get
        {
            return email;
        }

        set
        {
            email = value;
        }
    }

    public string Password
    {
        get
        {
            return password;
        }

        set
        {
            password = value;
        }
    }

    public int Dinero
    {
        get
        {
            return dinero;
        }

        set
        {
            dinero = value;
        }
    }

    public int PartidasJugadas
    {
        get
        {
            return partidasJugadas;
        }

        set
        {
            partidasJugadas = value;
        }
    }

    public int PartidasGanadas
    {
        get
        {
            return partidasGanadas;
        }

        set
        {
            partidasGanadas = value;
        }
    }

    #endregion

    public Usuario(int id,string nombre,string apellido, string username, string email, string password)
    {
        this.id = id;
        this.nombre = nombre;
        this.apellido = apellido;
        this.username = username;
        this.email = email;
        this.password = password;
    }
}
