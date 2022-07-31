using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    public DropType DropType { get; private set; }
    public MeshRenderer Mesh { get; private set; }
    public ScriptableDrops DropInfo { get; private set; }

    public void Init(ScriptableDrops dropInfo, Vector3 position)
    {
        transform.position = position;

        name = dropInfo.name;
        DropType = dropInfo.Type;
        Mesh = GetComponent<MeshRenderer>();
        Mesh.material = dropInfo.Mat;
        DropInfo = dropInfo;

        StartCoroutine(SendToPool(5f));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventory player))
        {
            if (player.GetComponent<PlayerController>()._canPickUp == false) return;

            if (player.AddToInventory(this) == true)
            {
                StopAllCoroutines();
                StartCoroutine(SendToPool(0f));
            }
                
        }
    }

    // SEND Gameobject back to Pool after time --> (AudioClip duration) has passed
    IEnumerator SendToPool(float time = 0)
    {
        yield return Helper.GetWait(time);

        // RESET SoundObject's parent and position 
        transform.SetParent(DropsPool.Instance.transform);
        transform.SetPositionAndRotation(Vector3.zero + DropsPool.Instance.DropPosition, Quaternion.identity);

        DropsPool.Instance.AddToPool(this); // ADD this SoundObject back into SoundPool to get used later

        gameObject.SetActive(false); // DISABLE this SoundObject (GameObject)
    }
}
