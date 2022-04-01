using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class ButtonsScript : MonoBehaviour
{
    private bool isReset;

    private List<Tuple<string, float, float>> GetAminoAcidSequence()
    {
        List<Tuple<string, float, float>> aminoAcidSequence = new List<Tuple<string, float, float>>();


        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] allBoxes = gameController.GetAllBoxes();
        foreach (DraggableBox box in allBoxes)
        {
            Tuple<string, float, float> aminoAcidInfo;

            if (box.GetIsPlaced())
            {
                aminoAcidInfo = Tuple.Create(box.aminoAcidChar.ToString(), box.transform.position.x, box.aminoAcidChar.Mass);
                aminoAcidSequence.Add(aminoAcidInfo);
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
            float gapSize = (placedBoxes[i+1].Item2 - (placedBoxes[i].Item2 + placedBoxes[i].Item3));
            if (gapSize > 0)
            {
                print("gapSize: " + gapSize);
            }
        }
    }

    private void WriteToCsv()
    {
        string filePath = "Assets/Data/out.csv";
        StreamWriter writer = new StreamWriter(filePath);

        foreach (var seq in GetAminoAcidSequence())
        {

            writer.WriteLine(seq.Item1 + ", " + seq.Item2 + ", " + seq.Item3);
        }
        writer.Close();
    }


    public void OnGetAminoAcidsClick()
    {
        WriteToCsv();
        FindGaps();
        foreach (Tuple<string, float, float> i in GetAminoAcidSequence())
        {
            print("seq: " + i);

        }

        //print(GetAminoAcidSequence().Count);
        //GetAminoAcidSequence();
    }

    public void OnResetClick()
    {
        ResetAminoAcids();
        print("reset btn clicked");
    }
}
