using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class GameController : MonoBehaviour
{
    public TextAsset CsvFile;
    public GameObject boxPrefab;
    public GameObject peakPrefab;
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
        float previousX = 0;
        for (int i = 0; i <= array.Length-1; i++)
        {
            string[] rows = array[i].Split(',');

            float xCoord = float.Parse(rows[0]);
            //float yCoord = float.Parse(rows[1]);

            CreatePeakPrefab(xCoord, peaksYPos, 0.2f,1, xCoord - previousX, i);
            previousX = xCoord;
        }
    }

    SlotDeserializer CreateSlotPrefab(float pos_x1, float pos_x2, float pos_y, float intensity)
    {
        GameObject slotObject = Instantiate(slotPrefab, new Vector3(pos_x1, pos_y, 0), Quaternion.identity);
        slotObject.transform.SetParent(GameObject.Find("SlotContainer").transform);

        SlotDeserializer slot = slotObject.GetComponent<SlotDeserializer>();
        slot.SetScale(MathF.Abs(pos_x2-pos_x1), intensity);
        slot.SetPos(pos_x1, pos_y);
        return slot;
    }

    Peak CreatePeakPrefab(float pos_x, float pos_y, float scale_x, float scale_y, float width_to_prev, int index)
    {
        GameObject peakObject = Instantiate(peakPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        peakObject.transform.SetParent(GameObject.Find("SlotContainer").transform);

        Peak peak = peakObject.GetComponent<Peak>();
        peak.SetImageScale(scale_x * slotAndBoxScaling, scale_y * slotAndBoxScaling);
        peak.SetPos(pos_x * scaleWidth, pos_y);
        peak.SetText(index + "\n" + width_to_prev.ToString());
        return peak;
    }

    DraggableBox CreateBoxPrefab(float pos_x, float pos_y, float scale_x, float scale_y)
    {
        GameObject boxObject = Instantiate(boxPrefab, new Vector3(pos_x, pos_y, 0), Quaternion.identity);
        boxObject.transform.SetParent(GameObject.Find("BoxContainer").transform);

        DraggableBox box = boxObject.GetComponent<DraggableBox>();
        box.width = scale_x.ToString();
        box.SetScale(scale_x * scaleWidth, scale_y * slotAndBoxScaling);
        box.SetPos(pos_x * scaleWidth, pos_y); //*slotAndBoxScaling
        return box;

    }

    internal void HighlightValidSlot(List<int> startIndexes, List<int> endIndexes, bool enabled)
    {
        // (var startAndEndIndexes in startIndexes.Zip(endIndexes, Tuple.Create)
        foreach (var startAndEndIndexes in startIndexes.Zip(endIndexes, Tuple.Create))
        {
            Peak x1 = GetPeak(startAndEndIndexes.Item1);
            Peak x2 = GetPeak(startAndEndIndexes.Item2);
            Vector2 x1_pos = x1.transform.position;
            Vector2 x2_pos = x2.transform.position;
            if (enabled)
            {
                print("x1 & x2: " + x1_pos.x + ", " + x2_pos.x + "dist: " + MathF.Abs(x2_pos.x-x1_pos.x));
                //tegn box mellom x1 og x2 koord
                CreateSlotPrefab(x1_pos.x, x2_pos.x, peaksYPos, 10);
            }
        

    
        }
    }

    internal void SetHighlight(List<int> startIndexes, List<int> endIndexes, bool enabled)
    {
        foreach (int i in startIndexes)
        {
            Peak slot = GetPeak(i);
            if (enabled) { slot.Highlight(); } else { slot.DefaultColor(); }
        }
        foreach (int i in endIndexes)
        {
            Peak slot = GetPeak(i);
            if (enabled) { slot.Highlight(); } else { slot.DefaultColor(); }

        }
    }
    
    internal void UpdateScore(DraggableBox draggableBox)
    {
        throw new NotImplementedException();
    }

    public Peak GetPeak(int index)
    {
        GameObject peakIndex = SlotContainer.transform.GetChild(index).gameObject;
        Peak peak = peakIndex.GetComponent<Peak>();

        if (peakIndex == null || peak == null)
        {
            throw new Exception("peak index or peak = null");
        }
        return peak;
    }

    public void CreateBoxes(JSONReader.AminoAcid[] aminoAcids)
    {

        foreach(JSONReader.AminoAcid aminoAcidChar in aminoAcids)
        {
            if (aminoAcidChar.slots.Length > 0)
            {
                DraggableBox box = CreateBoxPrefab(aminoAcidChar.Mass + rightBoxMargin, boxYPos, aminoAcidChar.Mass, aminoAcidChar.Mass);
                rightBoxMargin += 15;
                foreach (JSONReader.SerializedSlot slot in aminoAcidChar.slots)
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

        //l.transform.SetParent(GameObject.Find("SlotContainer").transform);

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