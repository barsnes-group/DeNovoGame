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
        print("OnGetAminoAcidsClick() clicked");
        List<Tuple<string, float, float>> aminoAcidSequence = new List<Tuple<string, float, float>>();
        AminoAcidSequence.GetAminoAcidSequence(aminoAcidSequence);
        AminoAcidSequence.FindGaps(aminoAcidSequence);
        string sequence = AminoAcidSequence.WriteToCsv(aminoAcidSequence, "Assets/Data/amino_acid_seq.csv");
        if (sequence == "" || sequence == null)
        {
            throw new Exception("seq null");
        }
        print(sequence);
        SetText(sequence);
    }

    public void OnResetClick()
    {
        ResetAminoAcids();
    }
}
