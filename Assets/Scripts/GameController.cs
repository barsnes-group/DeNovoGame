using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public TextAsset CsvFile;

    float xCoord;
    float yCoord;

    void Start()
    {
        DrawLine();
        string[] array = CsvFile.text.Split('\n');
        for (int i = 0; i <= array.Length-1; i++)
        {
            string[] rows = array[i].Split(',');

            xCoord = float.Parse(rows[0]);
            yCoord = float.Parse(rows[1]);

            Debug.Log("x: " + xCoord/10 + " y: " + yCoord/10);
            DrawObj(xCoord/10, yCoord/10);
        }
    }

    void DrawObj(float x, float y)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3((x*2)-30, 0, 0); //will change this
        cube.transform.localScale = new Vector3(0.08f, 1.0f, 0);


        
    }

    void DrawLine()
    {
        LineRenderer l = gameObject.AddComponent<LineRenderer>();

        List<Vector3> pos = new List<Vector3>
        {
            new Vector3(-500, 0),
            new Vector3(500, 0)
        };

        l.startWidth = 0.05f;
        l.SetPositions(pos.ToArray());
        l.useWorldSpace = true;
    }
}