using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance { get; private set; }

    // 存档根路径
    private string SavePath => Path.Combine(Application.persistentDataPath, "Saves");
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // 初始化存档目录
            if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
        }
        Debug.Log($"存档目录：{SavePath}");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Save(InventoryManager.instance.inventoryData, "InventoryData");
            Save(InventoryManager.instance.equipmentData, "EquipmentData");
            Debug.Log("保存成功");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Load(InventoryManager.instance.inventoryData, "InventoryData");
            Load(InventoryManager.instance.equipmentData, "EquipmentData");
            Debug.Log("加载成功");
        }
    }

    // 核心：文件存储替代PlayerPrefs，保留原有方法签名
    public void Save(Object data, string key)
    {
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
            Debug.Log($"创建存档目录：{SavePath}");
        }
        string json = JsonUtility.ToJson(data,true);
        string filePath = Path.Combine(SavePath, $"{key}.json");
        File.WriteAllText(filePath, json, System.Text.Encoding.UTF8);


    }

    public void Load(Object data, string key)
    {

        string filePath = Path.Combine(SavePath, $"{key}.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
            JsonUtility.FromJsonOverwrite(json, data);
        }

    }
}
