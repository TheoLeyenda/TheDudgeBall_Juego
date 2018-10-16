﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tirador : Enemigo {

    // Use this for initialization
    public float auxVida;
    public PoolPelota poolTirador;
    public PoolPelota pelotasRugby;
    private PoolObject poolObject;
    private float auxTiempoVida;
    public float velocidad;
    public float dilay;
    private float auxDilay;
    public GameObject Bola;
    public GameObject generadorPelota;
    public GameObject tirador;
    private float timeEstado;
    private float auxVelocidad;
    private float efectoFuego;
    private Rigidbody rig;
    private float dileyInsta;

    public PoolPelota poolPoderInmune;
    public PoolPelota poolDoblePuntuacion;
    public PoolPelota poolInstaKill;

    public int tipoPatron;

    void Start () {
        dileyInsta = 1;
        SetEstadoEnemigo(EstadoEnemigo.normal);
        rig = GetComponent<Rigidbody>();
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        auxDilay = dilay;
        auxVelocidad = velocidad;
        timeEstado = 0;
        efectoFuego = 0;
        efectoCongelado.SetActive(false);
        efectoMusica.SetActive(false);
        efectoQuemado.SetActive(false);
    }
    public void Prendido()
    {
        dileyInsta = 1;
        SetEstadoEnemigo(EstadoEnemigo.normal);
        vida = auxVida;
        rig = GetComponent<Rigidbody>();
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        auxVelocidad = velocidad;
        efectoFuego = 0;
        efectoCongelado.SetActive(false);
        efectoQuemado.SetActive(false);
        efectoMusica.SetActive(false);
        poolObject = GetComponent<PoolObject>();
    }
    // Update is called once per frame
    void Update () {
        if (Jugador.GetJugador() != null)
        {
            if (Jugador.GetJugador().GetInstaKill())
            {
                vida = 1;
            }
            if (!Jugador.GetJugador().GetInstaKill() && Jugador.GetJugador().GetActivarInstaKill())
            {
                vida = auxVida;
                if (dileyInsta > 0)
                {
                    dileyInsta = dileyInsta - Time.deltaTime;
                }
                if (dileyInsta <= 0)
                {
                    Jugador.GetJugador().SetActivarInstaKill(false);
                    dileyInsta = 1;
                }
            }
        }
        EstaMuerto();
        updateHP();
        if (GetEstadoEnemigo() != EstadoEnemigo.congelado && GetEstadoEnemigo() != EstadoEnemigo.bailando)
        {
            Movimiento();
        }
        if(dilay <= 0)
        {
            dilay = auxDilay;
            TirarBola();
        }
        if (dilay > 0)
        {
            dilay = dilay - Time.deltaTime;
        }
        if(GetMuerto())
        {
            // Seguir configurando la probabilidad de aparicion de los powers ups
            float auxiliar = Random.Range(1, 100);
            if (auxiliar > 0 && auxiliar <= 8)
            {
                GameObject go = poolPoderInmune.GetObject();
                if (go != null)
                {
                    poolPoderInmune.RestarId();
                    go.transform.position = transform.position;
                    go.transform.rotation = transform.rotation;
                }
            }
            if (auxiliar > 12 && auxiliar <= 25)
            {

                GameObject go = poolDoblePuntuacion.GetObject();
                if (go != null)
                {
                    poolDoblePuntuacion.RestarId();
                    go.transform.position = transform.position;
                    go.transform.rotation = transform.rotation;
                }
            }
            if (auxiliar > 30 && auxiliar <= 35)
            {
                GameObject go = poolInstaKill.GetObject();
                if (go != null)
                {
                    poolInstaKill.RestarId();
                    go.transform.position = transform.position;
                    go.transform.rotation = transform.rotation;
                }
            }
            Jugador.GetJugador().SumarPuntos(60);
            GameManager.GetGameManager().SumarMuertes();
            if (GameManager.GetGameManager() != null && estoyEnPool)
            {
                GameManager.GetGameManager().RestarEnemigoEnPantalla();
            }
            SetMuerto(false);
            if (!estoyEnPool)
            {
                gameObject.SetActive(false);
            }
            if (estoyEnPool)
            {
                poolObject.Resiclarme();
            }
        }
        if(timeEstado > 0)
        {
            timeEstado = timeEstado - Time.deltaTime;
            if (GetEstadoEnemigo() == EstadoEnemigo.congelado)
            {
                dilay = 1000000000;
            }
            if (GetEstadoEnemigo() == EstadoEnemigo.bailando)
            {
                SetRotarY(20);
                Rotar();
            }
            if(timeEstado <= 0 && GetEstadoEnemigo() == EstadoEnemigo.bailando)
            {
                SetEstadoEnemigo(EstadoEnemigo.normal);
            }
            if (GetEstadoEnemigo() == EstadoEnemigo.quemado || efectoQuemado.active)
            {
                efectoFuego = efectoFuego + Time.deltaTime;
                if (efectoFuego >= 1)
                {
                    if (Jugador.GetJugador() != null)
                    {
                        if (Jugador.GetJugador().GetDoblePuntuacion())
                        {
                            Jugador.GetJugador().SumarPuntos(5*2);
                        }
                        else
                        {
                            Jugador.GetJugador().SumarPuntos(5);
                        }
                        vida = vida - (GetDanioBolaFuego() + Jugador.GetJugador().GetDanioAdicionalPelotaFuego());
                    }
                    efectoFuego = 0;
                }
            }
        }
        if(timeEstado <= 0 && GetEstadoEnemigo() == EstadoEnemigo.congelado)
        {
            velocidad = auxVelocidad;
            dilay = auxDilay;
            SetEstadoEnemigo(EstadoEnemigo.normal);
        }
        if(timeEstado <= 0 && GetEstadoEnemigo() == EstadoEnemigo.quemado)
        {
            SetEstadoEnemigo(EstadoEnemigo.normal);
        }
        if (GetEstadoEnemigo() != EstadoEnemigo.quemado && GetEstadoEnemigo() != EstadoEnemigo.bailando)
        {
            efectoQuemado.SetActive(false);
        }
        if (GetEstadoEnemigo() != EstadoEnemigo.congelado)
        {
            efectoCongelado.SetActive(false);
        }
        if (GetEstadoEnemigo() != EstadoEnemigo.bailando)
        {
            efectoMusica.SetActive(false);
        }
    }
    public void Movimiento()
    {
        if (tipoPatron == 0)
        {
            if (Jugador.GetJugador() != null)
            {
                rig.velocity = Vector3.zero;
                rig.angularVelocity = Vector3.zero;
                transform.LookAt(new Vector3(Jugador.GetJugador().transform.position.x, transform.position.y, Jugador.GetJugador().transform.position.z));
                transform.position += transform.forward * Time.deltaTime * velocidad; //si comento esto es una torreta y sino es un jugador de rugby
            }
        }
        if(tipoPatron == 1)
        {
            rig.velocity = Vector3.zero;
            rig.angularVelocity = Vector3.zero;
            transform.position += transform.forward * Time.deltaTime * velocidad;
        }
    }
    public void TirarBola()
    {
        //Instantiate(Bola,generadorPelota.transform.position ,generadorPelota.transform.rotation);
        GameObject go = pelotasRugby.GetObject();
        PelotaEnemigo pelota = go.GetComponent<PelotaEnemigo>();
        go.transform.position = generadorPelota.transform.position;
        go.transform.rotation = generadorPelota.transform.rotation;
        pelota.Disparar();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PelotaComun")
        {
            if (Jugador.GetJugador() != null)
            {
                vida = vida - (GetDanioBolaComun() + Jugador.GetJugador().GetDanioAdicionalPelotaComun());
                EstaMuerto();
                if (Jugador.GetJugador().GetDoblePuntuacion())
                {
                    Jugador.GetJugador().SumarPuntos(10* 2);
                }
                else
                {
                    Jugador.GetJugador().SumarPuntos(10);
                }
            }
        }
        if (other.gameObject.tag == "PelotaDeHielo")
        {
            if (Jugador.GetJugador() != null)
            {
                if (Jugador.GetJugador().GetDoblePuntuacion())
                {
                    Jugador.GetJugador().SumarPuntos(10*2);
                }
                else
                {
                    Jugador.GetJugador().SumarPuntos(10);
                }
                vida = vida - (GetDanioBolaHielo() + Jugador.GetJugador().GetDanioAdicionalPelotaHielo());
            }
            EstaMuerto();
            if (velocidad > 0)
            {
                //velocidad = velocidad - 0.2f;
                velocidad = 0;
            }
            if (velocidad <= 0)
            {

                if (GetEstadoEnemigo() != EstadoEnemigo.congelado)
                {
                    timeEstado = 5;//tiempo por el cual el enemigo "Corredor" estara congelado
                }
                SetEstadoEnemigo(EstadoEnemigo.congelado);
                efectoCongelado.SetActive(true);
            }
        }
        if (other.gameObject.tag == "MiniPelota")
        {
            if (Jugador.GetJugador() != null)
            {
                if (Jugador.GetJugador().GetDoblePuntuacion())
                {
                    Jugador.GetJugador().SumarPuntos(10 * 2);
                }
                else
                {
                    Jugador.GetJugador().SumarPuntos(10);
                }
                vida = vida - (GetDanioMiniBola() + Jugador.GetJugador().GetDanioAdicionalMiniPelota());
                EstaMuerto();
            }
        }
        if (other.gameObject.tag == "PelotaDanzarina")
        {
           
            if (GetEstadoEnemigo() != EstadoEnemigo.bailando)
            {
                timeEstado = 7;//tiempo por el cual el enemigo estara bailando
            }
            SetEstadoEnemigo(EstadoEnemigo.bailando);
            efectoMusica.SetActive(true);
            vida = vida - GetDanioBolaDanzarina();
            EstaMuerto();
            if (Jugador.GetJugador().GetDoblePuntuacion())
            {
                Jugador.GetJugador().SumarPuntos(5*2);
            }
            else
            {
                Jugador.GetJugador().SumarPuntos(5);
            }
        }
        if (other.gameObject.tag == "PelotaDeFuego")
        {
            
            if (GetEstadoEnemigo() != EstadoEnemigo.quemado)
            {
                timeEstado = 7;
            }
            if (GetEstadoEnemigo() != EstadoEnemigo.bailando)
            {
                SetEstadoEnemigo(EstadoEnemigo.quemado);
            }
            efectoQuemado.SetActive(true);
            velocidad = auxVelocidad;
            dilay = auxDilay;
        }
        if(other.gameObject.tag == "PelotaExplociva")
        {
            EstaMuerto();
            if (Jugador.GetJugador() != null)
            {
                if (Jugador.GetJugador().GetDoblePuntuacion())
                {
                    Jugador.GetJugador().SumarPuntos(20*2);
                }
                else
                {
                    Jugador.GetJugador().SumarPuntos(10);
                }
                vida = vida - (GetDanioBolaExplociva() + Jugador.GetJugador().GetDanioAdicionalPelotaExplociva());
            }

        }
    }
    public void SetVelocidad(float _velocidad)
    {
        velocidad = _velocidad;
    }
    public void SetAuxVelocidad(float _auxVelociad)
    {
        auxVelocidad = _auxVelociad;
    }
    public float GetVelociadad()
    {
        return velocidad;
    }
    public float GetAuxVelocidad()
    {
        return auxVelocidad;
    }
    public void SumarVelocidad()
    {
        velocidad = velocidad + 0.02f;
        auxVelocidad = velocidad;
    }
}
