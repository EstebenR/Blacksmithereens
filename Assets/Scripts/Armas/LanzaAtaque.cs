﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanzaAtaque : MonoBehaviour
{

    /*
     * Este meétodo se encarga de:
     *  llevar la cuenta de la durabilidad de la lanza
     *  que al al recibir la orden de ataque(ataqueJugador.cs), la lanza se mueva hacia delante y luego vuelva a su posicion normal.
    */

    public GameObject Lanza;
    public float velocidad;
    public int durMaxLanza = 20;       //Durabilidad máxima

    Animator animador;
    Transform transformLanza;
    AtaqueJugador scriptarmas;
    GameObject Lanzada;
    Vector2 mouse_position; //posicion del raton
    Vector2 offset, screenPoint; //vectores para sacar el angulo
    int durActualLanza;                 //Durabilidad actual, la que va disminuyendo

    void Start()
    {
        animador = gameObject.GetComponent<Animator>();
        transformLanza = gameObject.GetComponent<Transform>();
        durActualLanza = durMaxLanza;
        scriptarmas = LevelManager.instance.Jugador().GetComponent<AtaqueJugador>();
    }

    void Update()
    {

    }

    /// <summary>
    /// Comprueba si la lanza está cerca del jugador y, en ese caso, activa la animación de ataque
    /// </summary>
     public void AtaqueLanza() {
        if (animador.GetCurrentAnimatorStateInfo(0).IsName("LanzaNormal"))      //Posicion relativa al jugador, para evitar ataques dobles
        {
            animador.SetTrigger("Ataque");
        }
    }


    /// <summary>
    /// Comprueba si la lanza está cerca del jugador y, en ese caso, activa la animación de lanzarla
    /// </summary>
    public void LanzarLanza()
    {
        if ((transformLanza.localPosition.y > -0.8) && animador.GetCurrentAnimatorStateInfo(0).IsName("LanzaNormal"))      //Posicion relativa al jugador, para evitar ataques dobles
        {            
            Lanzada = (GameObject)Instantiate(Lanza, transform); //crea la lanza que va a ser lanzada
            Lanzada.GetComponent<SpriteRenderer>().sortingOrder = 1; //cambia la sortingLayer
            Lanzada.transform.parent = null;  //elimina el padre de la lanzaLanzada para evitar que rote con el jugador
            mouse_position = Input.mousePosition; //obtiene posicion del raton
            screenPoint = Camera.main.WorldToScreenPoint(transform.position); //saca la posicion del jugador en relacion al tamaño de la pantalla de juego
            offset = new Vector2(mouse_position.x - screenPoint.x, mouse_position.y - screenPoint.y); //diferencia de posicion entre raton y jugador
            Lanzada.GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(offset, velocidad); //impulsa la lanza
            Lanzada.GetComponent<HacerDanoLanzaLanzada>().SetDurabilidad(durActualLanza); //le pasa a la lanza la durabilidad actual
            durActualLanza = durMaxLanza;  //resetea la durabiliad complete de la lanza
            Invoke("Destruir", 1f); 

            this.gameObject.SetActive(false); 

            scriptarmas.CambioArma(0); //cambia al martillo 
        }
    }

    /// <summary>
    /// Destruye el objeto pasado
    /// </summary>
    private void Destruir()
    {
        if (Lanza != null)
        {
            Destroy(Lanzada.gameObject);
        }
    }


    /// <summary>
    /// Resta 'cantidad' a la durabilidad actual de la lanza
    /// </summary>
    /// <param name="cantidad">Puntos de durabilidad a ser descontados</param>
     public void RestaDurLanza(int cantidad) {
        durActualLanza -= cantidad;
        print("Durabilidad Lanza: " + durActualLanza);
        if (durActualLanza <= 0)
        {
            scriptarmas.CambioArma(Armas.Martillo);
            durActualLanza = durMaxLanza;
        }
    }

    /// <summary>
    /// Resetea al valor maximo la durabilidad de la lanza
    /// </summary>
    public void ReseteaDurLanza() {
        durActualLanza = durMaxLanza;
    }

}
