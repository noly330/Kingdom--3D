using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolManager : MonoBehaviour
{
   public static ToolManager instance;

    private void Awake()
    {
        instance = this;
    }

    //TODO:以后用对象池优化
    public void PlayOneFX(GameObject FXObject,Vector3 position ,Vector3 rotation,Vector3 scale)
    {
        var obj = Instantiate(FXObject);
        obj.transform.position = position;
        obj.transform.eulerAngles = rotation;
    }
}
