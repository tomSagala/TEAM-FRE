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
    public AudioClip GnomeLaugh;

    public static string LUCK = "LUCK";
    public static string BADLUCK = "BADLUCK";
    public static string NEUTRAL = "NEUTRAL";
    public static string CURRENT = NEUTRAL;

    private AudioSource LuckSong;
    private AudioSource NeutralSong;
    private AudioSource BadLuckSong;

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
            {"HorseShoe", HorseShoe},
            {"GnomeLaugh", GnomeLaugh}
        };

        LuckSong = GetComponents<AudioSource>()[0];
        BadLuckSong = GetComponents<AudioSource>()[1];
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

    public void ModifyCurrentSongForAll(string sourceName)
    {
        if (INetwork.Instance.IsMaster())
        {
            INetwork.Instance.RPC(gameObject, "ModifyCurrentSongLocally", PhotonTargets.All, sourceName);
        }
    }

    [PunRPC]
    public void ModifyCurrentSongLocally(string sourceName)
    {
        if (sourceName == NEUTRAL)
        {
            LuckSong.Stop();
            BadLuckSong.Stop();
        }
        else if (sourceName == LUCK)
        {
            LuckSong.Play();
            BadLuckSong.Stop();
        }
        else if (sourceName == BADLUCK)
        {
            LuckSong.Stop();
            BadLuckSong.Play();
        }        
    }
}
