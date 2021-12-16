using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    public void SetScale(float scale_x, float scale_y)
    {
        this.transform.localScale = new Vector3(scale_x, scale_y, 0);
    }
}
