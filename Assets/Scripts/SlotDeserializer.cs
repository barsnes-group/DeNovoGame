using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public class SlotDeserializer : MonoBehaviour
{

    public float x1;
    public float x2;
    public List<float> intensity;

    internal void SetScale(float scale_x, float intensity)
    {
        transform.localScale = new Vector3(scale_x, intensity, 0);
    }

    internal void SetPos(float pos_x, float pos_y)
    {
        throw new NotImplementedException();
    }
}