using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {
    private static GameObject instance;

    public static Animator animator;
	void Start () {
        DontDestroyOnLoad(gameObject);
        if(instance == null)
        {
            instance = gameObject;
        }
        else
        {
            DestroyObject(gameObject);
            //instance = gameObject;
        }
        if(instance == gameObject)
        animator = GetComponentInChildren<Animator>();
	}
    
    void Update () {
		
	}
}
