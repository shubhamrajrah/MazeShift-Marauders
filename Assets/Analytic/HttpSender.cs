using System;
using Proyecto26;
using UnityEngine;
using Object = System.Object;

namespace Analytic
{
    public class HttpSender
    {

        private const string DataBaseUrl = "https://mazeshift-marauders-67598-default-rtdb.firebaseio.com/";

        public static void RecordData(string fileName, Object content)
        {
            string url = DataBaseUrl + fileName + ".json";
            Send(url, content);
        }

        public static void UpdateData(String fileName, Object content)
        {
            string url = DataBaseUrl + fileName + ".json";
            Update(url, content);
        }

        public String GetData(String fileName)
        {
            string url = DataBaseUrl + fileName + ".json";
            return Get(url);
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

        private static void Update(string url, Object content)
        {
            RestClient.Put(url, UnityEngine.JsonUtility.ToJson(content, true)).Then(response =>
            {
                Debug.Log("Response: " + response.Text);
            }).Catch(error =>
            {
                Debug.LogError("Record Data failed: " + error.Message);
            });
        }

        private static String Get(string url)
        {
            RestClient.Get(url).Then(response =>
            {
                Debug.Log("Response: " + response.Text);
                return response.Text;
            }).Catch(error =>
            {
                Debug.LogError("Record Data failed: " + error.Message);
            });
            return "";
        }
        
    }
}