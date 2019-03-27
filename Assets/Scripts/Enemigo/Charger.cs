﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla el movimiento del enemigo (lineal) con velocidad ajustable y la rotacion
/// </summary>
public class Charger : MonoBehaviour {

    public float TiempoPreparacion;
    public float TiempoRepeticion;
    public int fuerza;
    public float TiempoDescanso;
    public float velocidad;

    private Rigidbody2D rb;
    private Vector2 movimiento;
    private GameObject jugador;
    private Vector2 diferencia;
    private float angulo;
    private bool moverse = true;
    private bool moveratras = false;

	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        jugador = LevelManager.instance.Jugador(); //recibe una referencia del jugador
        InvokeRepeating("ComienzaCarga", 0, TiempoRepeticion);
    }
	
	void Update ()
    {
        if (moverse || moveratras)
        {
            //diferencia de posicion entre el jugador y el enemigo
            diferencia = new Vector2(jugador.transform.position.x - transform.position.x, jugador.transform.position.y - transform.position.y);
            angulo = Mathf.Atan2(diferencia.x, diferencia.y) * Mathf.Rad2Deg; //angulo a traves de la tangente y lo pasa a grados
            transform.rotation = Quaternion.Euler(0, 0, -angulo); //cambia la rotacion del enemigo
        }
    }

    private void FixedUpdate()
    {
        if (jugador != null && moverse) //cacheo de referencia
        {
            //halla el vector direccion entre la posicion del enemigo y la del jugador y lo normaliza
            movimiento = new Vector2(jugador.transform.position.x - rb.position.x, jugador.transform.position.y - rb.position.y).normalized;
            //mueve al enemigo asegurandose de que no supera la velocidad si se mueve en diagonal
            rb.velocity = Vector2.ClampMagnitude(movimiento * velocidad, velocidad);
        }
        else if (moveratras) //El enemigo se mueve hacia atrás
        {
            movimiento = new Vector2(jugador.transform.position.x - rb.position.x, jugador.transform.position.y - rb.position.y).normalized;
            rb.velocity = Vector2.ClampMagnitude(-movimiento * velocidad, velocidad);
        }
    }


    /// <summary>
    /// Este método se encarga de comenzar la corrutina 'Carga' al ser llamado desde el InvokeRepeating de Start()
    /// </summary>
    private void ComienzaCarga() 
    {
        StartCoroutine(Carga());
    }

    /// <summary>
    /// Esta corrutina  controla el moviminto del Charger, dividido en varias fases que se suceden tras cierto tiempo
    /// </summary>
    /// <returns></returns>
    private IEnumerator Carga()
    {
        moveratras = false; //Si es true, el enemigo se mueve hacia atrás
        moverse = true; //Si es true, el enemigo se mueve hacia delante mediante rb.velocity
        velocidad /= 2; //Se reduce la velocidad para prepararse antes de la carga
        yield return new WaitForSeconds(TiempoPreparacion); //Se espera durante un tiempo de preparación elegido desde el editor
        moverse = false; //Se cancela el movimiento mediante rb.velocity para añadir una fuerza de carga
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(movimiento * fuerza); //Se añade la fuerza de carga
        yield return new WaitForSeconds(TiempoDescanso); //Se espera durante un tiempo de descanso
        velocidad *= 2; //La velocidad vuelve a a normalidad
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        moveratras = true; //El enemigo comienza a alejarse del jugador, tras lo cual comenzará el proceso de carga de nuevo
    }
}