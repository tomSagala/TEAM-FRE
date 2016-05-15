using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkAudioManager : GameSingleton<NetworkAudioManager>
{
    public AudioClip CatLadyCry;
    public AudioClip HappyCat;
    public AudioClip AggressiveCat;
    public AudioClip HealingQuad;
    public AudioClip FireAoe;
    public AudioClip Bullet;
    public AudioClip MirrorShatter;
    public AudioClip RabbitExplosion;
    public AudioClip HorseShoe;

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
            {"MirrorShatter", MirrorShatter},
            {"RabbitExplosion", RabbitExplosion},
            {"HorseShoe", HorseShoe}
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
