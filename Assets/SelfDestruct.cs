using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float time;
    // Start is called before the first frame update

    void Update()
    {
        time -=  Time.deltaTime;
        if (time <= 0)
        {
            DestroyGameObject();
        }
    }

    // Update is called once per frame
    void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }

}
