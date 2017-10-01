using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class history_script : MonoBehaviour {
    public Sprite[] imgs;
    public GameObject imgui;
    public Text t;
    private int i = 1;
    
    public void next()
    {
        if (i == imgs.Length - 1)
        {
            t.text = "Play";
            imgui.GetComponent<Image>().sprite = imgs[i];
            ++i;
        } else if (i >= imgs.Length)
        {
            SceneManager.LoadScene("vrai_map");
        } else
        {
            imgui.GetComponent<Image>().sprite = imgs[i];
            ++i;
        }
    }
}
