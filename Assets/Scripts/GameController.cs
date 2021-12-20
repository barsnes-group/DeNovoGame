using UnityEngine;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public TextAsset CsvFile;
    public GameObject boxPrefab;
    public GameObject slotPrefab;
    [Header("Sizes")]
    public float size;
    public float width_space;
    public float peaks_y = 15;
    public float box_y = 15;
    public float space_boxes = 10;

    void Start()
    {
        DrawLine();
        string[] array = CsvFile.text.Split('\n');
        for (int i = 0; i <= array.Length-1; i++)
        {
            string[] rows = array[i].Split(',');

            float xCoord = float.Parse(rows[0]);
            //float yCoord = float.Parse(rows[1]);

            DrawPeaks(xCoord, peaks_y, 0.2f,1);

        }

    }

    void DrawPeaks(float pos_x, float pos_y, float scale_x, float scale_y)
    {

        GameObject slot = Instantiate(slotPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        slot.GetComponent<Slot>().SetScale(scale_x * size, scale_y * size);
        slot.transform.SetParent(GameObject.Find("SlotContainer").transform);
        slot.GetComponent<Slot>().SetPos(pos_x * width_space, pos_y);
        //get int fra slot og sett farge


    }

    void DrawBox(float pos_x, float pos_y, float scale_x, float scale_y)
    {
        GameObject box = Instantiate(boxPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        box.GetComponent<DraggableBox>().SetScale(scale_x * size, scale_y * size);
        box.transform.SetParent(GameObject.Find("BoxContainer").transform);
        box.GetComponent<DraggableBox>().SetPos(pos_x * width_space, pos_y);
    }

    public void CreateSlots(JSONReader.AminoAcid[] aminoAcids)
    {

        foreach(JSONReader.AminoAcid a in aminoAcids)
        {
            if (a.slots.Length > 0)
            {
                //print(a.AminoAcidName + " has mass: " + a.Mass);
                DrawBox(a.Mass + space_boxes, box_y, a.Mass, a.Mass);
                space_boxes += 20;

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

        l.transform.SetParent(GameObject.Find("SlotContainer").transform);


        List<Vector3> pos = new List<Vector3>
        {
            new Vector3(-500, peaks_y, 0),
            new Vector3(1000, peaks_y, 0)
        };

        l.startWidth = 0.1f;
        l.SetPositions(pos.ToArray());
        l.useWorldSpace = true;
    }

}