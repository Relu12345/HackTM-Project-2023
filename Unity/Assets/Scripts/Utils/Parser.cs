using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Parser : MonoBehaviour
{
    public static Question parseQuestion(string aiResponse)
    {
        string replacedStr = aiResponse.Replace('#', '\"');

        var jsonD = JObject.Parse(replacedStr);

        var question = (string)jsonD["question"];
        var answers = (JObject)jsonD["answers"];

        var answer1 = (string)answers["1"];
        var answer2 = (string)answers["2"];
        var answer3 = (string)answers["3"];
        var answer4 = (string)answers["4"];

        var correct = (int)jsonD["correct"];

        string[] answersList = new string[4]{
            answer1, answer2, answer3, answer4
        };

        return new Question(question, answersList, correct);
    }

    public class Question{
        public string question{get; }
        public string[] answers{get; }
        public int correctAnswer{get;}

        public Question(string _q, string[] _a, int _ca){
            question = _q;
            answers = _a;
            correctAnswer = _ca;
        }
    }
}
