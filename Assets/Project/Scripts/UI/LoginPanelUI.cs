using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserSignInData
{
    public string PhoneNumber;
    public string Password;

    public UserSignInData(string phoneNumber, string password)
    {
        PhoneNumber = phoneNumber;
        Password = password;
    }
}
public class LoginPanelUI : MonoBehaviour, IFeedBack
{
    [SerializeField] private Button _loginButton;
    [SerializeField] private Button _showSignUpPanelButton;
    [SerializeField] private Button _guestButton;
    [SerializeField] private InputFieldData[] _userInputFields;

    [Header("Broadcast Events")]
    [SerializeField] private FeedBackEventChannelSO _feedbackChannel;
    [SerializeField] private SignInEventChannelSO _userSignInChannel;
    [SerializeField] private VoidEventChannelSO _showSignUpPanel;

    private Dictionary<string, TMP_InputField> _inputFieldsDic = new Dictionary<string, TMP_InputField>();
    private FeedBack _feedBack = new FeedBack();
    public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

    private void Awake()
    {
        foreach (var inputField in _userInputFields)
        {
            _inputFieldsDic[inputField.PropertyName] = inputField.InputField;
        }

        _loginButton.onClick.AddListener(() => TrySignIn());
        _showSignUpPanelButton.onClick.AddListener(() => _showSignUpPanel.RaiseEvent());
        _guestButton.onClick.AddListener(() => _userSignInChannel.RaiseEvent(null));
    }

    private void TrySignIn()
    {
        if (CheckInputs())
        {
            string phoneNumber = UIManager.GetInputFieldValue(_inputFieldsDic, "PhoneNumber");
            string password = UIManager.GetInputFieldValue(_inputFieldsDic, "Password");
            _userSignInChannel.RaiseEvent(new UserSignInData(phoneNumber, password));
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
        _loginButton.onClick.RemoveListener(() => CheckInputs());
        _guestButton.onClick.RemoveListener(() => _userSignInChannel.RaiseEvent(null));
    }

}
