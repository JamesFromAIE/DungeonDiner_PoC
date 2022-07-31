using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerInventory : MonoBehaviour
{
    private PlayerController _player;

    [SerializeField] int _maxSlotCapacity;
    public InventorySlot[] Inventory { get; private set; }

    void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    void Start()
    {
        InitialiseInventory();
    }

    public void InitialiseInventory()
    {
        int slotLength = UIManager.Instance.InitialiseInventoryUI();

        Inventory = new InventorySlot[slotLength];

        for (int i = 0; i < Inventory.Length; i++)
        {
            Inventory[i] = new InventorySlot(_maxSlotCapacity);
        }
    }

    public bool AddToInventory(EnemyDrop drop)
    {
        //Debug.Log(drop.name + " pick up.");

        for (int i = 0; i < Inventory.Length; i++)
        {
            InventorySlot slot = Inventory[i];

            if (slot.AddDropIfAble(drop))
            {
                UIManager.Instance.UpdateSlot(i, slot);
                return true;
            }
                
        }

        Debug.Log("Inventory is Full.");
        return false;
    }

    public void DropEntireInventory()
    {
        for (int i = 0; i < Inventory.Length; i++)
        {
            InventorySlot slot = Inventory[i];

            if (slot.DropInfo != null)
            {
                int dropNumber = slot.CurrentCapacity;

                for (int j = 0; j < dropNumber; j++)
                {
                    EnemyDrop droppedItem = DropsPool.Instance.GetDropObject();

                    droppedItem.Init(slot.DropInfo, _player.transform.position + Random.insideUnitCircle.XYToXZ() * 2);
                }
            }

            slot.RemoveAllContents();

            UIManager.Instance.UpdateSlot(i, slot);
        }
    }
}

public class InventorySlot
{
    public int SlotCapacity = 9;

    public int CurrentCapacity = 0;

    public DropType SlotType = DropType.Default;

    public Sprite ItemSprite = null;

    public ScriptableDrops DropInfo = null;

    public InventorySlot(int slotCapacity)
    {
        SlotCapacity = slotCapacity;
    }

    #region Public Methods
    public bool AddDropIfAble(EnemyDrop drop)
    {
        if (SlotType == DropType.Default)
        {
            AddDropAndType(drop);
            Debug.Log("Added First Drop");
            return true;
        }
        else if (AreDropTypesMatching(drop) && IsThereSpace())
        {
            AddDrop();
            Debug.Log("Added Drop");
            return true;
        }

        return false;
    }

    public bool RemoveOneContent()
    {
        if (CurrentCapacity <= 0) return false;

        CurrentCapacity--;
        Debug.Log("Removed one drop.");

        NullifyTypeIfEmpty();

        return true;
    }

    public bool RemoveAllContents()
    {
        if (CurrentCapacity <= 0) return false;

        CurrentCapacity = 0;
        Debug.Log("Removed ALL drops.");

        NullifyTypeIfEmpty();

        return true;
    }

    #endregion

    #region Private Methods
    bool IsThereSpace() { return CurrentCapacity < SlotCapacity; }
    bool AreDropTypesMatching(EnemyDrop drop) { return drop.DropType == SlotType; }

    void AddDrop() { CurrentCapacity++; }

    void AddDropAndType(EnemyDrop drop)
    {
        CurrentCapacity++;
        SlotType = drop.DropType;
        ItemSprite = drop.DropInfo.Sprite;
        DropInfo = drop.DropInfo;
    }

    bool NullifyTypeIfEmpty()
    {
        if (CurrentCapacity <= 0)
        {
            SlotType = DropType.Default;
            ItemSprite = null;
            DropInfo = null;
            return true;
        }

        return false;
    }

    #endregion

}
