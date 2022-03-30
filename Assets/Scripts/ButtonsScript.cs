using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ButtonsScript : MonoBehaviour
{
    private bool isReset;

    //private List<string> seq = new List<string>();

    private List<List<string>> GetAminoAcidSequence()
    {
        List<List<string>> aminoAcidSequence = new List<List<string>>();

        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] allBoxes = gameController.GetAllBoxes();
        foreach (DraggableBox box in allBoxes)
        {
            List<string> aminoAcidInfo = new List<string>();

            if (box.GetIsPlaced())
            {
                aminoAcidInfo.Add(box.aminoAcidChar.ToString());
                aminoAcidInfo.Add(box.transform.position.x.ToString());
            }

            if (aminoAcidInfo.Count > 0)
            {
                aminoAcidSequence.Add(aminoAcidInfo);
                print("aminoAcidInfo: " + aminoAcidInfo[1]);
                //aminoAcidSequence.Sort((x, y) => string.Compare());
            }
        }
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
    }


    public void OnGetAminoAcidsClick()
    {
        WriteToCsv();
        /*         foreach (var seq in GetAminoAcidSequence())
                {
                    //print("seq: " + seq);
                    foreach (var aa in seq)
                    {
                        print("aa: " + aa.ToString());
                    }
                }
        */
        print(GetAminoAcidSequence().Count);

    }

    public void OnResetClick()
    {
        ResetAminoAcids();
        print("reset btn clicked");
    }
}
