using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using TMPro;

public class ButtonsScript : MonoBehaviour
{
    public TextMeshProUGUI seqText;
    bool sequenceBtnClicked;
    float maxX;

    [SerializeField]
    GameObject aminoSeqText;

    private List<Tuple<string, float, float>> GetAminoAcidSequence(List<Tuple<string, float, float>> aminoAcidSequence)
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] allBoxes = gameController.GetAllBoxes();
        print("allBoxes.Length " + allBoxes.Length);
        //foreach (DraggableBox box in allBoxes)
        for (int i = 0; i < allBoxes.Length - 1; i++)
        {
            Tuple<string, float, float> aminoAcidInfo;
            DraggableBox box = allBoxes[i];
            maxX = box.aminoAcidChar.MaxXValue;

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
                        print("aminoAcidInfo: " + aminoAcidInfo);
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

    private void FindGaps(List<Tuple<string, float, float>> aminoAcidSequence)
    {
        List<Tuple<string, float, float>> placedBoxes = aminoAcidSequence;
        int count = placedBoxes.Count;

        for (int i = 0; i < count - 1; i++)
        {
            float boxEndCoord = placedBoxes[i].Item2 + placedBoxes[i].Item3;
            float gapSize = (placedBoxes[i + 1].Item2 - boxEndCoord);
            if (gapSize > 0.5)
            {
                print("gapSize: " + gapSize);
                Tuple<string, float, float> gap = Tuple.Create("gap", boxEndCoord, gapSize);
                aminoAcidSequence.Add(gap);
            }
        }
        aminoAcidSequence.Sort((x, y) => x.Item2.CompareTo(y.Item2));
    }

    private string WriteToCsv(List<Tuple<string, float, float>> aminoAcidSequence, string filePath)
    {
        StreamWriter writer = new StreamWriter(filePath);
        string sequence = "Sequence: ";
        //get the gap infront of sequence
        sequence += " <" + aminoAcidSequence[0].Item2 + "> ";

        foreach (var seq in aminoAcidSequence)
        {

            //writer.WriteLine(seq.Item1 + ", " + seq.Item2 + ", " + seq.Item3);

            if (seq.Item1 == "gap")
            {
                sequence += " <" + seq.Item3 + "> ";
                writer.Write(" <" + seq.Item3 + ">, ");
            }
            else
            {
                sequence += seq.Item1 + " ";
                writer.Write(seq.Item1 + ", ");
            }
        }
        //get the gap at the end of the sequence
        float endCoorinate = (aminoAcidSequence[aminoAcidSequence.Count - 1].Item2 + aminoAcidSequence[aminoAcidSequence.Count - 1].Item3);
        sequence += " <" + endCoorinate + "> ";

        writer.Close();
        return sequence;
    }

    private void SetText(string text)
    {
        seqText.text = text.ToString();
    }

    private void ResetAminoAcids()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] draggableBoxes = gameController.GetAllBoxes();

        for (int i = 0; i < draggableBoxes.Length; i++)
        {
            DraggableBox box = draggableBoxes[i];

            box.ReturnToStartPos();
        }
    }
/*     public void Update()
    {
        OnGetAminoAcidsClick();
    } */

    public void OnGetAminoAcidsClick()
    {
        sequenceBtnClicked = true;
        List<Tuple<string, float, float>> aminoAcidSequence = new List<Tuple<string, float, float>>();
        GetAminoAcidSequence(aminoAcidSequence);
        FindGaps(aminoAcidSequence);
        string sequence = WriteToCsv(aminoAcidSequence, "Assets/Data/amino_acid_seq.csv");
        SetText(sequence);
    }

    public void OnResetClick()
    {
        ResetAminoAcids();
    }
}
