using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionBox : MonoBehaviour
{

    public Text description;

    public Text minimizedText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitDescription(string name, string type, string val)
    {
        string descStr = "";
        string minimizedStr = "";

        descStr = string.Format("Selected "+ type + "-Parameter: \n " + name +"\nCurrent Value: \n "+  val);
        minimizedStr = string.Format(name + ": " + val);
        description.text = descStr;
        minimizedText.text = minimizedStr;
    }

    public void InitDescription(string name, string type, string val, string enumChoices)
    {
        string descStr = "";
        string minimizedStr = "";

        descStr = string.Format("Selected " + type + "-Parameter: \n " + name + "\nEnum values: \n " + "[" + enumChoices + "]" + "\nCurrent Value: \n " + val);
        minimizedStr = string.Format(name + ": " + val);
        description.text = descStr;
        minimizedText.text = minimizedStr;
    }

}
