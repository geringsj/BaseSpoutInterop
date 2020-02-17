using HTC.UnityPlugin.Vive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using interop;
using System;

public class FloatInteraction : UnityFloatInteraction, IPointerClickHandler
{

    private List<HexagonEnum> digits = new List<HexagonEnum>();
    private GameObject comma;
    private bool finishedInit = false;

    // Start is called before the first frame update
    new void Start()
    {
        //digits = GetComponentsInChildren<GameObject<HexagonEnum>>();
        foreach (Transform child in this.transform)
        {
            if (child.GetComponent<HexagonEnum>())
            {
                digits.Add(child.GetComponent<HexagonEnum>());
            }
        }

        //init and disable the comma
       comma = transform.GetChild(4).gameObject;
        comma.SetActive(false);


        

        //for testing purposes only

       //Parameter<float> param = new Parameter<float>();
       // param.name = "zeszInt";
       // param.modulFullName = "tesmodulname";
       // param.param = 90123.261f;

       // selectedValue = param;
       // StartInteraction(param, null);
       // StopInteraction();
       // param.param = 860.097f;
       // StartInteraction(param, null);
        base.Start();


    }

    public override void StopInteraction()
    {
        finishedInit = false;
        base.StopInteraction();
    }

    public override void StartInteraction(Parameter<float> initValue, VisParamSender<float> sender)
    {
        finishedInit = false;
    Debug.Log("[FloatInteraction]: startInteraction ");
        base.StartInteraction(initValue, sender);

        // activate the comma
        comma.SetActive(true);

        // Set the front Text of each digit to get the number stored in initValue
        float value = selectedValue.param;
        //float tensBasis = 10000f;
        float tmp;
        Parameter<List<string>> tmpParam = new Parameter<List<string>>();

        Debug.Log("[FloatInteraction]: digits: " + digits.Count);
        Debug.Log("[FloatInteraction]: number: " + value);
        float number = value; 
        Debug.Log("[FloatInteraction]: number: " + value);

        LinkedList<int> stack = new LinkedList<int>();

        int count = 4;
        while (count > 0)
        {
            stack.AddLast((int) (number % 10));
            number = number / 10;
            count--;

        }
        Debug.Log("[FloatInteraction]: number: " + value);

        int tensBasis = 10;
        number = value;

        LinkedList<int> stackDeci = new LinkedList<int>();

        for (int i = 0; i < 3; i++)
        {
            stackDeci.AddLast((int)((number * tensBasis) % 10));
            Debug.Log("[FloatInteraction]: digit value " + (number * tensBasis) % 10 + ", int: " + (int)((number * tensBasis) % 10));
            number *= 10;
        }

        foreach (int a in stackDeci)
        {
            Debug.Log("[FloatInteraction]: decimal stack: " + a);
        }

        foreach (HexagonEnum digit in digits)
        {
            Debug.Log("[FloatInteraction]: stck count: " + stack.Count + ", stack deci count: " + stackDeci.Count);
            if (stack.Count > 0)
            {
                tmp = stack.First.Value;
                stack.RemoveFirst();
            }
            else if (stackDeci.Count > 0)
            {
                tmp = stackDeci.First.Value;
                Debug.Log("[FloatInteraction]: tmp deci: " + tmp);
                stackDeci.RemoveFirst();
            }
            else 
            {
                tmp = 0;
            }

            Debug.Log("[FloatInteraction]: digit " + tmp + " name " + digit.name); //+ tensBasis/10000 + " with value " + tmp);
            tmpParam.param = new List<string>(new string[] { tmp + "", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });
            digit.StartInteraction(tmpParam, null);
            //tensBasis /= 10;
        }
        finishedInit = true;
    }

    private void Update()
    {
        if (digits[0].gameObject.active && finishedInit)
        {
            float number = 0;
            int tensBasis = 1000;

            // Get int digits
            for (int i = 0; i < 4; i++)
            {
                HexagonEnum digit = digits[3 - i];
                number += Convert.ToSingle(digit.getFrontText().text) * tensBasis;//Single.Parse(digit.getFrontText().text, System.Globalization.NumberStyles.Float, ) * tensBasis;
                tensBasis /= 10;
            }

            tensBasis = 10;
            // get dezimal digits
            for (int i = 4; i < 7; i++)
            {
                HexagonEnum digit = digits[i];
                number += Convert.ToSingle(digit.getFrontText().text) / tensBasis;//Single.Parse(digit.getFrontText().text, System.Globalization.NumberStyles.Float, ) * tensBasis;
                tensBasis *= 10;
            }
            selectedValue.param = number;
            Debug.Log("[FloatInteraction]: number = " + number);

            if (senderManager != null)
            {
                senderManager.Send(selectedValue);
            }
        }
    } 

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.IsViveButton(ControllerButton.Trigger))
        {

        }
    }
}
