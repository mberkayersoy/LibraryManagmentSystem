using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserManager : MonoBehaviour, IFeedBack
{
    [SerializeField] private static User _currentUser;

    [Header("Listening Events")]
    [SerializeField] private UserEventChannelSO _tryAddUserChannel;
    [SerializeField] private SignInEventChannelSO _trySignInChannel;
    [SerializeField] private BookEventChannelSO _tryBorrowBookEventChannel;
    [SerializeField] private BookEventChannelSO _bookReturned;

    [Header("BroadCast Events")]
    [SerializeField] private BookEventChannelSO _userBorrowedBook;
    [SerializeField] private FeedBackEventChannelSO _feedBackChannel;
    [SerializeField] private VoidEventChannelSO _userSuccessfullyAdded;
    [SerializeField] private UserEventChannelSO _userSuccessfullySignIn;

    private static Dictionary<string, User> _userList = new Dictionary<string, User>();

    private FeedBack _feedBack = new FeedBack();
    public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

    private void Awake()
    {
        _userList = SaveLoadManager<User>.Load(gameObject.name);
        _tryAddUserChannel.OnEventRaised += TryAddNewUser;
        _trySignInChannel.OnEventRaised += CheckUser;
        _tryBorrowBookEventChannel.OnEventRaised += TryBorrowBook;
        _bookReturned.OnEventRaised += HandleUserReturnBook;

        //User newUser = new User("123456", "Berkay", "123456");
        //User newUser1 = new User("1234567", "Muhammet", "123456");
        //_userList.Add(newUser.PhoneNumber, newUser);
        //_userList.Add(newUser1.PhoneNumber, newUser1);
    }
    private void HandleUserReturnBook(Book book)
    {
        _userList[_currentUser.PhoneNumber].ReturnBook(book);
    }

    private void TryBorrowBook(Book book)
    {
        BorrowedBook borrowedBook = new BorrowedBook(_currentUser.Name, _currentUser.PhoneNumber, book);

        if (_userList[_currentUser.PhoneNumber].AddBook(borrowedBook))
        {
            _feedBack.Content = "Book Borrowed.";
            _userBorrowedBook.RaiseEvent(book);
        }
        else
        {
            _feedBack.Content = "You have already borrowed this book." +
    " You cannot borrow another copy of the same book without returning it.";
        }
        _feedBackChannel.RaiseEvent(_feedBack);
    }


    private void CheckUser(UserSignInData signInData)
    {
        if (signInData == null)
        {
            _userSuccessfullySignIn.RaiseEvent(null);
            _currentUser = null;
            return;
        }

        if (_userList.ContainsKey(signInData.PhoneNumber) && _userList[signInData.PhoneNumber].Password.Equals(signInData.Password))
        {
            _userSuccessfullySignIn.RaiseEvent(_userList[signInData.PhoneNumber]);
            _currentUser = _userList[signInData.PhoneNumber];
        }
        else
        {
            SetFeedBack("Phone number or password is incorrect.");
        }
    }

    private void TryAddNewUser(User user)
    {
        if (_userList.ContainsKey(user.PhoneNumber))
        {
            SetFeedBack("The phone number is already used by another user.");
        }
        else
        {
            _userList.Add(user.PhoneNumber, user);
            _userSuccessfullyAdded.RaiseEvent();
            SetFeedBack("Account Created \n You can log in.");
        }
    }
    private void SetFeedBack(string content)
    {
        FeedBack.Content = content;
        _feedBackChannel.RaiseEvent(_feedBack);
    }


    public static Dictionary<string, BorrowedBook> GetUserBorrowedBooks()
    {
        return _userList[_currentUser.PhoneNumber].BorrowedBooks;
    }
    public static List<BorrowedBook> GetAllBorrowedBooks()
    {
        List<BorrowedBook> allBorrowedBooks = new List<BorrowedBook>();

        foreach (var item in _userList)
        {
            foreach (var book in item.Value.BorrowedBooks)
            {
                allBorrowedBooks.Add(book.Value);
            }
        }
        return allBorrowedBooks;
    }

    public static List<BorrowedBook> GetAllOverdueBooks()
    {
        List<BorrowedBook> allBorrowedBooks = new List<BorrowedBook>();

        foreach (var item in _userList)
        {
            foreach (var book in item.Value.BorrowedBooks)
            {
                if (book.Value.IsOverdued())
                {
                    allBorrowedBooks.Add(book.Value);
                }
            }
        }
        return allBorrowedBooks;
    }
    private  void OnDestroy()
    {
        SaveLoadManager<User>.Save(_userList, gameObject.name);
        _tryAddUserChannel.OnEventRaised -= TryAddNewUser;
        _trySignInChannel.OnEventRaised -= CheckUser;
        _tryBorrowBookEventChannel.OnEventRaised -= TryBorrowBook;
        _bookReturned.OnEventRaised -= HandleUserReturnBook;
    }
}
