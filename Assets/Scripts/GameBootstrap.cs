using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameBootstrap : MonoBehaviour
{
    public List<GameObject> CharacterPrefabs;
    public List<Transform> CharacterSpawnPoints;

	void Start () 
    {
        INetwork network = INetwork.Instance;
        int characterId = network.GetCharacterId();
        if (!network.IsConnected || characterId == -1)
        {
            SceneManager.LoadScene("MainMenu");
            return;
        }

        network.Instantiate(CharacterPrefabs[characterId], CharacterSpawnPoints[characterId].position, CharacterSpawnPoints[characterId].rotation);
	}
}
