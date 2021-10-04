using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Mek.Localization.Editor
{
    public class LocalizationLoader : MonoBehaviour
    {
        private const string _googleSheetDocID = "1ZVunhEpGg5IM3aV4vZNk5vBad4DkhVSSy_vEGH5V2uk";
        private const string url = "https://docs.google.com/spreadsheets/d/" + _googleSheetDocID + "/export?format=csv";

        [MenuItem("Mek/FetchLocalizationData")]
        public static async void FetchLocalizationData()
        {
            await DownloadAsync(data =>
            {
                Localization localizationAsset = Resources.Load<Localization>($"Localization");

                if (localizationAsset != null)
                {
                    EditorUtility.SetDirty(localizationAsset);
                    localizationAsset.ReadCSV(data);
                }
                else
                {
                    localizationAsset = ScriptableObject.CreateInstance<Localization>();
                    AssetDatabase.CreateAsset(localizationAsset, "Assets/Resources/Localization.asset");
                    EditorUtility.SetDirty(localizationAsset);
                    localizationAsset.ReadCSV(data);
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.ClearDirty(localizationAsset);
            });
        }

        static readonly HttpClient client = new HttpClient();
        static async Task DownloadAsync(Action<string> onCompleted)
        {
            
            string downloadData = null;
            
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try	
            {
                Debug.Log("Starting Download...");
                
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                Stream receiveStream = await response.Content.ReadAsStreamAsync();
                StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);

                var content = await readStream.ReadToEndAsync();
                
                int equalsIndex = ExtractEqualsIndex(content);
                if (!response.IsSuccessStatusCode || (-1 == equalsIndex))
                {
                    Debug.LogError("Download Error: " + response.StatusCode);
                }
                else
                {
                    string versionText = content.Substring(0, equalsIndex);
                    downloadData = content.Substring(equalsIndex + 1);

                    Debug.Log($"Download succeed! Version: {versionText}");
                }

                onCompleted(downloadData);
            }
            catch(HttpRequestException e)
            {
                Debug.Log("\nException Caught!");	
                Debug.LogFormat("Message :{0} ",e.Message);
            }
        }
        
        private static int ExtractEqualsIndex(string text)
        {
            if (text == null || text.Length < 10)
            {
                return -1;
            }
            // First term will be preceeded by version number, e.g. "100=English"
            string versionSection = text.Substring(0, 5);
            int equalsIndex = versionSection.IndexOf('=');
            if (equalsIndex == -1)
                Debug.Log("Could not find a '=' at the start of the CSV");
            return equalsIndex;
        }
    }
}