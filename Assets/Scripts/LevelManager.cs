﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla las referencias pero no perdura entre escenas
/// </summary>
public class LevelManager : MonoBehaviour {

    public static LevelManager instance = null;
    public GameObject jugador;
    public UIManager uiManager;
    public ArenaManager arenaManager;

    private MovimientoCamara camara;
    /// <summary>
    /// Método que se asegura de que solo haya un GameManager al mismo tiempo
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(this.gameObject);
    }
    void Start () {
		
	}
	
	void Update ()
    {
	}
    /// <summary>
    /// Activa el método "asignarseguimiento" de la cámara, permitiendo que la misma fije a un objetivo distinto más una distancia de separación.
    /// </summary>
    /// <param name="objeto"></param>
    public void AsignarSeguimiento(Transform objeto, Vector3 distancia)
    {
        //Buscamos la cámara y su componente.
        camara = GameObject.Find("Main Camera").GetComponent<MovimientoCamara>();
        camara.AsignarSeguimiento(objeto, distancia);
    }

    /// <summary>
    /// Suma materiales al jugador
    /// </summary>
    public void SumarMateriales(int cantidad)
    {
        Materiales mat = jugador.GetComponent<Materiales>();
        if (mat) mat.SumarMateriales(cantidad);
    }

    /// <summary>
    /// Aumenta en uno el número de enemigos muertos
    /// </summary>
    public void EnemigoMuerto()
    {
        arenaManager.EnemigoMuerto();
    }

    /// <summary>
    /// Método para que los otros componentes tengan acceso al jugador
    /// </summary>
    /// <returns></returns>
    public GameObject Jugador()
    {
        return jugador;
    }

    /// <summary>
    /// Cambia la ui para reflejar la vida del jugador
    /// </summary>
    /// <param name="vida">cantidad de vida que tiene el jugador</param>
    /// <param name="vidaMax">vida maxima del jugador</param>
    public void ActualizaVida(int vida, int vidaMax)
    {
        uiManager.ActualizaVida(vida, vidaMax);
    }

    /// <summary>
    /// Cambia la ui para reflejar los materiales del jugador
    /// </summary>
    /// <param name="materiales">Cantidad de materiales</param>
    /// <param name="materialesMax">Cantidad maxima de materiales a tener</param>
    public void ActualizaMateriales(int materiales, int materialesMax)
    {
        uiManager.ActualizaMateriales(materiales, materialesMax);
    }

    /// <summary>
    /// Dice al UIManager que muestre los nuevos materiales obtenidos
    /// </summary>
    /// <param name="mat"></param> Materiales a mostrar en pantalla
    /// <param name="pos"></param> Posición a la que se quieren enseñar
    public void MuestraPopUpMat(string mat, Vector2 pos)
    {
        uiManager.CreaPopUpMateriales(mat, pos);
    }

    public void VuelveaMenu()
    {
        SceneManager.LoadScene("Menu 1");
    }
}
