using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanelUI : MonoBehaviour, IFeedBack
{
    [Header("UI Elements")]
    [SerializeField] private Button _signUpButton;
    [SerializeField] private Button _backToLoginButton;
    [SerializeField] private InputFieldData[] _userInputFields;
    [Header("Broadcast Events")]
    [SerializeField] private UserEventChannelSO _tryAddUser;
    [SerializeField] private FeedBackEventChannelSO _feedbackChannel;
    [Header("Listening Events")]
    [SerializeField] private VoidEventChannelSO _userSuccessfullyAdded;
    [SerializeField] private VoidEventChannelSO _backToSignInPanel;

    private Dictionary<string, TMP_InputField> _inputFieldsDic = new Dictionary<string, TMP_InputField>();
    private FeedBack _feedBack = new FeedBack();
    public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

    private void Awake()
    {
        _signUpButton.onClick.AddListener(() => TryAddUser());
        _userSuccessfullyAdded.OnEventRaised += ClearInputs;
        _backToLoginButton.onClick.AddListener(() => _backToSignInPanel.RaiseEvent());
        foreach (var inputField in _userInputFields)
        {
            _inputFieldsDic[inputField.PropertyName] = inputField.InputField;
        }
    }

    private void TryAddUser()
    {
        if (CheckInputs())
        {
            string phoneNumber = UIManager.GetInputFieldValue(_inputFieldsDic, "PhoneNumber");
            string name = UIManager.GetInputFieldValue(_inputFieldsDic, "Name");
            string password = UIManager.GetInputFieldValue(_inputFieldsDic, "Password");

            User newUser = new User(phoneNumber, name, password);
            _tryAddUser.RaiseEvent(newUser);
        }
    }
    private bool CheckInputs()
    {
        foreach (var inputFieldData in _userInputFields)
        {
            if (string.IsNullOrWhiteSpace(inputFieldData.InputField.text))
            {
                SetFeedBack(inputFieldData.PropertyName);
                return false;
            }
        }

        return true;
    }

    private void SetFeedBack(string content)
    {
        FeedBack.Content = "The " + content + " field can't be left blank.";
        _feedbackChannel.RaiseEvent(_feedBack);
    }
    private void ClearInputs()
    {
        foreach (var item in _inputFieldsDic)
        {
            item.Value.text = "";
        }
    }

    private void OnDisable()
    {
        ClearInputs();
    }

    private void OnDestroy()
    {
        _signUpButton.onClick.RemoveListener(() => TryAddUser());
        _userSuccessfullyAdded.OnEventRaised -= ClearInputs;
        _backToLoginButton.onClick.RemoveListener(() => _backToSignInPanel.RaiseEvent());
    }
}
