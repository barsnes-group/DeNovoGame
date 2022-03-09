using System;
using TMPro;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private float x1;
    private float x2;
    private float intensity;
    internal Peak startpeak;
    internal Peak endpeak;

    public void setX1(float x1)
    {
        if (x1 > 0)
        {
            this.x1 = x1;
        }
        throw new NullReferenceException();
    }

    private void setIntensity(float intensity)
    {
        if (intensity == null)
        {
            throw new ArgumentException();
        }
        this.intensity = intensity;
    }

    public float getIntensity()
    {
        if (intensity == null)
        {
            throw new ArgumentException();
        }
        return intensity;
    }

    public void setX2(float x2)
    {
        if (x2 > 0)
        {
            this.x2 = x2;
        }
        throw new NullReferenceException();
    }

    internal void SetScale(float scale_x, float intensity)
    {
        if (scale_x == null || scale_x == 0)
        {
            throw new ArgumentException();
        }
        setIntensity(intensity);
        transform.localScale = new Vector3(scale_x, intensity, 0);
    }

    internal void SetPos(float pos_x, float pos_y)
    {
        Vector3 parent_transform = transform.parent.position;
        transform.localPosition = new Vector3((parent_transform.x + pos_x), (parent_transform.y + pos_y), 1);
    }

    internal float GetSlotScaleX()
    {
        return transform.localScale.x;
    }

    internal float GetSlotScaleY()
    {
        float avg = intensity;
        if (avg >= 0)
        {
            return avg;
        }
        throw new Exception("slot y scale type: " + avg.GetType());
    }

    public void SetColor(Color color)
    {
        GetComponentInChildren<SpriteRenderer>().color = color;
    }

    public void SetText(string text)
    {
        GetComponent<TextMeshPro>().text = text.ToString();
    }
}