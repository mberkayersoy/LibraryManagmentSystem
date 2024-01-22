using System;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class User : IFeedBack
{
    private string _name;
    private string _password;
    private string _phoneNumber;
    private Dictionary<string, BorrowedBook> _borrowedBooks;
    private FeedBack _feedBack;
    public User(string phoneNumber, string name, string password)
    {
        _phoneNumber = phoneNumber;
        _name = name;
        _password = password;
        _borrowedBooks = new Dictionary<string, BorrowedBook>();
        _feedBack = new FeedBack();
    }

    public string Name { get => _name; private set => _name = value; }
    public string Password { get => _password; private set => _password = value; }
    public string PhoneNumber { get => _phoneNumber; private set => _phoneNumber = value; }
    public Dictionary<string, BorrowedBook> BorrowedBooks { get => _borrowedBooks; private set => _borrowedBooks = value; }

    public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

    public bool AddBook(BorrowedBook borrowedBook)
    {
        if (_borrowedBooks.ContainsKey(borrowedBook.Book.Isbn))
        {
            _feedBack.Content = "You have already borrowed this book." +
                " You cannot borrow another copy of the same book without returning it.";
            return false;
        }
        else
        {
            _borrowedBooks.Add(borrowedBook.Book.Isbn, borrowedBook);
            _feedBack.Content = "Book Borrowed.";
            return true; 
        }
    }
    public void ReturnBook()
    {

    }
}