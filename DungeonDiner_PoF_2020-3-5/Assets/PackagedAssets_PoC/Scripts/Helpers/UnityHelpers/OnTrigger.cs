using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class OnTrigger : MonoBehaviour
{
    [Tooltip("ALL Triggers will fire against THIS Tag")]
    [SerializeField] string _tagCheck = "Default";

    [Header("Events")]
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    public UnityEvent onTriggerStay;

    void Awake()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>())
            collider.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(_tagCheck))
            onTriggerEnter.Invoke();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(_tagCheck))
            onTriggerExit.Invoke();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(_tagCheck))
            onTriggerStay.Invoke();
    }
}

