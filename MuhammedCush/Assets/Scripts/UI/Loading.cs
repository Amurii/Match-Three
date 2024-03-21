using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Slider loadingSlider;
    [SerializeField] TextMeshProUGUI loadingTxt;
    public static Loading instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(instance);
      //  StartCoroutine(AsynchronousLoad());
    }
    private void OnEnable()
    {
        OnSliderChange(0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnSliderChange(int present)
    {
        loadingSlider.value = present;
        loadingTxt.text = "LOADING : " + loadingSlider.value + "%";
    } 
}
