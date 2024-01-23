using System;
using System.Collections.Generic;

public class Library : IFeedBack
{
    private Dictionary<string, Book> _bookList = new Dictionary<string, Book>();
    public Dictionary<string, Book> BookList { get => _bookList; set => _bookList = value; }

    private FeedBack _feedBack = new FeedBack();
    public FeedBack FeedBack { get => _feedBack; set => _feedBack = value; }

    public void AddBook(Book book, int addedCopy = 1)
    {
        if (_bookList.ContainsKey(book.Isbn))
        {
            _bookList[book.Isbn].AddCopy(addedCopy);
        }
        else
        {
            _bookList.Add(book.Isbn, book);
            book.AddCopy(addedCopy);
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

    public void IncreaseExistBookCopy(Book existBook)
    {
        _bookList[existBook.Isbn].TotalCopies += existBook.TotalCopies;
    }

    public List<Book> GetAllBooks()
    {
        List<Book> allBooks = new List<Book>();
        foreach (var item in _bookList)
        {
            allBooks.Add(item.Value);
        }

        return allBooks;
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

    public void BorrowBook(string isbn)
    {
        if (_bookList.ContainsKey(isbn))
        {
            if (_bookList[isbn].TotalCopies > 0)
            {
                _bookList[isbn].TotalCopies--;
                _bookList[isbn].BorrowedCopies++;
            }
        }
    }

    public void ReturnBook(string isbn)
    {
        if (_bookList.ContainsKey(isbn))
        {
            _bookList[isbn].TotalCopies++;
            _bookList[isbn].BorrowedCopies--;
        }
    }
}
