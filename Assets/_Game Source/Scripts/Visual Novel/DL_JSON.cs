using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DL_JSON
{
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public static string JsonFromArray<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T> { Items = array };
        return JsonUtility.ToJson(wrapper);
    }

    public static T[] ArrayFromJsonNoWrapper<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    private class Wrapper<K, V>
    {
        public K[] Keys;
        public V[] Values;
    }

    public static string DictionaryToJson<K, V>(Dictionary<K, V> dictionary)
    {
        Wrapper<K, V> wrapper = new Wrapper<K, V>();
        wrapper.Keys = dictionary.Keys.ToArray();
        wrapper.Values = dictionary.Values.ToArray();
        return JsonUtility.ToJson(wrapper);
    }

    public static Dictionary<K, V> DictionaryFromJson<K, V>(string json)
    {
        Wrapper<K, V> wrapper = JsonUtility.FromJson<Wrapper<K, V>>(json);
        Dictionary<K, V> res = new Dictionary<K, V>();
        for (int i = 0; i < wrapper.Keys.Length; i++)
        {
            res.Add(wrapper.Keys[i], wrapper.Values[i]);
        }

        return res;
    }

    public static async Task<string> DownloadSheetsTsv(string sheetId, string gid)
    {
        Debug.Log($"===> DownloadDataContent {gid} ");
        string targetUrl = $"https://docs.google.com/spreadsheets/d/{sheetId}/export?format=tsv&gid={gid}";
        using var request = UnityWebRequest.Get(targetUrl);
        var op = request.SendWebRequest();
        while (!op.isDone) await Task.Yield();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"===> {gid} <color=green>[{request.result}]</color>");
            return request.downloadHandler.text;
        }

        Debug.LogWarning($"===> targetUrl {targetUrl} <color=red>[{request.result}]</color>");
        return null;
    }

    public static string ConvertTsvToJson<T>(string tsv) where T : new()
    {
        var lines = tsv.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length < 2) return "[]";
        var headers = lines[0].Split('\t');
        var resultList = new List<T>();
        for (int i = 1; i < lines.Length; i++)
        {
            var values = lines[i].Split('\t');
            var obj = new T();
            var type = typeof(T);
            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                var field = type.GetField(headers[j]);
                if (field != null)
                {
                    field.SetValue(obj, values[j]);
                }
                else
                {
                    var prop = type.GetProperty(headers[j]);
                    if (prop != null && prop.CanWrite)
                    {
                        prop.SetValue(obj, values[j]);
                    }
                }
            }

            resultList.Add(obj);
        }

        return JsonUtility.ToJson(new Wrapper<T> { Items = resultList.ToArray() });
    }
}