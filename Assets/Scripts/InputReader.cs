using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour ,Controller.IDialogueActions
{
    public static Controller _inputActions;
    static Dialogues _dialogue;
    public void OnEnable()
    {
        _dialogue = FindObjectOfType<Dialogues>();
        if (_inputActions!=null)
        {
            return;
        }
        _inputActions = new Controller();
        _inputActions.Dialogue.SetCallbacks(this);
        _inputActions.Dialogue.Enable();
    }
    public static void OnDisable()
    {
        _inputActions.Dialogue.Disable();
    }
    public void OnNextPhrase(InputAction.CallbackContext context)
    {
        if (context.started && _dialogue.DialoguePlay)
        {
            try
            {
                _dialogue.ContinueStory(_dialogue._choiceButtonsPanel.activeInHierarchy);
            }
            catch
            {

            }
        }
    }
}
