using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerSatisfaction : MonoBehaviour
{
    [SerializeField] Image happyFace;
    [SerializeField] Image neutralFace;
    [SerializeField] Image angryFace;

    [SerializeField] float customerTimer;
    [SerializeField] float neutralTime;
    [SerializeField] float angryTime;



    void Start()
    {       

        happyFace.enabled = true;
        neutralFace.enabled = false;
        angryFace.enabled = false;

    }


    void Update()
    {
        customerTimer = customerTimer - Time.deltaTime;

        if (customerTimer <= 0)
        {
            Destroy(gameObject);
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
        
        Debug.Log(customerTimer);

    }
}
