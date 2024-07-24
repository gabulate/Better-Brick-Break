using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsHolder : MonoBehaviour
{
    public static AssetsHolder Instance;
    public Gradient bColors;
    public GameObject blockPrefab;
    public GameObject ballPickUpPrefab;
    public GameObject ballPrefab;
    public GameObject ballVisual;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
