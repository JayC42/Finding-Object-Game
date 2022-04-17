using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TMP_Text ObjectsToFind;

    //[SerializeField] private TMP_Text ObjectsToFind;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void SetText(string text)
    {
         ObjectsToFind.text = text;
    }
}
