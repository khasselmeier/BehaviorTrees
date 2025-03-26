using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyNPC : MonoBehaviour
{
    public string dialougeText = "Hello, traveler!";
    public GameObject dialogueCanvas;

    void Start()
    {
        dialogueCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueCanvas.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            dialogueCanvas.SetActive(false);
        }
    }
}
