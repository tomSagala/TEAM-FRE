using UnityEngine;
using UnityEngine.UI;
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
    public LifeBar lifeBar;
    public Text username;
    public Countdown RespawnCountdown;

	void Start () 
    {
        CreatePlayer();
        StateManager.Instance.GoToState("Countdown");
	}

    public void CreatePlayer(bool isRespawn = false)
    {
        INetwork network = INetwork.Instance;
        int characterId = network.GetCharacterId();
        int playerId = network.GetId();
        if (!network.IsConnected || characterId == -1)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }
        string playerName = network.GetPlayerName(); 
        GameObject character = network.Instantiate(CharacterPrefabs[characterId], CharacterSpawnPoints[playerId].position, CharacterSpawnPoints[playerId].rotation);
        string team = (characterId >= 2) ? TeamsEnum.GoodLuckTeam : TeamsEnum.BadLuckTeam;
        character.GetComponent<Character>().SetTeam(team);
        character.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        character.transform.Find("MainCamera").gameObject.SetActive(true);
        character.GetComponent<Character>().SetSpawnPoint(CharacterSpawnPoints[playerId]);
        SetPlayerInPlayerDependent(character, playerName);

        if (isRespawn)
            StateManager.Instance.GoToState("Respawn");
    }


    private void SetPlayerInPlayerDependent(GameObject player, string playerName)
    {
        InitialCountdown.SetCharacter(player.GetComponent<Character>());
        InitialCountdown.Setup();
        RespawnCountdown.SetCharacter(player.GetComponent<Character>());
        RespawnCountdown.Setup();
        PowerManager.SetCharacter(player.GetComponent<Character>());
        lifeBar.SetCharacter(player.GetComponent<Character>());
        lifeBar.Setup();
        username.text = playerName;
    }
}
