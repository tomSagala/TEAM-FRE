using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterThumbnail : MonoBehaviour
{
    public Sprite[] thumbnails;
    private Image activeThumbnail;

    void Awake()
    {
        activeThumbnail = GetComponent<Image>();
    }

    public void SetCharacterThumbnailFromId(int id)
    {
        activeThumbnail.sprite = thumbnails[id];
    }
}
