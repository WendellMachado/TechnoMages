using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon;

public class TCP_MessagingSystem : PunBehaviour
{
    [SerializeField]
    InputField messageBox;

    Queue<string> messages;

    const int messageCount = 6;

    private void Start()
    {
        messages = new Queue<string>(messageCount);
    }

    public void AddMessage(string message)
    {
        this.gameObject.GetComponent<PhotonView>().RPC("AddMessage_RPC", PhotonTargets.All, message);
    }

    [PunRPC]
    void AddMessage_RPC(string message)
    {
        messages.Enqueue(message);
        if (messages.Count > messageCount) { messages.Dequeue(); }

        messageBox.text = string.Empty;
        foreach (string m in messages)
        {
            messageBox.text += m + "\n";
        }
    }
}
