using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISlot : MonoBehaviour
{
    public Sprite ItemSprite { get; private set; } = null;
    [SerializeField] Image SpriteImage;
    [SerializeField] TMP_Text CapacityText;

    public void Init()
    {
        //SlotImage = GetComponentInChildren<Image>();
        //CapacityText = GetComponentInChildren<TMP_Text>();
    }

    public void UpdateSlotValues(InventorySlot content)
    {
        CapacityText.text = content.CurrentCapacity.ToString();

        if (content.ItemSprite != null) SpriteImage.color = Color.white;
        else SpriteImage.color = Color.black;

        SpriteImage.sprite = content.ItemSprite;
    }
}
