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

    // Healing Quad
    public AudioClip HealingQuad;

    // FireAoe
    public AudioClip FireAoe;

    // Bullet
    public AudioClip Bullet;

    // Mirror Shatter
    public AudioClip MirrorShatter;


    private Dictionary<string, AudioClip> audioClips;

    void Start()
    {
        audioClips = new Dictionary<string, AudioClip>()
        {
            {"CatLadyCry", CatLadyCry},
            {"HappyCat", HappyCat},
            {"AggressiveCat", AggressiveCat},
            {"HealingQuad", HealingQuad},
            {"FireAoe", FireAoe},
            {"Bullet", Bullet},
            {"MirrorShatter", MirrorShatter}
        };
    }

    public void PlayAudioClipForAll(string clipName, Vector3 position, float volume)
    {
        if (INetwork.Instance.IsMaster())
        {
            INetwork.Instance.RPC(gameObject, "PlayAudioClipLocally", PhotonTargets.All, clipName, position, volume);
        }
    }

    [PunRPC]
    public void PlayAudioClipLocally(string clipName, Vector3 position, float volume)
    {
        AudioClip clip;
        bool clipFound = audioClips.TryGetValue(clipName, out clip);

        if (clipFound)
            AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}
