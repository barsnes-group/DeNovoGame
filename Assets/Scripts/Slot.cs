using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    public void SetScale(float scale_x, float scale_y)
    {
        this.transform.localScale = new Vector3(scale_x, scale_y, 0);
    }

    public void SetPos(float pos_x, float pos_y)
    {
        Vector3 parent_transform = transform.parent.position;
        transform.localPosition = new Vector3((parent_transform.x + pos_x), (parent_transform.y + pos_y), 0);
    }

    internal void highlight()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;    }
    internal void defaultColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
    }
}
