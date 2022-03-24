using UnityEngine;
using TMPro;

public class ButtonsScript : MonoBehaviour
{
    private bool isReset;

    public string GetAminoAcidSequence()
    {
        string sequence = "";

        if (isReset) {
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
                sequence += box.aminoAcidChar.ToString() + " , " + box.transform.position.x;
                
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
        isReset = true;
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
