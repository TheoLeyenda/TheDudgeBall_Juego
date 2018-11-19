﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTrampa : MonoBehaviour {

    // Use this for initialization
    public PoolPelota poolEnemigo;
    private PoolObject poolObject;
    public float RangoX;
    public float RangoZ;
    public int cantidad;
    public bool generarAlEmpezar;
    void Start () {
        
        if (generarAlEmpezar)
        {
            for (int i = 0; i < cantidad; i++)
            {
                Generar();
            }
        }
	}

    // Update is called once per frame
    void Update () {
		
	}
    public void Generar()
    {
        GameObject go = poolEnemigo.GetObject();
        float x = Random.Range(-RangoX, RangoX);
        float z = Random.Range(-RangoZ, RangoZ);
        go.transform.position = new Vector3(transform.position.x+x, transform.position.y, transform.position.z+z);
    }
}
