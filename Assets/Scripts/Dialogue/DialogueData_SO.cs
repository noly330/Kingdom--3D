using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "DialogueData", menuName = "Dialogue/DialogueData_SO")]
public class DialogueData_SO : ScriptableObject
{
    public List<DialoguePiece> dialoguePieces = new List<DialoguePiece>();
}

[System.Serializable]
public class DialoguePiece
{
    public string ID;
    public Sprite image;
    [TextArea(3, 10)]
    public string text;
    public bool hasOptions;
    public List<DialogueOption> options = new List<DialogueOption>();
}

public class DialogueOption
{
    public string text;
    public string targetID;

    //TODO：任务
    public bool takeQuest;
}
