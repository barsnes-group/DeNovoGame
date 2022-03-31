using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

public class ButtonsScript : MonoBehaviour
{
    private bool isReset;

    //private List<string> seq = new List<string>();

    private List<Tuple<string, float>> GetAminoAcidSequence()
    {
        List<Tuple<string, float>> aminoAcidSequence = new List<Tuple<string, float>>();


        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] allBoxes = gameController.GetAllBoxes();
        foreach (DraggableBox box in allBoxes)
        {
            Tuple<string, float> aminoAcidInfo;

            if (box.GetIsPlaced())
            {
                aminoAcidInfo = Tuple.Create(box.aminoAcidChar.ToString(), box.transform.position.x);

                aminoAcidSequence.Add(aminoAcidInfo);
            }
        }
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
    /* 
        private void WriteToCsv()
        {
            string filePath = "Assets/Data/out.csv";
            StreamWriter writer = new StreamWriter(filePath);

            foreach (var seq in GetAminoAcidSequence())
            {
                if (seq.Count > 0)
                {
                    writer.WriteLine(seq[0] + "," + seq[1]);
                }
            }
            writer.Close();
        } */


    public void OnGetAminoAcidsClick()
    {
        //WriteToCsv();
        foreach (Tuple<string, float> i in GetAminoAcidSequence())
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
