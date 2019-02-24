using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveAmmo : MonoBehaviour
{
    GameObject Player;
    [SerializeField]
    GameObject particles;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Player.GetComponent<PlayerController>().Ammo += 15;
            Instantiate(particles,this.transform.position,this.transform.rotation);
            this.gameObject.SetActive(false);
        }
    }

}
