using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AddNewBookPanelUI : MonoBehaviour, IFeedBack
{
    [SerializeField] private InputFieldData[] _bookInputFields;
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
            string isbn = UIManager.GetInputFieldValue(_inputFieldsDic, "ISBN");
            string title = UIManager.GetInputFieldValue(_inputFieldsDic, "Title");
            string author = UIManager.GetInputFieldValue(_inputFieldsDic, "Author");
            int totalCopies = int.Parse(UIManager.GetInputFieldValue(_inputFieldsDic, "AddedCopy"));

            if (totalCopies <= 0)
            {
                SetFeedBack("number of copies");
                return;
            }

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
        FeedBack.Content = "The " + content + " field is not valid.";
        _feedbackChannel.RaiseEvent(_feedback);
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
        _tryAddButton.onClick.RemoveListener(TryAddBook);
    }
}
