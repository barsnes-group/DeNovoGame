using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public float size;
    public float width_space;


    public void SetScale(float scale_x, float scale_y)
    {
        this.transform.localScale = new Vector3(scale_x, scale_y, 0);
    }

    public void SetPos(float pos_x, float pos_y, float x_offset, float y_offset)
    {
        transform.localPosition = new Vector3(pos_x * width_space + x_offset, pos_y * width_space + y_offset, 0);
    }
}
