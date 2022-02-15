using System;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private float x1;
    private float x2;
    public List<float> intensity;
    internal Peak startpeak;
    internal Peak endpeak;

    public float setX1(float x1)
    {
        if (x1 > 0)
        {
            return x1;
        }
        throw new NullReferenceException();
    }

    public float setX2(float x2)
    {
        if (x2 > 0)
        {
            return x2;
        }
        throw new NullReferenceException();
    }

    internal void SetScale(float scale_x, float intensity)
    {
        transform.localScale = new Vector3(scale_x, intensity, 0);
    }

    internal void SetPos(float pos_x, float pos_y)
    {
        Vector3 parent_transform = transform.parent.position;
        transform.localPosition = new Vector3((parent_transform.x + pos_x), (parent_transform.y + pos_y), 0);
    }

    internal float GetSlotScaleX()
    {
        float slotXScale = Mathf.Abs(x2 - x1);
        print("slotXScale" + slotXScale);
        if (slotXScale >= 0)
        {
            return slotXScale;
        }
        throw new Exception("slot x scale not a number " + slotXScale);
    }

    internal float GetSlotScaleY()
    {
        float sum = 0;
        foreach (float i in intensity)
        {
            sum += i;
        }
        float avg = sum / intensity.Count;
        if (avg >= 0)
        {
            return avg;
        }
        throw new Exception("slot y scale type: " + avg.GetType());
    }
}
