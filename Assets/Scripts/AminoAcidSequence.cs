using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

/**
Class that get the amino acids from the placed boxes 
and calculate the gaps between them.
**/

public static class AminoAcidSequence
{
    public static List<Tuple<string, float, float>> GetAminoAcidSequence(List<Tuple<string, float, float>> aminoAcidSequence)
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] allBoxes = gameController.GetAllBoxes();
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
                        float deNormalizedStartCoord = slots[j].start_peak_coord * maxX;
                        aminoAcidInfo = Tuple.Create(box.aminoAcidChar.ToString(), deNormalizedStartCoord, box.aminoAcidChar.MassOriginal);
                        aminoAcidSequence.Add(aminoAcidInfo);
                    }
                }
            }
        }
        List<float> peaks = new List<float>();
        for (int i = 0; i < gameController.GetLastPeak().index - 1; i++)
        {
            if (gameController.GetPeak(i).coord != 0)
            {
                peaks.Add(gameController.GetPeak(i).coord);
            }
        }

        //sort list by the xpos of the box
        aminoAcidSequence.Sort((x, y) => x.Item2.CompareTo(y.Item2));
        return aminoAcidSequence;
    }

    public static void FindGaps(List<Tuple<string, float, float>> aminoAcidSequence)
    {
        List<Tuple<string, float, float>> placedBoxes = aminoAcidSequence;
        int count = placedBoxes.Count;

        for (int i = 0; i < count - 1; i++)
        {
            float boxEndCoord = placedBoxes[i].Item2 + placedBoxes[i].Item3;
            float gapSize = (placedBoxes[i + 1].Item2 - boxEndCoord);
            if (gapSize > 0.5)
            {
                Tuple<string, float, float> gap = Tuple.Create("gap", boxEndCoord, gapSize);
                aminoAcidSequence.Add(gap);
            }
        }
        aminoAcidSequence.Sort((x, y) => x.Item2.CompareTo(y.Item2));
    }

    public static string WriteToCsv(List<Tuple<string, float, float>> aminoAcidSequence, string filePath)
    {
        StreamWriter writer = new StreamWriter(filePath);
        string sequence = "Sequence: ";
        //get the gap infront of sequence
        sequence += " <" + aminoAcidSequence[0].Item2 + ">, ";

        foreach (var seq in aminoAcidSequence)
        {
            if (seq.Item1 == "gap")
            {
                sequence += " <" + seq.Item3 + ">, ";
                writer.Write(" <" + seq.Item3 + ">, ");
            }
            else
            {
                sequence += seq.Item1 + ", ";
                writer.Write(seq.Item1 + ", ");
            }
        }
        //get the gap at the end of the sequence
        float endCoorinate = (aminoAcidSequence[aminoAcidSequence.Count - 1].Item2 + aminoAcidSequence[aminoAcidSequence.Count - 1].Item3);
        sequence += " <" + endCoorinate + "> ";

        writer.Close();
        return sequence;
    }

}
