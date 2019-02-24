using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    GameObject Player;
    public Animator DoorAnimator;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DoorAnimator.SetTrigger("Open");
            this.gameObject.SetActive(false);
        }
    }

}
