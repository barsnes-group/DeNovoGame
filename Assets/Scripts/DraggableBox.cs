using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
//https://gist.github.com/Matthew-J-Spencer/65aea3d55f1e2c6ccb2c3586bccbdbdb 
public class DraggableBox : MonoBehaviour
{

    public List<int> startIndexes;
    public List<int> endIndexes;
    public List<float> startCoord;
    public List<float> endCoord;

    public string width;

    private Vector3 _dragOffset;
    private Camera _cam;
    private Vector3 startPos;
    [SerializeField] private float _speed = 10;
    [SerializeField] private GameObject textObject;
    private float boxToSlotTheshold = 5;

    void Awake()
    {
        _cam = Camera.main;
    }

    private String indexesToString() {
        String str = "";
        foreach (var s in startIndexes) {
            str += s+", ";
        }
        str += "\n endpoints:";
        foreach (var s in endIndexes)
        {
            str += s+", ";
        }
        return str;
    }



    void OnMouseDown()
    {
        print("box can be placed in " + indexesToString());
        _dragOffset = transform.position - GetMousePos();
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.SetHighlight(startIndexes, endIndexes, true);
    }

    private void OnMouseUp()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        gameController.SetHighlight(startIndexes, endIndexes, false);
        //check if close enough to start index, snap into position
        foreach (int i in startIndexes)
        {
            Vector2 peakPos = gameController.GetPeak(i).transform.position;
            if (Vector2.Distance(peakPos, transform.position) < boxToSlotTheshold)
            {
                SnapPosition(peakPos);
                gameController.UpdateScore(this);
                return;
            }

        }
        ReturnToStartPos();
    }

    private void ReturnToStartPos()
    {
        transform.position = startPos;
    }

    private void SnapPosition(Vector2 peakPos)
    {
        transform.position = peakPos;
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
        startPos = transform.position;
    }
    private void Update()
    {
        SetText(width);
    }
    internal void SetText(string text)
    {
        textObject.GetComponent<TextMeshProUGUI>().text = text.ToString();
    }
}