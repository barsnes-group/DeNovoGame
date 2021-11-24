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
            return;
            string[] rows = array[i].Split(',');

            xCoord = float.Parse(rows[0]);
            yCoord = float.Parse(rows[1]);

            //Debug.Log("x: " + xCoord + " y: " + yCoord);
            DrawPeaks(xCoord, yCoord);  
        }
    }


    void DrawPeaks(float x, float y)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = new Vector3(x, 0, 0); 
        cube.transform.localScale = new Vector3(0.4f, 6.0f, 0);

    }
    public void createSlots(JSONReader.AminoAcid[] aminoAcids)
    {
        //sjekke om aa har slot
     
        foreach(JSONReader.AminoAcid a in aminoAcids)
        {
            //Debug.Log("a.slots: " + a.slots.Length);
            if (a.slots.Length > 0)
            {
                Debug.Log("name: " + a.AminoAcidName + " mass: " + a.Mass + " antall slots: " + a.slots.Length);
                foreach (JSONReader.Slot s in a.slots)
                {
                    Debug.Log("x1: " + s.x1 + " x2: " + s.x2);
                }
            }
        }
       
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