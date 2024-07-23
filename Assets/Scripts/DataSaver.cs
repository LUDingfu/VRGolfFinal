using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
// using FireSharp;
// using FireSharp.EventStreaming;
// using FireSharp.Interfaces;
// using FireSharp.Response;

[Serializable]
public class dataToSave
{
    public string userName;
    public int totalCoins;
    public int crrLevel;
    public int highScore;
}
public class DataSaver : MonoBehaviour
{
    public dataToSave dts;
    public string userId;
    // private DatabaseReference dbRef;
    [SerializeField] private Renderer ballRenderer;
    [SerializeField] private GolfBallController golfBallController;

    // private IFirebaseClient client;
    
    private void Awake()
    {
        // Connection conn = new Connection();
        // client = conn.client;
        // StartCoroutine(RunDatabaseUpdateChecker());
        // dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        // dbRef.Child("CurrentUser").ValueChanged += OnCurrentUserValueChanged;
    }
    
    private IEnumerator RunDatabaseUpdateChecker()
    {
        // Run the DatabaseUpdateChecker continuously in the background
        while (true)
        {
            yield return new WaitUntil(() => DatabaseUpdateChecker().IsCompleted);
            // Optionally, add a delay to prevent it from running too frequently
            yield return new WaitForSeconds(1f); // Adjust the delay as needed
        }
    }

    private async Task DatabaseUpdateChecker()
    {
        // EventStreamResponse response = await client.OnAsync("CurrentUser", added: (s, args, context) =>
        //     {
        //         Debug.Log(args.Data);
        //     },
        //     (s, args, context) =>
        //     {
        //         Debug.Log(args.Data);
        //         OnCurrentUserValueChanged(s, args);
        //     },
        //     (s, args, context) =>
        //     {
        //         Debug.Log(args);
        //     }
        // );
    }

    // private void OnCurrentUserValueChanged(object sender, ValueChangedEventArgs e)
    // {
    //     // userId = e.Snapshot.Value.ToString();
    //     // userId = e.Data
    //     ballRenderer.material.color = OnColorChanged(); 
    // }
    private Color OnColorChanged()
    {
        switch (userId)
        {
            case "user002":
                return Color.red;
            case "user001":
                return Color.blue;
            default:
                return Color.white;
        }
    }
    
    public void SaveDataFn()
    {
        string json = JsonUtility.ToJson(dts);
        // dbRef.Child("users").Child(userId).Child("gameValues").Child("crrLevel").SetRawJsonValueAsync(json);
    }

    public void LoadDataFn()
    {
        // StartCoroutine(LoadDataEnum());
        
    }
    

    // IEnumerator LoadDataEnum()
    // {
    //     // var severData = dbRef.Child("users").Child(userId).GetValueAsync();
    //     // yield return new WaitUntil(predicate: (() => severData.IsCompleted));
    //     
    //     print("process is completed");
    //
    //     // DataSnapshot snapshot = severData.Result;
    //     string jsonData = snapshot.GetRawJsonValue();
    //
    //     if (jsonData != null)
    //     {
    //         print("server data found!");
    //
    //         dts = JsonUtility.FromJson<dataToSave>(jsonData);
    //     }else
    //     {
    //         print("no data found");
    //     }
    // }
}
