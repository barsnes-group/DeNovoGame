using UnityEngine;
using TMPro;

public class AminoAcidSequence : MonoBehaviour
{

    public string sequence;

    /* private void SetText(string text)
    {
        GetComponent<TextMeshPro>().text = text.ToString();
    } */

    public string GetAminoAcidSequence()
    {
        GameController gameController = GameObject.Find("GameController").GetComponent<GameController>();
        DraggableBox[] draggableBoxes = gameController.GetAllBoxes();

        for (int i = 0; i < draggableBoxes.Length; i++)
        {
            DraggableBox box = draggableBoxes[i];
            if (box.GetIsPlaced())
            {
                sequence += box.aminoAcidChar.ToString();
            }
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
    }

    public void OnGetAminoAcidsClick()
    {
        string seq = GetAminoAcidSequence();
        print("Amino Acid Sequence: " + seq);
    }

    public void OnResetClick()
    {
        ResetAminoAcids();
        print("reset btn clicked");
    }
}
