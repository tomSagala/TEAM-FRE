using UnityEngine;
using System.Collections;

public class CreditsState : MonoBehaviour
{
    public void BackButton()
    {
        StateManager.Instance.GoToState("MainMenu");
    }
}

