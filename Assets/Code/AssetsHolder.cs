using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsHolder : MonoBehaviour
{
    public static AssetsHolder Instance;
    public Gradient bColors;
    public GameObject blockPrefab;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
