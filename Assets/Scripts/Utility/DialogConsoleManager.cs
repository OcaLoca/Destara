/* ---------------------------------------------- 
 * Copyright � Mobybit
 * ----------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class DialogConsoleManager : MonoBehaviour
    {
        //Dialog
        bool LetterTypingIsFinish = false;
        public GameObject pnlDialogContainer;
        public TMP_Text txtDialog;
        [SerializeField] int letterPerSeconds = 30;

        public void CleanText()
        {
            txtDialog.text = string.Empty;
            Canvas.ForceUpdateCanvases();
        }
        public IEnumerator TypeDialogTxt(string dialog, bool assign = false)
        {
            if (assign) yield break;

            txtDialog.text = dialog; // Imposta il testo completo per il rendering
            txtDialog.ForceMeshUpdate(); // Assicura che il testo venga aggiornato

            TMP_TextInfo textInfo = txtDialog.textInfo;

            // Nascondi tutte le lettere (imposta alpha a 0)
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];
                if (charInfo.isVisible)
                {
                    SetCharacterAlpha(i, 0);
                }
            }

            txtDialog.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            // Scrittura progressiva: mostra una lettera alla volta
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                var charInfo = textInfo.characterInfo[i];

                if (charInfo.isVisible)
                {
                    SetCharacterAlpha(i, 255); // Imposta alpha a 255 (visibile)
                    txtDialog.UpdateVertexData(TMPro.TMP_VertexDataUpdateFlags.Colors32);
                    yield return new WaitForSeconds(1f / letterPerSeconds);
                }
            }

            yield return new WaitForSeconds(1);
        }

        // Funzione per modificare l'alpha di una singola lettera
        private void SetCharacterAlpha(int index, byte alpha)
        {
            TMPro.TMP_TextInfo textInfo = txtDialog.textInfo;
            int materialIndex = textInfo.characterInfo[index].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[index].vertexIndex;

            Color32[] vertexColors = textInfo.meshInfo[materialIndex].colors32;

            for (int j = 0; j < 4; j++) // Modifica i 4 vertici della lettera
            {
                vertexColors[vertexIndex + j].a = alpha;
            }
        }


        /// <summary>
        /// Mostra le parole al posto delle lettere
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="assign"></param>
        /// <returns></returns>
        public IEnumerator TypeDialogTxtVersionTwo(string dialog, bool assign = false)
        {
            if (assign) yield break;

            txtDialog.text = string.Empty;

            if (string.IsNullOrEmpty(dialog)) { yield break; }

            string currentText = "";
            bool insideTag = false;
            string tagBuffer = "";
            string wordBuffer = "";


            foreach (var letter in dialog.ToCharArray())
            {

                if (letter == '<') // Inizia un tag HTML
                {
                    insideTag = true;
                    tagBuffer += letter;
                }
                else if (letter == '>') // Finisce un tag HTML
                {
                    insideTag = false;
                    tagBuffer += letter;
                    wordBuffer += tagBuffer; // Aggiunge il tag alla parola
                    tagBuffer = ""; // Svuota il buffer del tag
                }
                else if (insideTag)
                {
                    tagBuffer += letter;
                }
                else if (char.IsWhiteSpace(letter) || letter == '.' || letter == ',' || letter == '!' || letter == '?')
                {
                    // Fine di una parola: Mostra la parola completa
                    currentText += wordBuffer + letter;
                    txtDialog.text = currentText;
                    wordBuffer = ""; // Reset word buffer
                    yield return new WaitForSeconds(1f / letterPerSeconds);
                }
                else
                {
                    wordBuffer += letter; // Accumula la parola fino al completamento
                }
                yield return null;
            }

            txtDialog.text += wordBuffer;

            txtDialog.text = currentText;

            yield return new WaitForSeconds(1);
        }
    }

}
