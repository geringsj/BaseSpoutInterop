using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HTC.UnityPlugin.Vive;
using UnityEngine.UI;
using interop;

/**
 * this Script manages the displayed values on the OctagonEnum Prefab. The displayable values are stored in values. 
 * Its necessary that values has at least as many entries as total sides on the Octagonenum. 
 */ 
public class HexagonEnum : UnityEnumInteraction, IPointerClickHandler
{
    //specific variables depending on the geometric form (octagon)
    public Color SelectedValueColor = Color.red;

    private int upSides = 5;
    private int downsides = 3;
    private int totalsides = 8;

    private int rotation;
    private List<string> values = new List<string>(new string[]{"0","1","2","3","4","5","6","7","8","9"});
    private int backElementIndx;
    private int frontElementIndx;
    private int backValueIndx;
    private Text[] texts; 
    private bool isRotating = false;
    private int i;

    override public void StartInteraction(Parameter<List<string>> initValue, VisParamSender<List<string>> sender)
    {
        //if (initValue.name.Equals("ao::shadeMode"))
        //{
        //    initValue.param.RemoveAt(0);
        //    initValue.param.RemoveAt(0);
        //    initValue.param.RemoveAt(0);
        //    initValue.param.RemoveAt(0);

        //    Debug.Log("[smoothrotation] deleted first elements " + string.Join(", ", initValue.param.ToArray()));
        //}
        transform.Rotate(new Vector3(0, 0, 0));
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

        //reset();
        Debug.Log("[VisInteraction]: start interaction for " + initValue.modulFullName + ", " + initValue.name);
        selectedValue = initValue;
        Debug.Log("[smoothrotation]: received Values = " + string.Join(", ", selectedValue.param.ToArray()));
        this.senderManager = sender;
        this.value = null;
        this.value = initValue.param;
        setUpValuesList();
        values = new List<string>(initValue.param.ToArray());
        values.RemoveAt(0);
        gameObject.SetActive(true);

        // set the text color to black
        foreach (Text t in texts)
        {
            t.color = Color.black;
        }

        Debug.Log("[smoothrotation]: received Values = " + string.Join(", ", values.ToArray()));

        Debug.Log("[Integerinteraction]: Init attributes: param values = " + string.Join(", ", selectedValue.param.ToArray()) + " backindex = " + backElementIndx + " frontElementindex " + frontElementIndx + " backvalueindex " + backValueIndx);

        Debug.Log("[Integerinteraction]: Values after setup = " + string.Join(", ", selectedValue.param.ToArray()));

        //int startidx = 0;
        //for (int i = 0; i < values.Count; i++)
        //{
        //    string str = values[0];

        //    if (str.Equals(selectedValue.param[0]))
        //    {
        //        startidx = i;
        //        break;
        //    }
        //}

        // rotate (without animation) until the front element has the same string as the selected value
        while (!texts[frontElementIndx].text.Equals(selectedValue.param[0]))
        {

            if (!isRotating)
            {
                //Debug.Log("[Integerinteraction]: Values before rotate = " + string.Join(", ", selectedValue.param.ToArray()));
                
                rotateNoAnimation();
                //Debug.Log("[Integerinteraction]: Values after rotate = " + string.Join(", ", selectedValue.param.ToArray()));
            }
            Debug.Log("[Integerinteraction]: selectedValue = " + selectedValue.param[0] + ", frontElement = " + texts[frontElementIndx].text + ", " + !texts[frontElementIndx].text.Equals(selectedValue.param[0]));
        }
        texts[frontElementIndx].color = SelectedValueColor;
        i = -50;

        Debug.Log("[VisEnumInteraction]: Enum param successful started");
        Debug.Log("[Integerinteraction]: finished setUp, displayed Value = " + texts[frontElementIndx].text);

        if (values.Count < totalsides)
        {
            rotateNoAnimation();
            rotateNoAnimation();
            rotateNoAnimation();
            rotateNoAnimation();
            rotateNoAnimation();
            rotateNoAnimation();
            rotateNoAnimation();
            rotateNoAnimation();
        }

    }

    private void reset()
    {


      rotation = -50;
     List<string> values = new List<string>();
      backElementIndx = 4;
      frontElementIndx = 0;
      backValueIndx = 4;
      isRotating = false;
        i = -50; ;
}


