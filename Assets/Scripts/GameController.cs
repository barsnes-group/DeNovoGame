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
        //DrawAminoAcids();
        string[] array = CsvFile.text.Split('\n');
        for (int i = 0; i <= array.Length-1; i++)
        {
            string[] rows = array[i].Split(',');

            xCoord = float.Parse(rows[0]);
            yCoord = float.Parse(rows[1]);

            Debug.Log("x: " + xCoord/10 + " y: " + yCoord/10);
            DrawPeaks(xCoord/10, yCoord/10);
        }
    }

    void DrawPeaks(float x, float y)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(x, 0, 0); 
        cube.transform.localScale = new Vector3(0.4f, 6.0f, 0);

    }

    void DrawLine()
    {
        LineRenderer l = gameObject.AddComponent<LineRenderer>();

        List<Vector3> pos = new List<Vector3>
        {
            new Vector3(0, 0),
            new Vector3(1000, 0)
        };

        l.startWidth = 0.3f;
        l.SetPositions(pos.ToArray());
        l.useWorldSpace = true;
    }

}