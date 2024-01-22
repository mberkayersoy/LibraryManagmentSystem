using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SearchRowUI : MonoBehaviour, IFeedBack
{
    [Header("Broadcast Events")]
    [SerializeField] private BookEventChannelSO _showAddBookPopUpChannel;
    [SerializeField] private BookEventChannelSO _borrowBookChannel;
    [SerializeField] private FeedBackEventChannelSO _feedBackChannel;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _isbnText;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _authorText;
    [SerializeField] private TextMeshProUGUI _totalCopiesText;
    [SerializeField] private TextMeshProUGUI _borrowedCopiesText;
    [SerializeField] private Button _addButton;
    [SerializeField] private Button _borrowButton;
    private Book _rowBook;

    private FeedBack _feedBack = new FeedBack();
    public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

    private void Awake()
    {
        _addButton.onClick.AddListener(() => _showAddBookPopUpChannel.RaiseEvent(_rowBook));
        _borrowButton.onClick.AddListener(TryBorrow);
    }

    public void TryBorrow()
    {
        if (_rowBook.TotalCopies > 0)
        {
            _borrowBookChannel.RaiseEvent(_rowBook);
        }
        else
        {
            _feedBack.Content = "All copies of the book are on loan. Please try again later.";
            _feedBackChannel.RaiseEvent(_feedBack);
        }
    }
    public void SetData(Book book)
    {
        _isbnText.text = book.Isbn;
        _titleText.text = book.Title;
        _authorText.text = book.Author;
        _totalCopiesText.text = book.TotalCopies.ToString();
        _borrowedCopiesText.text = book.BorrowedCopies.ToString();
        _rowBook = book;
    }
}
