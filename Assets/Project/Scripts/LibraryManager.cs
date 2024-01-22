using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LibraryManager: MonoBehaviour, IFeedBack
{
    [Header("Listening Events")]
    [SerializeField] private BookEventChannelSO _newBookAdded;
    [SerializeField] private AddExistBookDataEventChannelSO _existBookAdded;
    [SerializeField] private BookEventChannelSO _bookBorrowed;
    [Header("Broadcast Events")]
    [SerializeField] private FeedBackEventChannelSO _feedBackChannel;

    private static Library _library;
    private FeedBack _feedBack = new FeedBack();
    public Library Library { get => _library; private set => _library = value; }
    public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

    private void Awake()
    {
        _newBookAdded.OnEventRaised += HandleNewBookAdded;
        _existBookAdded.OnEventRaised += HandleExistingBookAdded;
        _bookBorrowed.OnEventRaised += HandleBorrowedBook;
        _library = new Library();

        _library.AddBook(new Book("0", "1", "Author0"), 10);
        _library.AddBook(new Book("1", "2", "Author1"), 10);
        _library.AddBook(new Book("2", "3", "Author2"), 10);
        _library.AddBook(new Book("3", "4", "Author3"), 10);
        _library.AddBook(new Book("4", "5", "Author4"), 10);

    }

    private void HandleBorrowedBook(Book book)
    {
        _library.BorrowBook(book.Isbn);
    }

    public static List<Book> GetBooks(string input = null)
    {
        if (string.IsNullOrEmpty(input))
        {
            return _library.GetAllBooks();
        }
        else
        {
            return _library.SearchBook(input);
        }
    }
    private void HandleNewBookAdded(Book book)
    {
        if (book != null)
        {
            _feedBackChannel.RaiseEvent(_library.TryAddNewBook(book));
        }
    }
    private void HandleExistingBookAdded(ExistBookAddData existBookData)
    {
        _library.AddBook(existBookData.ExistBook, existBookData.AddedCopy);
    }

    private void OnDestroy()
    {
        _newBookAdded.OnEventRaised -= HandleNewBookAdded;
        _existBookAdded.OnEventRaised -= HandleExistingBookAdded;
    }
}
