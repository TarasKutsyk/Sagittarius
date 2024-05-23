using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 1f;

    Material myMaterial;
    Vector2 offset;
    
    private void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        offset = new Vector2(0f, backgroundScrollSpeed);
    }

    private void Update()
    {
        Debug.Log($"Update {Time.deltaTime}");
        myMaterial.mainTextureOffset += offset * Time.deltaTime;
    }
}
