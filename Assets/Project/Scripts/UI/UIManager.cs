using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[System.Serializable]
public class InputFieldData
{
    public TMP_InputField InputField;
    public string PropertyName;
}
public class UIManager : MonoBehaviour
{
    [Header("FEEDBACK")]
    [SerializeField] private TextMeshProUGUI _feedBackText;
    [SerializeField] private Image _feedBackBackground;
    [SerializeField] private TextMeshProUGUI _userText;
    [Header("BUTTONS")]
    [SerializeField] private Button _showAddButtonPanelButton;
    [SerializeField] private Button _showSearchBooksPanelButton;
    [SerializeField] private Button _showReturnBookPanelButton;
    [SerializeField] private Button _showOverdueBooksPanelButton;
    [SerializeField] private Button _showAllBorrowedBooksPanelButton;
    [SerializeField] private Button _overdueBooksPanelButton;
    [SerializeField] private Button _logOutButton;
    [SerializeField] private Button _quitButton;
    [Header("UI PANELS")]
    [SerializeField] private Transform _libraryPanel;
    [SerializeField] private Transform _searchPanel;
    [SerializeField] private Transform _addBookPanel;
    [SerializeField] private Transform _signInPanel;
    [SerializeField] private Transform _signUpPanel;
    [SerializeField] private Transform _returnPanel;
    [SerializeField] private Transform _allBorrowedBooksPanel;
    [SerializeField] private Transform _overdueBooksPanel;
    [Header("POPUPS")]
    [SerializeField] private AddExistBookUI _addExistBookPopUp;
    [SerializeField] private BorrowBookUI _borrowBookPopUp;
    [Header("Listening Events")]
    [SerializeField] private FeedBackEventChannelSO _feedBackTextChannel;
    [SerializeField] private UserEventChannelSO _userSignInChannel;
    [SerializeField] private VoidEventChannelSO _showSignUpPanelChannel;
    [SerializeField] private VoidEventChannelSO _backToSignInPanelChannel;
    [SerializeField] private BookEventChannelSO _showAddBookPopUpChannel;
    [SerializeField] private AddExistBookDataEventChannelSO _addExistBookChannel;
    [SerializeField] private BookEventChannelSO _showBorrowBookPopUpChannel;
    [SerializeField] private BookEventChannelSO _bookBorrowedEventChannel;
    [Header("BroadCast Events")]
    [SerializeField] private BookListEventChannelSO _getAllBooksChannel;
    [SerializeField] private VoidEventChannelSO _userSuccessfullyAdded;

    private FeedBack _feedBack = new FeedBack();
    private Coroutine _feedBackCoroutine;
    private User _currentUser;
    private void Awake()
    {
        _userSuccessfullyAdded.OnEventRaised += ShowLogInScreen;
        _feedBackTextChannel.OnEventRaised += DisplayFeedBack;
        _userSignInChannel.OnEventRaised += ShowLibraryPanel;
        _showSignUpPanelChannel.OnEventRaised += ShowSignUpPanel;
        _showAddBookPopUpChannel.OnEventRaised += ShowAddBookPopUp;
        _showBorrowBookPopUpChannel.OnEventRaised += TryShowBorrowBookPopUp;
        _addExistBookChannel.OnEventRaised += HideAddBookPopUp;
        _bookBorrowedEventChannel.OnEventRaised += HideBorrowBookPopUp;
        _backToSignInPanelChannel.OnEventRaised += () => SetActivePanel(_signInPanel.name);
        _showSearchBooksPanelButton.onClick.AddListener(() => SetActivePanel(_searchPanel.name));
        _showAddButtonPanelButton.onClick.AddListener(() => SetActivePanel(_addBookPanel.name));
        _showReturnBookPanelButton.onClick.AddListener(TryShowReturnBookPanel);
        _showAllBorrowedBooksPanelButton.onClick.AddListener(() => SetActivePanel(_allBorrowedBooksPanel.name));
        _overdueBooksPanelButton.onClick.AddListener(() => SetActivePanel(_overdueBooksPanel.name));
        _logOutButton.onClick.AddListener(() => SetActivePanel(_signInPanel.name));
        _quitButton.onClick.AddListener(QuitButtonClicked);

        SetActivePanel(_signInPanel.name);
    }

