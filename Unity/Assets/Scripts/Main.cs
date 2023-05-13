using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour
{

    [SerializeField]
    private AIController aIController;

    [SerializeField]
    private GPS gps;

    [SerializeField]
    private UILogic uILogic;

    private Parser.Question currentQuestion;

    // Start is called before the first frame update
    void Start()
    {
        aIController = GetComponent<AIController>();
        gps = GetComponent<GPS>();
        uILogic = GetComponent<UILogic>();

        gps.Init();
        aIController.Init();
        uILogic.Init(StartButton, NextQuestion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    async void StartButton()
    {
        // currentQuestion = Parser.parseQuestion("{#question#:#Used in ancient times by the poet Tibullus, The Eternal City is a nickname given to what European capital?#, #answers#:{#1#:#Venice#,#2#:#Tivoli#,#3#:#Rome#,#4#:#Siena#},#correct#:3}");

        var r = await aIController.GetResponse("Timisoara, Romania");

        currentQuestion = Parser.parseQuestion(r);
        uILogic.DisplayQuestion(currentQuestion, buttonPressed);
        // Debug.Log(r);
       
    }

    async void NextQuestion()
    {
        var r = await aIController.GetResponse("Baia Mare, Romania");

        currentQuestion = Parser.parseQuestion(r);
        uILogic.DisplayQuestion(currentQuestion, buttonPressed);
    }

    void buttonPressed(int userAnswer)
    {
        if(userAnswer == currentQuestion.correctAnswer){
            uILogic.ChangeScreen(uILogic.corect);
        }
        else{
            uILogic.ChangeScreen(uILogic.gresit);
        }
    }

}
