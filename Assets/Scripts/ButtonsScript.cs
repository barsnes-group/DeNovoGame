using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class ButtonsScript : MonoBehaviour
{
    public TextMeshProUGUI seqText;
    public GameController gameController;
    float maxX;

    public void SetText(string text)
    {
        seqText.text = text.ToString();
    }

    private void ResetAminoAcids()
    {
        //GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] draggableBoxes = gameController.GetAllBoxes();

        for (int i = 0; i < draggableBoxes.Length; i++)
        {
            DraggableBox box = draggableBoxes[i];

            box.ReturnToStartPos();
        }
    }

    //This updates the amino acid sequence each frame
    public void Update()
    {
        OnGetAminoAcidsClick();
    }

    public void OnGetAminoAcidsClick()
    {
        //GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();

        List<Tuple<string, float, float>> aminoAcidSequence = new List<Tuple<string, float, float>>();
        if (aminoAcidSequence == null)
        {
            SetText("Sequence: ");
        }
        else
        {
            AminoAcidSequence.GetAminoAcidSequence(aminoAcidSequence, gameController);
            AminoAcidSequence.FindGaps(aminoAcidSequence);

            AminoAcidSequence aaSeq = new AminoAcidSequence();
            string sequence = aaSeq.MakeSequence(aminoAcidSequence, gameController);
            //AminoAcidSequence.WriteToCsv(sequence, "Assets/Data/amino_acid_seq.csv");
            SetText(sequence);
        }
    }

    public void OnResetClick()
    {
        ResetAminoAcids();
    }
}
