using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class SaveManager : MonoBehaviour
{
  public static SaveManager instance { get; set; }
     public SaveData state;
    [Header("User Info")]
    [SerializeField] int Coins;
    [SerializeField] int Moves;
    [SerializeField] bool isSoundOn;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;
        Load();
        Debug.Log(Helper.Serialize<SaveData>(state));
        loadUserInfoForInspector();
    }
    void loadUserInfoForInspector()
    {
        Coins = state.Coins;
        Moves = state.Moves;
        isSoundOn = state.IsSoundOn;
       //PlayerPrefs.DeleteAll();
    }
    public void Save()
    {
       
        PlayerPrefs.SetString("save",Helper.Serialize<SaveData>(state));
      //  Debug.Log(Helper.Serialize<SaveData>(state));
    }
    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            //Already saved
            state = Helper.Deserialize<SaveData>(PlayerPrefs.GetString("save"));
        }
        else
        {
            Debug.Log("No files create new one !");
            state = new SaveData();
            Save();
        }
    }
}
