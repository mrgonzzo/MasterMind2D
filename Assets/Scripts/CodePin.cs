using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodePin : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetColor(Color color)
    {
        sr.color = color;
    }
}
