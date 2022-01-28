using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public float x1;
    public float x2;
    public List<float> intensity;

    internal void SetScale(float scale_x, float intensity)
    {
        transform.localScale = new Vector3(scale_x, intensity, 0);
    }
}
