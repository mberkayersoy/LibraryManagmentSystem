using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour, IFeedBack
{
    [SerializeField] private User _currentUser;

    [Header("Listening Events")]
    [SerializeField] private UserEventChannelSO _tryAddUserChannel;
    [SerializeField] private SignInEventChannelSO _trySignInChannel;
    [SerializeField] private BookEventChannelSO _tryBorrowBookEventChannel;

    [Header("BroadCast Events")]
    [SerializeField] private BookEventChannelSO _userBorrowedBook;
    [SerializeField] private FeedBackEventChannelSO _feedBackChannel;
    [SerializeField] private VoidEventChannelSO _userSuccessfullyAdded;
    [SerializeField] private UserEventChannelSO _userSuccessfullySignIn;

    private Dictionary<string,User> _userList = new Dictionary<string,User>();

    private FeedBack _feedBack = new FeedBack();
    public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

    private void Awake()
    {
        _tryAddUserChannel.OnEventRaised += TryAddNewUser;
        _trySignInChannel.OnEventRaised += CheckUser;
        _tryBorrowBookEventChannel.OnEventRaised += TryBorrowBook;

        User newUser = new User("123456", "Berkay", "123456");
        _userList.Add(newUser.PhoneNumber, newUser);
    }

    private void TryBorrowBook(Book book)
    {
        BorrowedBook borrowedBook = new BorrowedBook(_currentUser, book);

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


    private void CheckUser(UserSignInData user)
    {
        if (user == null)
        {
            _userSuccessfullySignIn.RaiseEvent(null);
            _currentUser = null;
            return;
        }

        if (_userList.ContainsKey(user.PhoneNumber))
        {
            _userSuccessfullySignIn.RaiseEvent(_userList[user.PhoneNumber]);
            _currentUser = _userList[user.PhoneNumber];
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

    private void OnDestroy()
    {
        _tryAddUserChannel.OnEventRaised -= TryAddNewUser;
        _trySignInChannel.OnEventRaised -= CheckUser;
    }
}

public enum UserTypes
{
    User,
    Visitor,
}
