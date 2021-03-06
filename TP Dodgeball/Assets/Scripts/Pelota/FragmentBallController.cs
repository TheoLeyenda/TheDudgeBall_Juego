﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentBallController : MonoBehaviour {

    // Use this for initialization
    public FragmentBall codeFragment;
    public SmallBalls codeMiniBall1;
    public SmallBalls codeMiniBall2;
    public SmallBalls codeMiniBall3;
    public GameObject FragmentBall;
    public GameObject miniBall1;
    public GameObject miniBall2;
    public GameObject miniBall3;
    public Pool pool;
    private PoolObject poolObject;
    public float auxTimeLifeFragmentBalls;
    //public float auxTiempoVidaMiniPelota1;
    //public float auxTiempoVidaMiniPelota2;
    //public float auxTiempoVidaMiniPelota3;
    private void Start()
    {
        auxTimeLifeFragmentBalls = codeFragment.GetLifeTime();
    }
    void OnEnable() {
        
        poolObject = GetComponent<PoolObject>();
        FragmentBall.SetActive(true);
        miniBall1.SetActive(false);
        miniBall2.SetActive(false);
        miniBall3.SetActive(false);
        codeFragment.SetRecycle(false);
        codeMiniBall1.SetRecycle(false);
        codeMiniBall2.SetRecycle(false);
        codeMiniBall3.SetRecycle(false);
    }
	
	// Update is called once per frame
	void Update () {
        if(codeFragment.GetRecycle())
        {
            codeFragment.SetLifeTime(auxTimeLifeFragmentBalls);
            FragmentBall.SetActive(false);
            miniBall1.SetActive(true);
            miniBall2.SetActive(true);
            miniBall3.SetActive(true);

            if (codeMiniBall1.GetRecycle() && codeMiniBall2.GetRecycle() && codeMiniBall3.GetRecycle())
            {
                poolObject.Recycle();
            }
        }
	}
    public void Shoot()
    {
        codeFragment.SetLifeTime(auxTimeLifeFragmentBalls);
        FragmentBall.SetActive(true);
        codeFragment.SetRecycle(false);
        codeMiniBall1.SetRecycle(false);
        codeMiniBall2.SetRecycle(false);
        codeMiniBall3.SetRecycle(false);
        FragmentBall.transform.position = (transform.position + transform.right)-transform.forward;
        codeFragment.Shoot();
    }
}