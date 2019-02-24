using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public GameObject Player;
    public int HP;
    // Start is called before the first frame update
    void Start()
    {
        HP = 3;
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 0.05f);
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
