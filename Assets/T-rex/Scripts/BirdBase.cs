using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdBase : CactusBase
{
    private void OnEnable()
    {
        int y = Random.Range(1, 3);
        startPosition.y = y;
        transform.position = startPosition;
    }
}
