using StarworkGC.Localization;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text textBox;

    public Button playDialogue1Button;

    [TextArea]
    public string dialogue1;
    [TextArea]
    public string dialogue2;
    [TextArea]
    public string dialogue3;

    private DialogueVertexAnimator dialogueVertexAnimator;
    void Awake() {
        dialogueVertexAnimator = new DialogueVertexAnimator(textBox);
        playDialogue1Button.onClick.AddListener(delegate { PlayDialogue1(); });
        dialogue1 = Localization.Get("cadiNelVuototst");
    }

    private void PlayDialogue1() {
        PlayDialogue(dialogue1);
    }

    private Coroutine typeRoutine = null;
    void PlayDialogue(string message) {
        this.EnsureCoroutineStopped(ref typeRoutine);
        dialogueVertexAnimator.textAnimating = false;
        List<DialogueCommand> commands = DialogueUtility.ProcessInputString(message, out string totalTextMessage);
        typeRoutine = StartCoroutine(dialogueVertexAnimator.AnimateTextIn(commands, totalTextMessage, null));
    }
}