    // Start is called before the first frame update
    new void Start()
    {


        //Parameter<List<string>> param = new Parameter<List<string>>();
        //param.name = "test";
        //param.modulFullName = "modulNameTest";
        //param.param = new List<string>(new string[] { "5", "1", "2", "3", "4", "5"});//new string[] { "erstesEl", "zweitesEl", "drittesEl", "viertesElement" });

        //StartInteraction(param, null);
        //StopInteraction();
        //StartInteraction(param, null);

        gameObject.SetActive(false);
        setUpValuesList();
    }

    private void setUpValuesList()
    {
        //if (totalsides > values.Count)
        //{
        //    int diff = totalsides - values.Count;

        //    for (int i = 0; i < diff; i++ )
        //    {
        //        if (i >= values.Count)
        //        {
        //            i = 0;
        //        }
        //        values.Add(values[i]);
        //    }
        //}

        // get the text fields in rotation direction
        texts = GetComponentsInChildren<Text>();

        string[] arrayText = new string[texts.Length];
        int i = 0;
        foreach (Text t in texts)
        {
            arrayText[i] = t.name;
            i++;
        }

        Debug.Log("[Integerinteraction]: found texts = " + string.Join(", ", arrayText));


        backElementIndx = upSides - 1;
        backValueIndx = (upSides - 1) % values.Count;
        frontElementIndx = 0;
        Debug.Log("[HexagonEnumScript]: frontElementIndx = " + frontElementIndx + ", texts: " + texts.Length);

        // set the texts of the sides which are up
        int valuesIdx = 0;

        for (int index = 0; index < upSides; index++)
        {
            texts[index].text = values[valuesIdx];

            valuesIdx++;
            if (valuesIdx >= values.Count)
            {
                valuesIdx = 0;
            }
        }

        // set the texts of the sides which are down
        valuesIdx = 1;
        for (int index = 1; index <= downsides; index++)
        {
            texts[texts.Length - index].text = values[values.Count - valuesIdx];

            valuesIdx++;
            if (valuesIdx >= values.Count)
            {
                valuesIdx = 0;
            }
        }
        isRotating = false;
        rotation = 0;
        i = -1;
    }

    

    private void rotate()
    {
        texts[frontElementIndx].color = Color.black;
        /*
         *change text of the back element too increase the number of displayable values to infinity
         */
        backElementIndx++;
        if (backElementIndx >= totalsides) // prevent index out of bounds
        {
            backElementIndx = 0;
        }

        frontElementIndx++;
        if (frontElementIndx >= totalsides) // prevent index out of bounds
        {
            frontElementIndx = 0;
        }

        backValueIndx = (backValueIndx + 1) % values.Count;

        texts[backElementIndx].text = values[backValueIndx];

        
        Debug.Log("[hexagonEnum]: front Text: " + selectedValue.param[0] + ", " + selectedValue.param.Count);
        Debug.Log("[hexagonEnum]: displayed Value: " + string.Join(", ", GetSelectedValue().param.ToArray()));
        Debug.Log("[hexagonEnum]: Values: " + string.Join(", ", values.ToArray()));
        texts[frontElementIndx].color = SelectedValueColor;
        if (senderManager != null)
        {
            
            selectedValue.param[0] = texts[frontElementIndx].text;
            senderManager.Send(base.selectedValue);
        }

        rotation = -45;
        i = -1;
    }

    private void rotateNoAnimation()
    {
        rotate();
        i = -50;
        this.transform.Rotate(new Vector3(-45, 0, 0));
    }

    public Text getFrontText()
    {
        return texts[frontElementIndx];
    }

    public bool SetValue(string value)
    {
        if (values.Contains(value))
        {
            while (!getFrontText().text.Equals(value))
            {
                rotate();
            }
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("[Integerinteraction] is rotating = " + isRotating);
        if (i > rotation)
        {
            isRotating = true;
            this.transform.Rotate(new Vector3(i * 0.045f, 0, 0));
            //this.transform.rotation = Quaternion.Euler(0, i * 0.045f, 0);
            //this.transform.eulerAngles = new Vector3(i * 0.045f, 0, 0);
           //Debug.Log("i = " + i + ", " + rotation);
           i--;
         
        }
        //else if (i == rotation)
        //{
        //        this.transform.Rotate(new Vector3(frontElementIndx*-45, 0, 0));
        //    Debug.Log("[smoothrotation] currrent rotation " + frontElementIndx * -45 + " front index " + frontElementIndx);
        //        i--;
        //}
        else
        {
            isRotating = false;
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.IsViveButton(ControllerButton.Trigger))
        {

            if (!isRotating)
            {
                rotate();
            }


        }
    }
}
