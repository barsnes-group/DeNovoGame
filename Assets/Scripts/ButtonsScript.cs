using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class ButtonsScript : MonoBehaviour
{
    public TextMeshProUGUI seqText;
    float maxX;

    [SerializeField]
    GameObject aminoSeqText;

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
        List<Tuple<string, float, float>> aminoAcidSequence = new List<Tuple<string, float, float>>();
        if (aminoAcidSequence == null)
        {
            SetText("Sequence: ");
        }
        else
        {
            AminoAcidSequence.GetAminoAcidSequence(aminoAcidSequence);
            AminoAcidSequence.FindGaps(aminoAcidSequence);

            string sequence = AminoAcidSequence.MakeSequence(aminoAcidSequence);
            //AminoAcidSequence.WriteToCsv(sequence, "Assets/Data/amino_acid_seq.csv");
            SetText(sequence);
        }
    }

    public void OnResetClick()
    {
        ResetAminoAcids();
    }
}
