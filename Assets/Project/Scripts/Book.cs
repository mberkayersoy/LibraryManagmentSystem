using System;

public class Book
{
    private string _isbn;
    private string _title;
    private string _author;
    private int _totalCopies;
    private int _borrowedCopies;
    public string Title { get => _title; private set => _title = value; }
    public string Author { get => _author; private set => _author = value; }
    public string Isbn { get => _isbn; private set => _isbn = value; }
    public int TotalCopies { get => _totalCopies; set => _totalCopies = value; }
    public int BorrowedCopies { get => _borrowedCopies; set => _borrowedCopies = value; }

    public void AddCopy(int add)
    {
        TotalCopies += add;
    }
    public Book(string isbn, string title, string author)
    {
        _isbn = isbn;
        _title = title;
        _author = author;
        _borrowedCopies = 0;
    }
    public Book(string isbn, string title, string author, int addedCopy)
    {
        _isbn = isbn;
        _title = title;
        _author = author;
        _totalCopies = addedCopy;
        _borrowedCopies = 0;
    }
}
[Serializable]
public class BorrowedBook
{
    private User _borrower;
    private Book _book;
    private DateTime _borrowDate;
    private DateTime _returnDueDate;

    public BorrowedBook(User borrower, Book book, int returnDueTime = 2)
    {
        _borrower = borrower;
        _book = book;
        _borrowDate = DateTime.Now;
        _returnDueDate = _borrowDate.AddMinutes(returnDueTime);
    }

    public DateTime BorrowDate { get => _borrowDate; }
    public DateTime ReturnDueDate { get => _returnDueDate; }
    public User Borrower { get => _borrower; private set => _borrower = value; }
    public Book Book { get => _book; private set => _book = value; }
}
