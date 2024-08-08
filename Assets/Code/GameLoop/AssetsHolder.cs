using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsHolder : MonoBehaviour
{
    public static AssetsHolder Instance;
    [Header("Prefabs")]
    public Gradient bColors;
    public GameObject blockPrefab;
    public GameObject ballPickUpPrefab;
    public GameObject ballPrefab;
    public GameObject ballVisual;
    public GameObject destroyParticles;

    [Header("Sounds")]
    public SoundClip breakSound;
    public SoundClip wallBounceSound;
    public SoundClip blockBounceSound;
    public SoundClip extraBallSound;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
