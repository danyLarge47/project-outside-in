using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "Game Content Database", menuName = "Dany Custom/Game Content Database")]

public class VN_ContentDownloader : ScriptableObject
{
    
    // https://docs.google.com/spreadsheets/d/1aYsFUwuutv91vJtMCteGCGjwz6x-JtfFIApBpEYkjfM/edit?gid=1326626947#gid=1326626947
    
    
      const string databaseSheetsId = "1aYsFUwuutv91vJtMCteGCGjwz6x-JtfFIApBpEYkjfM";
    const string guid_contents = "1326626947";

    [FoldoutGroup("Raw Data"), SerializeField, TableList]
    private List<ContentRow> raw_contents = new();

    [SerializeField] private Dictionary<string, ContentNode> content_database = new();
    public IEnumerable<ContentNode> Nodes => content_database.Values;

    public ContentNode GetContentData(string contentId)
    {
        if (content_database.ContainsKey(contentId)) return content_database[contentId];
        return null;
    }

#if UNITY_EDITOR

    public async Task DownloadContentData()
    {
        Debug.Log($"------------ Download Main Story ------------");
        var tsvData = await DL_JSON.DownloadSheetsTsv(databaseSheetsId, guid_contents);
        if (tsvData == null)
        {
            Debug.LogWarning($"Null TSV");
            return;
        }

        string jsonData = DL_JSON.ConvertTsvToJson<ContentRow>(tsvData);
        raw_contents = DL_JSON.ArrayFromJsonNoWrapper<ContentRow>(jsonData).ToList();
        raw_contents = raw_contents.Where(item => !string.IsNullOrEmpty(item.Row_Type)).ToList();
        PopulateContentDatabase(raw_contents);
        EditorUtility.SetDirty(this);
    }
    
    void PopulateContentDatabase(List<ContentRow> data)
    {
        ContentNode tempNode = null;
        for (int i = 0; i < data.Count; i++)
        {
            if (string.IsNullOrEmpty(data[i].Row_Type)) continue;
            if (data[i].Row_Type.Equals("NodeStart"))
            {
                var newNode = new ContentNode();
                newNode.Id = data[i].Row_Id;
                if (content_database.ContainsKey(newNode.Id))
                {
                    Debug.LogWarning($"Content {newNode.Id} already exist");
                    continue;
                }

                content_database.Add(newNode.Id, newNode);
                tempNode = newNode;
            }

            if (tempNode != null) tempNode.contentRows.Add(data[i].GetDuplicate());
        }
    }

    [Button(ButtonSizes.Large)]
    public void ResetAll()
    {
        raw_contents.Clear();
        content_database.Clear();
    }

    [Button(ButtonSizes.Large)]
    public async void UpdateAll()
    {
        try
        {
            await DownloadContentData();
        }
        catch (System.Exception error)
        {
            Debug.LogError("Sheet import rejected; previous database kept. " + error.Message);
        }

        EditorApplication.Beep();
    }

#endif
}
