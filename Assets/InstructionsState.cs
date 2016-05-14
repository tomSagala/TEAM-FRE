using UnityEngine;
using System.Collections;

public class InstructionsState : MonoBehaviour
{
    public void BackButton()
    {
        StateManager.Instance.GoToState("MainMenu");
    }
}
