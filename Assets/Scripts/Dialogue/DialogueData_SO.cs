using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/DialogueData_SO")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
    public Dictionary<string, DialoguePiece> dialoguePiecesDictionary = new Dictionary<string, DialoguePiece>();
#if UNITY_EDITOR

    private void OnValidate()
    {
        dialoguePiecesDictionary.Clear();
        for (int i = 0; i < dialoguePieces.Count; i++)
        {
            dialoguePiecesDictionary.Add(dialoguePieces[i].ID, dialoguePieces[i]);
        }
    }

#endif
}


[System.Serializable]
public class DialoguePiece
{
    public string ID;
    public Sprite image;
    [TextArea(3, 10)]
    public string text;
    public string targetID;
    public bool hasOptions;
    public List<DialogueOption> options = new List<DialogueOption>();
}

[System.Serializable]

public class DialogueOption
{
    public string text;
    public string targetID;

    //TODO：任务
    public bool takeQuest;
}
