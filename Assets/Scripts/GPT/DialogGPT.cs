using OpenAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace magister
{
    public class DialogGPT : MonoBehaviour
    {
        [SerializeField] string initializeConversation;
        [SerializeField] TMP_InputField inputField;
        [SerializeField] Button sendButton;
        [SerializeField] TMP_Text received;
        
        private OpenAIApi openai = new OpenAIApi();
        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt;
        private Personality dialogTarget;

        private void Awake()
        {
            Button btn = sendButton.GetComponent<Button>();
            btn.onClick.AddListener(SendReply);
            transform.GetComponent<DialogWindow>().OnDialogWindowShown.AddListener(InitializeDialog);
        }

        private void Start()
        {

        }


        private async void InitializeDialog()
        {
            Debug.Log("Dialog Initialized");
            dialogTarget = transform.GetComponent<DialogWindow>().GetPersonality();

            received.text = "";
            inputField.text = "";
            messages.Clear();

            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = dialogTarget.GetInitialPrompt() + "\n" + initializeConversation
            };

            messages.Add(newMessage);

            sendButton.enabled = false;
            inputField.enabled = false;

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0125",
                Messages = messages
            });

            //OnReplayRecived.Invoke();

            Debug.LogWarning(completionResponse.Choices);

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);

                received.text = message.Content;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            sendButton.enabled = true;
            inputField.enabled = true;

        }


        private async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };

            if (messages.Count == 0) newMessage.Content = prompt + "\n" + inputField.text;

            messages.Add(newMessage);

            sendButton.enabled = false;
            inputField.text = "";
            inputField.enabled = false;

            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0125",
                Messages = messages
            });

            //OnReplayRecived.Invoke();

            Debug.LogWarning(completionResponse.Choices);

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);

                received.text = message.Content;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            sendButton.enabled = true;
            inputField.enabled = true;
        }

    }
}
