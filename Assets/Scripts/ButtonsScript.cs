using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class ButtonsScript : MonoBehaviour
{
    private bool isReset;
    private List<Tuple<string, float, float>> aminoAcidSequence = new List<Tuple<string, float, float>>();

    private List<Tuple<string, float, float>> GetAminoAcidSequence()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] allBoxes = gameController.GetAllBoxes();
        print("allBoxes.Length " + allBoxes.Length);
        //foreach (DraggableBox box in allBoxes)
        for (int i = 0; i < allBoxes.Length - 1; i++)
        {
            Tuple<string, float, float> aminoAcidInfo;
            DraggableBox box = allBoxes[i];
            float maxX = box.aminoAcidChar.MaxXValue;

            if (box.GetIsPlaced() && box != null)
            {
                //loop though the box's valid slots, to get the start coordinates for the placed box
                JSONReader.SerializedSlot[] slots = box.aminoAcidChar.slots;
                for (int j = 0; j < slots.Length; j++)
                {
                    if (box.GetStartPeak().index == slots[j].start_peak_index)
                    {
                        aminoAcidInfo = Tuple.Create(box.aminoAcidChar.ToString(), slots[j].start_peak_coord * maxX, box.aminoAcidChar.MassOriginal);
                        aminoAcidSequence.Add(aminoAcidInfo);
                        print("aminoAcidInfo: " + aminoAcidInfo);
                    }
                }
            }
        }
        //sort list by the xpos of the box
        aminoAcidSequence.Sort((x, y) => x.Item2.CompareTo(y.Item2));
        return aminoAcidSequence;
    }

    private void ResetAminoAcids()
    {
        //TODO: all boxes return to start pos
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] draggableBoxes = gameController.GetAllBoxes();

        for (int i = 0; i < draggableBoxes.Length; i++)
        {
            DraggableBox box = draggableBoxes[i];

            box.ReturnToStartPos();

        }
        isReset = true;
    }

    private void FindGaps()
    {
        List<Tuple<string, float, float>> placedBoxes = GetAminoAcidSequence();

        for (int i = 0; i < placedBoxes.Count - 1; i++)
        {
            float boxWidth = placedBoxes[i].Item2 + placedBoxes[i].Item3;
            float gapSize = (placedBoxes[i + 1].Item2 - boxWidth);
            if (gapSize > 0)
            {
                print("gapSize: " + gapSize);
                Tuple<string, float, float> gap = Tuple.Create("gap", boxWidth, gapSize);
                aminoAcidSequence.Add(gap);
            }
        }
        aminoAcidSequence.Sort((x, y) => x.Item2.CompareTo(y.Item2));
    }

    private void WriteToCsv()
    {
        string filePath = "Assets/Data/out.csv";
        StreamWriter writer = new StreamWriter(filePath);

        foreach (var seq in aminoAcidSequence)
        {

            writer.WriteLine(seq.Item1 + ", " + seq.Item2 + ", " + seq.Item3);
        }
        writer.Close();
    }


    public void OnGetAminoAcidsClick()
    {
        FindGaps();
        WriteToCsv();
        /*         foreach (Tuple<string, float, float> i in aminoAcidSequence)
                {
                    print("seq: " + i);

                } */

        //print(GetAminoAcidSequence().Count);
        //GetAminoAcidSequence();
    }

    public void OnResetClick()
    {
        ResetAminoAcids();
        print("reset btn clicked");
    }
}
