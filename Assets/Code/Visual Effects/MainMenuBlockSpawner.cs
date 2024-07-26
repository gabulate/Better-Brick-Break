using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBlockSpawner : MonoBehaviour
{
    
    [SerializeField]
    private float cooldown = 1;
    [SerializeField]
    private float current = 0;
    [SerializeField]
    private GameObject blockPrefab;

    void Update()
    {
        if(current > 0)
        {
            current -= Time.deltaTime;
        }
        else
        {
            SpawnBlock();
            current = cooldown;
        }
    }

    private void Start()
    {
        foreach(BallScript b in FindObjectsOfType<BallScript>())
        {
            b.isBalling = true;
        }
    }

    void SpawnBlock()
    {
        Vector2 position = new Vector2(Random.Range(-3, 4), 6);

        GameObject g = Instantiate(blockPrefab, position, Quaternion.identity, transform);
        Block bs = g.GetComponent<Block>();
        int[] pos = { 0, 0 };
        bs.SetBlock(Random.Range(1, 8), pos);
        StartCoroutine(MovePosition(bs.transform, new Vector2(position.x, position.y - 16), 16));
    }

    public IEnumerator MovePosition(Transform block, Vector3 position, float seconds)
    {
        Vector3 previousPosition = block.position;

        float step = 1 / seconds;
        float current = 0;

        
        while (current < 1 && block)
        {
            current += step * Time.deltaTime;
            block.position = Vector3.Lerp(previousPosition, position, current);
            yield return null;
        }

        if(block)
            Destroy(block.gameObject);
    }
}
