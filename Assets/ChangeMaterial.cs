using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] Material MyMaterial;
    [SerializeField] Texture2D[] MyTextures;
    Renderer MyRenderer;

    void Awake()
    {
        MyRenderer = GetComponent<Renderer>();
    }

    void Start()
    {
        MyRenderer.material.mainTexture = MyTextures[Random.Range(0,MyTextures.Length)];
    }

}
