﻿using UnityEngine;
using System.Collections;

public class Bootstrap : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StateManager.Instance.GoToState(StateManager.Instance.startStateName);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
