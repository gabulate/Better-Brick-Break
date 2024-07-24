using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallArea : MonoBehaviour
{
    static readonly LayerMask ballLayer = 1 << 6;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ballLayer == (ballLayer | (1 << collision.gameObject.layer)))
        {
            collision.GetComponent<BallScript>().Recall();
        }
        
    }
}