    public void QuitButtonClicked()
    {
        #if UNITY_EDITOR
        if (Application.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        #endif

        #if !UNITY_EDITOR
        Application.Quit();
        #endif
    }

    private void TryShowReturnBookPanel()
    {
        if (_currentUser != null)
        {
            SetActivePanel(_returnPanel.name);
        }
        else
        {
            _feedBack.Content = "If you want to return a book, please log in.";
            DisplayFeedBack(_feedBack);
        }
    }

    private void HideBorrowBookPopUp(Book book)
    {
        _borrowBookPopUp.gameObject.SetActive(false);
    }

    private void TryShowBorrowBookPopUp(Book book)
    {
        if (_currentUser != null)
        {
            _borrowBookPopUp.gameObject.SetActive(true);
            _borrowBookPopUp.SetData(book);
        }
        else
        {
            _feedBack.Content = "Guests cannot borrow books. If you want to borrow a book, please log in as a user.";
            DisplayFeedBack(_feedBack);
        }
    }

    private void HideAddBookPopUp(ExistBookAddData data)
    {
        _addExistBookPopUp.gameObject.SetActive(false);
    }

    private void ShowAddBookPopUp(Book book)
    {
        _addExistBookPopUp.gameObject.SetActive(true);
        _addExistBookPopUp.SetData(book);
    }

    private void ShowSignUpPanel()
    {
        SetActivePanel(_signUpPanel.name);
    }

    private void ShowLibraryPanel(User user)
    {
        SetActivePanel(_libraryPanel.name);
        if (user == null)
        {
            _currentUser = null;
            _userText.text = "Current User: Guest";
        }
        else
        {
            _currentUser = user;
            _userText.text = "Current User: " + user.Name;
        }
    }

    private void ShowLogInScreen()
    {
        SetActivePanel(_signInPanel.name);
    }

    private void DisplayFeedBack(FeedBack feedback)
    {
        _feedBackBackground.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        _feedBackText.text = feedback.Content;

        if (_feedBackCoroutine != null)
        {
            StopCoroutine(_feedBackCoroutine);
        }
        _feedBackCoroutine = StartCoroutine(CleanFeedBackText());
    }
    private IEnumerator CleanFeedBackText()
    {
        yield return new WaitForSeconds(2f);
        _feedBackBackground.color = new Color();
        _feedBackText.text = "";
        _feedBackCoroutine = null;
    }
    private void SetActivePanel(string activatePanel)
    {
        if (activatePanel == _signInPanel.name || activatePanel == _signUpPanel.name)
        {
            _signInPanel.gameObject.SetActive(activatePanel.Equals(_signInPanel.name));
            _signUpPanel.gameObject.SetActive(activatePanel.Equals(_signUpPanel.name));
            _libraryPanel.gameObject.SetActive(false);
            _searchPanel.gameObject.SetActive(false);
            _addBookPanel.gameObject.SetActive(false);
            _returnPanel.gameObject.SetActive(false);
            _allBorrowedBooksPanel.gameObject.SetActive(false);
            _overdueBooksPanel.gameObject.SetActive(false);
        }
        else
        {
            _signInPanel.gameObject.SetActive(false);
            _signUpPanel.gameObject.SetActive(false);
            _libraryPanel.gameObject.SetActive(true);
            _searchPanel.gameObject.SetActive(activatePanel.Equals(_searchPanel.name));
            _addBookPanel.gameObject.SetActive(activatePanel.Equals(_addBookPanel.name));
            _returnPanel.gameObject.SetActive(activatePanel.Equals(_returnPanel.name));
            _allBorrowedBooksPanel.gameObject.SetActive(activatePanel.Equals(_allBorrowedBooksPanel.name));
            _overdueBooksPanel.gameObject.SetActive(activatePanel.Equals(_overdueBooksPanel.name));
        }
    }

    public static string GetInputFieldValue(Dictionary<string, TMP_InputField> inputFieldsDic, string propertyName)
    {
        if (inputFieldsDic.TryGetValue(propertyName, out TMP_InputField inputField))
        {
            return inputField.text;
        }
        return null;
    }

    private void OnDestroy()
    {
        _userSuccessfullyAdded.OnEventRaised -= ShowLogInScreen;
        _feedBackTextChannel.OnEventRaised -= DisplayFeedBack;
        _userSignInChannel.OnEventRaised -= ShowLibraryPanel;
        _showSignUpPanelChannel.OnEventRaised -= ShowSignUpPanel;
        _showAddBookPopUpChannel.OnEventRaised -= ShowAddBookPopUp;
        _showBorrowBookPopUpChannel.OnEventRaised -= TryShowBorrowBookPopUp;
        _addExistBookChannel.OnEventRaised -= HideAddBookPopUp;
        _bookBorrowedEventChannel.OnEventRaised -= HideBorrowBookPopUp;
        _backToSignInPanelChannel.OnEventRaised -= () => SetActivePanel(_signInPanel.name);
        _showSearchBooksPanelButton.onClick.RemoveListener(() => SetActivePanel(_searchPanel.name));
        _showAddButtonPanelButton.onClick.RemoveListener(() => SetActivePanel(_addBookPanel.name));
        _showReturnBookPanelButton.onClick.RemoveListener(TryShowReturnBookPanel);
        _showAllBorrowedBooksPanelButton.onClick.RemoveListener(() => SetActivePanel(_allBorrowedBooksPanel.name));
        _overdueBooksPanelButton.onClick.RemoveListener(() => SetActivePanel(_overdueBooksPanel.name));
        _logOutButton.onClick.RemoveListener(() => SetActivePanel(_signInPanel.name));
        _quitButton.onClick.RemoveListener(QuitButtonClicked);
    }
}
