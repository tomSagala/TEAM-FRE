﻿using UnityEngine;
using System.Collections;

public class StateManager : GameSingleton<StateManager> 
{
	[SerializeField]
	public Canvas[] States;
	public string startStateName;

    void Start()
    {
        GoToState(startStateName);
    }

	public void GoToState(string stateName)
	{
        Debug.Log("Going to : " + stateName);
		for (int i = 0; i < States.Length; ++i) 
		{
            bool isActive = States[i].name.Equals(stateName);
            States[i].gameObject.SetActive(isActive);
		}
	}
}
