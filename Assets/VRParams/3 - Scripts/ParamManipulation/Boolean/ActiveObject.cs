﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using HTC.UnityPlugin.Vive;
using interop;

public class ActiveObject : UnityBoolInteraction, IPointerClickHandler
{
    private bool isActive;

    public override void StartInteraction(Parameter<bool> initValue, VisParamSender<bool> sender)
    {
        base.StartInteraction(initValue, sender);

        isActive = selectedValue.param;

        if (isActive)
        {
            transform.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

        }
        else
        {
            transform.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.IsViveButton(ControllerButton.Trigger))
        {
            if (isActive)
            {
                transform.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

            } else
            {
                transform.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }

            isActive = !isActive;
            selectedValue.param = isActive;
            senderManager.Send(selectedValue);
        }
    }
}