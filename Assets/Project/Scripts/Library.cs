using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Library : IFeedBack
{

    private Dictionary<string, Book> _bookList = new Dictionary<string, Book>();
    public Dictionary<string, Book> BookList { get => _bookList; private set => _bookList = value; }
    private FeedBack _feedBack = new FeedBack();
    public FeedBack FeedBack { get => _feedBack; set => _feedBack = value; }

    public void AddBook(string isbn, string title, string author, int addedCoppy = 0)
    {
        if (_bookList.ContainsKey(isbn))
        {
            _bookList[isbn].TotalCopies++;
        }
        else
        {
            _bookList.Add(isbn, new Book(isbn, title, author));
        }
    }

    public FeedBack TryAddNewBook(Book newBook)
    {
        if (_bookList.ContainsKey(newBook.Isbn))
        {
            return CheckBookContent(newBook);
        }
        else
        {
            _bookList.Add(newBook.Isbn, newBook);
            _feedBack.Content = "New Book Added.";
            return _feedBack;
        }

    }

    private FeedBack CheckBookContent(Book newBook)
    {
        if (_bookList[newBook.Isbn].Title.ToLower() == newBook.Title.ToLower() &&
            _bookList[newBook.Isbn].Author.ToLower() == newBook.Author.ToLower())
        {
            _feedBack.Content = "The newly added book already exists in the library. The number of copies was increased.";
            IncreaseExistBookCopy(newBook);
            return _feedBack;
        }
        else
        {
            _feedBack.Content = "The ISBN number entered already exists as a different book in the library. Please check the data you entered.";
            return _feedBack;
        }
    }

    private void IncreaseExistBookCopy(Book existBook)
    {
        _bookList[existBook.Isbn].TotalCopies += existBook.TotalCopies;
    }

    public Dictionary<string, Book> GetAllBooks()
    {
        return _bookList;
    }

    public List<Book> SearchBook(string bookData)
    {
        if (string.IsNullOrEmpty(bookData)) return null;

        List<Book> books = new List<Book>();

        foreach (var book in _bookList)
        {
            if (book.Value.Author.ToLower().Contains(bookData.ToLower()) ||
                book.Value.Title.ToLower().Contains(bookData.ToLower()))
            {
                books.Add(book.Value);
            }
        }
        return books;
    }


    private void BorrowBook(string isbn)
    {
        if (_bookList.ContainsKey(isbn))
        {
            if (_bookList[isbn].TotalCopies > 0)
            {
                _bookList[isbn].TotalCopies--;
            }
        }
        else
        {
            
        }
    }

    private void ReturnBook(string isbn)
    {
        if (_bookList.ContainsKey(isbn))
        {
            _bookList[isbn].TotalCopies++;
        }
        else
        {
            
        }
    }

    private void GetOverdueBooks()
    {

    }
}
