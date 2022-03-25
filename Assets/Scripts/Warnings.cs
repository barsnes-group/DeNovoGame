using UnityEngine;
using TMPro;

public class Warnings : MonoBehaviour
{

    public void SetText(string text)
    {
            GetComponent<TextMeshPro>().text = text.ToString();
    }
}
