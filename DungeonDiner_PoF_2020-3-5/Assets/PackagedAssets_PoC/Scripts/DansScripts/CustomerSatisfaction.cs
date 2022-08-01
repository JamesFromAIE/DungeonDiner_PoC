using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class CustomerSatisfaction : MonoBehaviour
{
    [SerializeField] Image happyFace;
    [SerializeField] Image neutralFace;
    [SerializeField] Image angryFace;

    [SerializeField] float customerTimer;
    [SerializeField] float neutralTime;
    [SerializeField] float angryTime;

    [SerializeField] Transform customerSeat;
    [SerializeField] Transform resturantExitBox;

    NavMeshAgent agent;

    void Start()
    {   
        agent = GetComponent<NavMeshAgent>();
        agent.destination = customerSeat.position;

        happyFace.enabled = true;
        neutralFace.enabled = false;
        angryFace.enabled = false;
    }

    void Update()
    {
        customerTimer = customerTimer - Time.deltaTime;

        if (customerTimer <= 0)
        {        
            agent.destination = resturantExitBox.position;           
        }

        if (customerTimer <= angryTime)
        {
            happyFace.enabled = false;
            neutralFace.enabled = false;
            angryFace.enabled = true;

        }
        else if (customerTimer <= neutralTime)
        {
            happyFace.enabled = false;
            neutralFace.enabled = true;
            angryFace.enabled = false;
        }

        else
            happyFace.enabled = true;      
      

    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("hi");
        if (other.CompareTag("ResturantExit"))
        {
            Destroy(gameObject);
        }
        
    }
}
