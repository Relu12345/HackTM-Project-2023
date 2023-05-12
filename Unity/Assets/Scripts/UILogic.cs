using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UILogic : MonoBehaviour
{
    private VisualElement intrebare;
    private VisualElement corect;
    private VisualElement gresit;
    private int raspuns;

    private void Start()
    {
        raspuns = 0;
        // root-ul documentului UI
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        intrebare = root.Q("IntrebareWindow");
        corect = root.Q("CorectWindow");
        gresit = root.Q("GresitWindow");

        raspuns = 3;

        intrebare.Q<Button>("Button1").clicked += () =>
        {
            intrebare.style.display = DisplayStyle.None;
            if (raspuns == 1) 
            {
                corect.style.display = DisplayStyle.Flex;
            }
            else
            {
                gresit.style.display = DisplayStyle.Flex;
            }
        };
        intrebare.Q<Button>("Button2").clicked += () =>
        {
            intrebare.style.display = DisplayStyle.None;
            if (raspuns == 2)
            {
                corect.style.display = DisplayStyle.Flex;
            }
            else
            {
                gresit.style.display = DisplayStyle.Flex;
            }
        };
        intrebare.Q<Button>("Button3").clicked += () =>
        {
            intrebare.style.display = DisplayStyle.None;
            if (raspuns == 3)
            {
                corect.style.display = DisplayStyle.Flex;
            }
            else
            {
                gresit.style.display = DisplayStyle.Flex;
            }
        };
        intrebare.Q<Button>("Button4").clicked += () =>
        {
            intrebare.style.display = DisplayStyle.None;
            if (raspuns == 4)
            {
                corect.style.display = DisplayStyle.Flex;
            }
            else
            {
                gresit.style.display = DisplayStyle.Flex;
            }
        };


        corect.Q<Button>("NextQuestion").clicked += () =>
        {
            intrebare.style.display = DisplayStyle.Flex;
            corect.style.display = DisplayStyle.None;
        };

        gresit.Q<Button>("NextQuestion").clicked += () =>
        {
            intrebare.style.display = DisplayStyle.Flex;
            gresit.style.display = DisplayStyle.None;
        };

    }
}
