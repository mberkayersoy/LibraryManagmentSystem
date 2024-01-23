using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExistBookAddData
{
    public Book ExistBook;
    public int AddedCopy;

    public ExistBookAddData(Book existBook, int addedCopy)
    {
        ExistBook = existBook;
        AddedCopy = addedCopy;
    }
}

public class AddExistBookUI : MonoBehaviour, IFeedBack
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _addButton;
    [SerializeField] private TextMeshProUGUI _authorText;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _existCopyText;
    [SerializeField] private TMP_InputField _copyCountInput;

    [Header("Broadcast Events")]
    [SerializeField] private AddExistBookDataEventChannelSO _addExistBookChannel;
    [SerializeField] private FeedBackEventChannelSO _feedBackChannel;
    private Book _existBook;

    private FeedBack _feedBack = new FeedBack();
    public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

    private void Awake()
    {
        _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        _addButton.onClick.AddListener(AddBook);
    }

    private void AddBook()
    {
        if (string.IsNullOrEmpty(_copyCountInput.text)) {
            return;
        }
        else
        {
            if (Int32.Parse(_copyCountInput.text) > 0)
            {
                _addExistBookChannel.RaiseEvent(new ExistBookAddData(_existBook, Int32.Parse(_copyCountInput.text)));
                _feedBack.Content = "Book added.";
                _copyCountInput.text = string.Empty;
                _feedBackChannel.RaiseEvent(_feedBack);
                gameObject.SetActive(false);
            }
        }
    }

    public void SetData(Book book)
    {
        _existBook = book;
        _authorText.text = "Author: " + book.Author;
        _titleText.text = "Title: " + book.Title;
        _existCopyText.text = "Total Copy: " + book.TotalCopies.ToString();
    }

    private void OnDestroy()
    {
        _closeButton.onClick.RemoveListener(() => gameObject.SetActive(false)); 
        _addButton.onClick.RemoveListener(AddBook);
    }
}
