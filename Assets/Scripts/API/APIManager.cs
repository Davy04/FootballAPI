using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using CSharpAPI.Models;


public class APIManager : MonoBehaviour
{
    public Root Root;
    
    private const string apiUrl = "https://raw.githubusercontent.com/openfootball/football.json/master/2020-21/en.1.json";
    
    public IEnumerator LoadMatchData()
    {
        using UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Error downloading data: {request.error}");
            yield break;
        }

        try
        {
            string json = request.downloadHandler.text;
            
            Root = JsonConvert.DeserializeObject<Root>(json);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error processing data:{request.error} {ex.Message} ");
        }
    }
}