using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DraggableBox : MonoBehaviour
{


    private float dist;
    private bool dragging = false;
    private Vector3 offset;
    private Transform toDrag;

    // Update is called once per frame
    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            dragging = true;

            Debug.Log("touch pos: " + touch.position + " name: " + gameObject.name);

            this.transform.position = touch.position;
        }

    }


    public void SetScale(float scale_x, float scale_y)
    {
        RectTransform rt = GetComponent<RectTransform>();
        rt.localScale = new Vector3(scale_x, scale_y, 1);
    }

    //private Vector3 _dragOffset;
    //private Camera _cam;

    //[SerializeField] private float _speed = 10;

    //void Awake()
    //{
    //    _cam = Camera.main;
    //}

    //void OnMouseDown()
    //{
    //    print("OnMouseDown");
    //    _dragOffset = transform.position - GetMousePos();
    //}

    //void OnMouseDrag()
    //{
    //    print("OnMouseDrag");
    //    transform.position = Vector3.MoveTowards(transform.position, GetMousePos() + _dragOffset, _speed * Time.deltaTime);
    //}

    //Vector3 GetMousePos()
    //{
    //    var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
    //    mousePos.z = 0;
    //    return mousePos;
    //}
}