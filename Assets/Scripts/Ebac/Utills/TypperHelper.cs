using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class TypperHelper : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float timeLetters = .1f;
    public string phrase;

    #region ========== DEBUG ==========

    [NaughtyAttributes.Button]
    public void StartType()
    {
        StartCoroutine(TypeCoroutine(phrase));
    }

    #endregion

    private void Start()
    {
        //StartCoroutine(TypeCoroutine(phrase));
    }

    IEnumerator TypeCoroutine(string s)
    {
        textMesh.text = "";

        foreach(char c in s.ToCharArray())
        {
            textMesh.text += c;
            yield return new WaitForSeconds(timeLetters);
        }
    }
}
