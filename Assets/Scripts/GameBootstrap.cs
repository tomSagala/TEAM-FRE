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

        GameObject character = network.Instantiate(CharacterPrefabs[characterId], CharacterSpawnPoints[characterId].position, CharacterSpawnPoints[characterId].rotation);
        character.GetComponent<RigidbodyFirstPersonController>().enabled = true;
        character.transform.Find("MainCamera").gameObject.SetActive(true);
	}
}
