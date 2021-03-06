﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerPlayer : MonoBehaviour {

    // Use this for initialization
    public GameObject player_Windows;
    public GameObject player_Android;
	void Start () {
#if UNITY_EDITOR
        if (player_Windows != null)
        {
            Instantiate(player_Windows, this.transform.position, Quaternion.identity);
        }
#elif UNITY_STANDALOVE
        if (player_Windows != null)
        {
            Instantiate(player_Windows, this.transform.position, Quaternion.identity);
        }
#elif UNITY_ANDROID
        if(player_Android != null)
        {
            Instantiate(player_Android, this.transform.position, Quaternion.identity);
        }
#endif
    }
}