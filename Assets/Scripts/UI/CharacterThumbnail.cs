using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterThumbnail : MonoBehaviour
{
    public Sprite[] thumbnails;
    private Image activeThumbnail;

    public void SetCharacterThumbnailFromId(int id)
    {
        activeThumbnail = GetComponent<Image>();
        activeThumbnail.sprite = thumbnails[id];
    }
}
