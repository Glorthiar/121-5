using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSelf : MonoBehaviour
{
    float TimeToDisable;
    void OnEnable()
    {
        TimeToDisable = .15f;
    }
    // Update is called once per frame
    void Update()
    {
        TimeToDisable -= 1 * Time.deltaTime;
        if (TimeToDisable <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
