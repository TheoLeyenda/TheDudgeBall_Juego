﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerJugador : MonoBehaviour {

    // Use this for initialization
    public GameObject Jugador_Windows;
    public GameObject Jugador_Android;
	void Start () {
#if UNITY_EDITOR
        if (Jugador_Windows != null)
        {
            Instantiate(Jugador_Windows, this.transform.position, Quaternion.identity);
        }
#elif UNITY_STANDALOVE
        if (Jugador_Windows != null)
        {
            Instantiate(Jugador_Windows, this.transform.position, Quaternion.identity);
        }
#elif UNITY_ANDROID
        if(Jugador_Android != null)
        {
            Instantiate(Jugador_Android, this.transform.position, Quaternion.identity);
        }
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}
}
