using System;
using Analytics.DTO;
using Models;
using Proyecto26;
using UnityEngine;
using Object = System.Object;

namespace Analytics
{
    public class HttpSender
    {
        private const string DataBaseUrl = "https://mazeshift-marauders-67598-default-rtdb.firebaseio.com/";

        public static void RecordData(string fileName, Object content)
        {
            string url = DataBaseUrl + fileName + ".json";
            Debug.Log(url);
            Send(url, content);
        }
        private static void Send(string url, Object content)
        {
            
            RestClient.Post(url, UnityEngine.JsonUtility.ToJson(content, true)).Then(response =>
            {
                Debug.Log("Response: " + response.Text);
            }).Catch(error =>
            {
                Debug.LogError("Record Data failed: " + error.Message);
            });
        }
    }
}