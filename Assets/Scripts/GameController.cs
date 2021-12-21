using UnityEngine;
using System.Collections.Generic;
using System;

public class GameController : MonoBehaviour
{
    public TextAsset CsvFile;
    public GameObject boxPrefab;
    public GameObject slotPrefab;
    [Header("Sizes")]
    public float slotAndBoxScaling;
    public float peaksYPos = 15;
    public float boxYPos = 15;
    public float scaleWidth = 0.1f;
    private float rightBoxMargin = 1;
    [SerializeField]
    GameObject SlotContainer;

    void Start()
    {
        DrawLine();
        string[] array = CsvFile.text.Split('\n');
        for (int i = 0; i <= array.Length-1; i++)
        {
            string[] rows = array[i].Split(',');

            float xCoord = float.Parse(rows[0]);
            //float yCoord = float.Parse(rows[1]);

            DrawPeaks(xCoord, peaksYPos, 0.2f,1);

        }

    }

    internal void SetHighlight(List<int> startIndexes, List<int> endIndexes, bool enabled)
    {
        foreach (int i in startIndexes)
        {
            Slot slot = GetPeak(i).GetComponent<Slot>();
            if (enabled) { slot.highlight(); } else { slot.defaultColor(); }

        }
        foreach (int i in endIndexes)
        {
            Slot slot = GetPeak(i).GetComponent<Slot>();
            if (enabled) { slot.highlight(); } else { slot.defaultColor(); }


        }
    }

    Slot DrawPeaks(float pos_x, float pos_y, float scale_x, float scale_y)
    {

        GameObject slotObject = Instantiate(slotPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        slotObject.transform.SetParent(GameObject.Find("SlotContainer").transform);

        Slot slot = slotObject.GetComponent<Slot>();
        slot.SetScale(scale_x * slotAndBoxScaling, scale_y * slotAndBoxScaling);
        slot.SetPos(pos_x * scaleWidth, pos_y);
        return slot;
        //get int fra slot og sett farge


    }

    public GameObject GetPeak(int number)
    {
        return SlotContainer.transform.GetChild(number).gameObject;
    }

    DraggableBox DrawBox(float pos_x, float pos_y, float scale_x, float scale_y)
    {
        GameObject boxObject = Instantiate(boxPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        boxObject.transform.SetParent(GameObject.Find("BoxContainer").transform);

        DraggableBox box = boxObject.GetComponent<DraggableBox>();
        box.SetScale(scale_x * slotAndBoxScaling * scaleWidth, scale_y * slotAndBoxScaling);
        box.SetPos(pos_x * scaleWidth, pos_y);
        return box;
        
    }

    public void CreateSlots(JSONReader.AminoAcid[] aminoAcids)
    {

        foreach(JSONReader.AminoAcid aminoAcidChar in aminoAcids)
        {
            if (aminoAcidChar.slots.Length > 0)
            {
                DraggableBox box = DrawBox(aminoAcidChar.Mass + rightBoxMargin, boxYPos, aminoAcidChar.Mass, aminoAcidChar.Mass);
                rightBoxMargin += 20;
                foreach (JSONReader.Slot slot in aminoAcidChar.slots)
                {
                    box.startIndexes.Add(slot.start_peak_index);
                    box.endIndexes.Add(slot.end_peak_index);

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
            new Vector3(-500, peaksYPos, 0),
            new Vector3(1000, peaksYPos, 0)
        };

        l.startWidth = 0.1f;
        l.SetPositions(pos.ToArray());
        l.useWorldSpace = true;
    }

}