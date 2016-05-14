using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameBootstrap : MonoBehaviour
{
    public List<GameObject> CharacterPrefabs;
    public List<Transform> CharacterSpawnPoints;

    /* Player dependent */
    public Countdown InitialCountdown;
    public AmmoQuantity AmmoQty;
    public PowerStatusManager PowerManager;
    public Countdown RespawnCountdown;

	void Start () 
    {
        INetwork network = INetwork.Instance;
        int characterId = network.GetCharacterId();
        int playerId = network.GetId();
        if (!network.IsConnected || characterId == -1)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        GameObject character = network.Instantiate(CharacterPrefabs[characterId], CharacterSpawnPoints[playerId].position, CharacterSpawnPoints[playerId].rotation);
        character.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        character.transform.Find("MainCamera").gameObject.SetActive(true);


        SetPlayerInPlayerDependent(character);

        StateManager.Instance.GoToState("Countdown");
	}


    private void SetPlayerInPlayerDependent(GameObject player)
    {
        Camera playerCamera = player.GetComponentInChildren<Camera>();

        InitialCountdown.SetBlurCamera(playerCamera);
        PowerManager.SetCharacter(player.GetComponent<Character>());
    }
}
