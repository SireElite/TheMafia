using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialoguePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _TMP;
    [SerializeField] private AudioClip _typeSound;

    public static DialoguePanel Instance { get; private set; }

    public bool SentenceIsTyping;

    private float _typingSpeed;
    private AudioSource _audioSource;

    public void Initialize()
    {
        #region Singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception($"More than one {name} Instance");
        }
        #endregion

        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        DialogueEntity.OnWantsToSay += TypeSentence;
        Host.OnGameRestart += PrepareToRestart;
    }

    private void OnDisable()
    {
        DialogueEntity.OnWantsToSay -= TypeSentence;
        Host.OnGameRestart -= PrepareToRestart;
    }

    private void PrepareToRestart() => StopAllCoroutines();

    public void SetTypingSpeed(float typingRate) => _typingSpeed = 1f / typingRate;

    private void TypeSentence(string sentence, DialogueEntity dialogueEntity)
    {
        StartCoroutine(TypeCoroutine(sentence, dialogueEntity.DialogueName));
    }

    private IEnumerator TypeCoroutine(string sentence, string speakerName)
    {
        SentenceIsTyping = true;
        _TMP.text = $"{speakerName}: ";

        foreach(var character in sentence)
        {
            _TMP.text += character;
            _audioSource.PlayOneShot(_typeSound);
            yield return new WaitForSeconds(_typingSpeed);
        }

        yield return new WaitForSeconds(1f);
        SentenceIsTyping = false;
    }
}
