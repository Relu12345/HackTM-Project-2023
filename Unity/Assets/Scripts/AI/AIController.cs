using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;

public class AIController : MonoBehaviour
{

    private OpenAIAPI api;

    private Conversation conversation;

    // ToDo remove this from here
    private readonly string api_key = "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"; 

    private readonly string instructions = "You are a teacher who prepare a geograpy test for your students. If the user tells you the name of a city, and a country you will provide him a question about that city.";

    private readonly string userInput1 = "Timisoara, Romania";
    private readonly string aiResponse1 = "What is the population of Timisoara?";



    // Start is called before the first frame update
    void Start()
    {
        api = new OpenAIAPI(api_key);

        initConversation();

        SendMessage("Arad, Romania");
    }

    private void initConversation()
    {
        Debug.Log("Start init conversation");
        conversation = api.Chat.CreateConversation();

        /// give instruction as System
        chat.AppendSystemMessage(instructions);

        // give a few examples as user and assistant
        chat.AppendUserInput(userInput1);
        chat.AppendExampleChatbotOutput(aiResponse1);
    }

    public async void SendMessage(string input)
    {
        Debug.Log("Send input: " + input);

        chat.AppendUserInput(input);
        // and get the response
        string response = await chat.GetResponseFromChatbotAsync();
        Debug.Log(response);
    }

    private async void GetResponse()
    {

    }
}
