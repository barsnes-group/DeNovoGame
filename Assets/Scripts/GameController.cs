using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public TextAsset CsvFile;
    public GameObject boxPrefab;
    public GameObject slotPrefab;
    [Header("sizes")]
    public float size;
    public float width_space;
    public float x_offset;
    public float y_offset;

    void Start()
    {
        DrawLine();
        string[] array = CsvFile.text.Split('\n');
        for (int i = 0; i <= array.Length-1; i++)
        {
            string[] rows = array[i].Split(',');

            float xCoord = float.Parse(rows[0]);
            //float yCoord = float.Parse(rows[1]);

            DrawPeaks(xCoord, 5,1,1);

        }

    }

    void DrawPeaks(float pos_x, float pos_y, float scale_x, float scale_y)
    {

        GameObject slot = Instantiate(slotPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        slot.GetComponent<Slot>().SetScale(scale_x, scale_y);
        slot.GetComponent<Slot>().SetPos(pos_x, pos_y, x_offset, y_offset);
        slot.transform.SetParent(GameObject.Find("SlotContainer").transform);
        //get int fra slot og sett farge


    }

    void DrawBox(float pos_x, float pos_y, float scale_x, float scale_y)
    {
        GameObject box = Instantiate(boxPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        box.GetComponent<DraggableBox>().SetScale(scale_x, scale_y);
        box.GetComponent<DraggableBox>().SetPos(pos_x, pos_y, x_offset, y_offset);
        box.transform.SetParent(GameObject.Find("BoxContainer").transform);
        //Renderer rend = box.GetComponent<Renderer>();
        //rend.material.color = Color.yellow;
    }

    public void CreateSlots(JSONReader.AminoAcid[] aminoAcids)
    {

        int n = 0;
        foreach(JSONReader.AminoAcid a in aminoAcids)
        {
            if (a.slots.Length > 0)
            {
                //print(a.AminoAcidName + " has mass: " + a.Mass);
                DrawBox(a.Mass + n, 10, a.Mass, a.Mass);
                n += 20;

                foreach (JSONReader.Slot s in a.slots)
                {
                    //Debug.Log("x1: " + s.x1 + " x2: " + s.x2);
                }
            }
        }  
    }


    void DrawLine()
    {
        LineRenderer l = gameObject.AddComponent<LineRenderer>();

        List<Vector3> pos = new List<Vector3>
        {
            new Vector3(-500, 0, 0),
            new Vector3(1000, 0, 0)
        };

        l.startWidth = 0.3f;
        l.SetPositions(pos.ToArray());
        l.useWorldSpace = true;
    }

}