﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPausa : MonoBehaviour {


    public GameObject menuPausa;
    bool juegoPausado = false;
    public GameObject controles;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (juegoPausado)Continuar();            
            else
            {
                menuPausa.SetActive(true);  //muestra el menu
                Time.timeScale = 0;  //desactiva el juego
                GameManager.instance.CambiarPausa(true);
            }
            juegoPausado = !juegoPausado;  //invierte la variable juegoPausado
        }
	}

    /// <summary>
    /// Continua el juego
    /// </summary>
    public void Continuar()
    {
        menuPausa.SetActive(false);  //desactiva el menu
        Time.timeScale = 1;  //activa el juego
        GameManager.instance.CambiarPausa(false);
    }

    /// <summary>
    /// Este método activa y desactiva el panel que muestra los controles
    /// </summary>
    public void Controles()
    {
        if (controles.activeInHierarchy)
            controles.SetActive(false);
        else
            controles.SetActive(true);
    }
}
