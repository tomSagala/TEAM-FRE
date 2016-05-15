using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkAudioManager : GameSingleton<NetworkAudioManager>
{
    // Cat lady
    public AudioClip CatLadyCry;

    // Cats
    public AudioClip HappyCat;
    public AudioClip AggressiveCat;

    private Dictionary<string, AudioClip> audioClips;

    void Start()
    {
        audioClips = new Dictionary<string, AudioClip>()
        {
            {"CatLadyCry", CatLadyCry},
            {"HappyCat", HappyCat},
            {"AggressiveCat", AggressiveCat}
        };
    }

    public void PlayAudioClipForAll(string clipName, Vector3 position)
    {
        if (INetwork.Instance.IsMaster())
        {
            INetwork.Instance.RPC(gameObject, "PlayAudioClipLocally", PhotonTargets.All, clipName, position);
        }
    }

    [PunRPC]
    public void PlayAudioClipLocally(string clipName, Vector3 position)
    {
        AudioClip clip;
        bool clipFound = audioClips.TryGetValue(clipName, out clip);

        if (clipFound)
            AudioSource.PlayClipAtPoint(clip, position);
    }
}
