using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DraggableBox : MonoBehaviour
{

    public List<int> startIndexes;
    public List<int> endIndexes;

    private Vector3 _dragOffset;
    private Camera _cam;

    [SerializeField] private float _speed = 10;

    void Awake()
    {
        _cam = Camera.main;
    }

    void OnMouseDown()
    {
        _dragOffset = transform.position - GetMousePos();
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.SetHighlight(startIndexes, endIndexes, true);
    }
    private void OnMouseUp()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.SetHighlight(startIndexes, endIndexes, false);
    }

    void OnMouseDrag()
    {
        transform.position = Vector3.MoveTowards(transform.position, GetMousePos() + _dragOffset, _speed * Time.deltaTime);
    }

    Vector3 GetMousePos()
    {
        var mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 1;
        return mousePos;
    }





    public void SetScale(float scale_x, float scale_y)
    {
           transform.localScale = new Vector3(scale_x, scale_y, 0);
    }

    public void SetPos(float pos_x, float pos_y)
    {
        Vector3 parent_transform = transform.parent.position;
        transform.localPosition = new Vector3((parent_transform.x + pos_x), (parent_transform.y + pos_y), 0);
    }

    
}