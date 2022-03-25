using UnityEngine;
using System.Collections.Generic;

public class ButtonsScript : MonoBehaviour
{
    private bool isReset;
    private List<string> aminoAcidSequence = new List<string>();
    private List<string> seq = new List<string>();

    public string GetAminoAcidSequence()
    {
        string sequence = "";

        if (isReset)
        {
            sequence = " ";
            return sequence;
        }

        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] draggableBoxes = gameController.GetAllBoxes();
        //TODO: sort array costume
        for (int i = 0; i < draggableBoxes.Length; i++)
        {
            DraggableBox box = draggableBoxes[i];
            if (box.GetIsPlaced())
            {
                seq.Add(box.aminoAcidChar.ToString());
                seq.Add(box.transform.position.x.ToString());
                sequence += "[" + box.aminoAcidChar.ToString() + ", " + box.transform.position.x + "], ";
            }
            aminoAcidSequence.Add(seq.ToString());
        }

        return sequence;
    }

    public void ResetAminoAcids()
    {
        //TODO: all boxes return to start pos
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] draggableBoxes = gameController.GetAllBoxes();

        for (int i = 0; i < draggableBoxes.Length; i++)
        {
            DraggableBox box = draggableBoxes[i];
            if (box.GetIsPlaced())
            {
                box.ReturnToStartPos();
            }
        }
        isReset = true;
    }

    public void OnGetAminoAcidsClick()
    {
        string seq = GetAminoAcidSequence();
        print("Amino Acid Sequence: " + seq);

        print("=============================");

        foreach (var item in aminoAcidSequence)
        {
            print(item.ToString());
        }
        //print("aminoAcidSequence[0]: " + aminoAcidSequence[0]);

    }

    public void OnResetClick()
    {
        ResetAminoAcids();
        print("reset btn clicked");
    }
}
