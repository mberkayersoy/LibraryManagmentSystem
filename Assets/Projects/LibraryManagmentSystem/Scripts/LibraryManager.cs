using System.Collections.Generic;
using UnityEngine;

namespace LibraryManagmentSystem
{
    public class LibraryManager : MonoBehaviour
    {
        [Header("Listening Events")]
        [SerializeField] private BookEventChannelSO _newBookAdded;
        [SerializeField] private AddExistBookDataEventChannelSO _existBookAdded;
        [SerializeField] private BookEventChannelSO _bookBorrowed;
        [SerializeField] private BookEventChannelSO _bookReturned;
        [Header("Broadcast Events")]
        [SerializeField] private FeedBackEventChannelSO _feedBackChannel;

        private static Library _library = new Library();
        private FeedBack _feedBack = new FeedBack();
        public Library Library { get => _library; private set => _library = value; }
        public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

        private void Awake()
        {
            _library.BookList = SaveLoadManager<Book>.Load(gameObject.name);
            _newBookAdded.OnEventRaised += HandleNewBookAdded;
            _existBookAdded.OnEventRaised += HandleExistingBookAdded;
            _bookBorrowed.OnEventRaised += HandleBorrowedBook;
            _bookReturned.OnEventRaised += HandleReturnedBook;

        }

        private void HandleReturnedBook(Book book)
        {
            _library.ReturnBook(book.Isbn);
            _feedBack.Content = "The book returned";
            _feedBackChannel.RaiseEvent(_feedBack);
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
            SaveLoadManager<Book>.Save(_library.BookList, gameObject.name);
            _newBookAdded.OnEventRaised -= HandleNewBookAdded;
            _existBookAdded.OnEventRaised -= HandleExistingBookAdded;
            _bookBorrowed.OnEventRaised -= HandleBorrowedBook;
            _bookReturned.OnEventRaised -= HandleReturnedBook;
        }
    }
}