using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("Combat")]
    [SerializeField] int _slainGoal = 10;
    private int _slainMonsters = 0;
    [SerializeField] Slider _healthBar;

    [Header("Game")]
    [SerializeField] Button _restartButton;
    [SerializeField] Button _quitButton;


    [SerializeField] TMP_Text _enemyText;
    [SerializeField] TMP_Text _winText;
    [SerializeField] TMP_Text _deadText;


    [Header("Inventory")]
    [SerializeField] int targetSlotCount;
    [SerializeField] HorizontalLayoutGroup _inventorySpace;
    [SerializeField] UISlot _slotPrefab;

    private UISlot[] _uiSlots;


    public int InitialiseInventoryUI()
    {
        int inventoryWidth = (int)_inventorySpace.GetComponent<RectTransform>().rect.width;
        int slotWidth = (int)_slotPrefab.GetComponent<RectTransform>().rect.width;

        int maxNumberOfSlots = (inventoryWidth / slotWidth);

        if (inventoryWidth % slotWidth < slotWidth - 1) maxNumberOfSlots--;

        _uiSlots = new UISlot[Mathf.Min(targetSlotCount, maxNumberOfSlots)];

        for (int i = 0; i < _uiSlots.Length; i++)
        {
            var newSlot = _uiSlots[i] = Instantiate(_slotPrefab, _inventorySpace.transform.position, Quaternion.identity, _inventorySpace.transform);
            newSlot.Init();
        }

        return _uiSlots.Length;
    }

    public void UpdateSlot(int index, InventorySlot contents)
    {
        UISlot slot = _uiSlots[index];

        slot.UpdateSlotValues(contents);
    }

    public void UpdateHealth(float health)
    {
        float percentageHealth = health / 100;

        _healthBar.value = percentageHealth;
    }

    public void UpdateDeadMonsters()
    {
        _slainMonsters++;
        _enemyText.text = "Monsters slain: " + _slainMonsters;

        if (_slainMonsters >= _slainGoal)
        {
            _winText.gameObject.SetActive(true);
            _restartButton.gameObject.SetActive(true);
            _quitButton.gameObject.SetActive(true);
        }
    }

    public void DisplayDeadUI()
    {
        _deadText.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _quitButton.gameObject.SetActive(true);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}


