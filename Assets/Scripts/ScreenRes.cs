using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: make singletone
public class ScreenRes : MonoBehaviour
{
    [SerializeField] private bool fullScreen = false;
    [SerializeField] private int defaultHeight = 1024;
    [SerializeField] private int defaultWidth = 576;

    void Start()
    {
        Debug.Log("Screen");

        Screen.SetResolution(defaultWidth, defaultHeight, fullScreen, 144);
    }
}
