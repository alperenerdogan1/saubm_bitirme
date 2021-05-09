using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    List<SavedObjectData> savedObjectDatas = new List<SavedObjectData>();
    [SerializeField] BuildManager BuildManager;
    private void Start()
    {
        if (GameManager.Instance.loadedGame)
        {
            LoadGame();
        }
    }
    public void SaveGame()
    {
        foreach (Transform child in transform)
        {
            SavedObjectData data = new SavedObjectData();
            data.name = child.name;
            data.position = new float[] { child.position.x, child.position.y, child.position.z };
            data.rotation = new float[] { child.rotation.x, child.rotation.y, child.rotation.z, child.rotation.w };
            data.scale = new float[] { child.localScale.x, child.localScale.y, child.localScale.z };
            savedObjectDatas.Add(data);
        }
        FileStream fileStream = new FileStream("save.dat", FileMode.Create);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        binaryFormatter.Serialize(fileStream, savedObjectDatas);
        fileStream.Close();
    }

    public void LoadGame()
    {
        List<SavedObjectData> items;
        using (Stream stream = File.Open("save.dat", FileMode.Open))
        {
            var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            items = (List<SavedObjectData>)bformatter.Deserialize(stream);
        }
        foreach (var item in items)
        {
            GameObject go;
            go = GameManager.Instance.GetItemGameObjectFromResources(item.name);
            go.transform.position = new Vector3(item.position[0], item.position[1], item.position[2]);
            go.transform.rotation = new Quaternion(item.rotation[0], item.rotation[1], item.rotation[2], item.rotation[3]);
            go.transform.localScale = new Vector3(item.scale[0], item.scale[1], item.scale[2]);
            BuildManager.BuildObject(go, go.transform.position, item.name);
        }
    }
    public void Quit()
    {
        Application.Quit();
    }

}
