using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class BookInputField
{
    public TMP_InputField InputField;
    public string PropertyName;
}

public class AddBookPanelUI : MonoBehaviour, IFeedBack
{
    [SerializeField] private BookInputField[] _bookInputFields;
    [SerializeField] private Button _tryAddButton;

    [Header("Broadcast Events")]
    [SerializeField] private BookEventChannelSO _tryAddBook;
    [SerializeField] private FeedBackEventChannelSO _feedbackChannel;

    private Dictionary<string, TMP_InputField> _inputFieldsDic = new Dictionary<string, TMP_InputField>();

    private FeedBack _feedback = new FeedBack();
    public FeedBack FeedBack { get => _feedback; private set => _feedback = value; }

    private void Awake()
    {
        _tryAddButton.onClick.AddListener(TryAddBook);

        foreach (var inputField in _bookInputFields)
        {
            _inputFieldsDic[inputField.PropertyName] = inputField.InputField;
        }
    }

    private void TryAddBook()
    {
        if (CheckInputs())
        {
            string isbn = GetInputFieldValue("ISBN");
            string title = GetInputFieldValue("Title");
            string author = GetInputFieldValue("Author");
            int totalCopies = int.Parse(GetInputFieldValue("AddedCopy"));

            Book newBook = new Book(isbn, title, author, totalCopies);
            _tryAddBook.RaiseEvent(newBook);
        }
    }

    private bool CheckInputs()
    {
        foreach (var inputField in _bookInputFields)
        {
            if (string.IsNullOrWhiteSpace(inputField.InputField.text))
            {
                SetFeedBack(inputField.PropertyName);
                return false;
            }
        }

        return true;
    }
    private void SetFeedBack(string content)
    {
        FeedBack.Content = "The " + content + " field can't be left blank.";
        _feedbackChannel.RaiseEvent(_feedback);
    }

    private string GetInputFieldValue(string propertyName)
    {
        if (_inputFieldsDic.TryGetValue(propertyName, out TMP_InputField inputField))
        {
            return inputField.text;
        }
        return null;
    }
}
