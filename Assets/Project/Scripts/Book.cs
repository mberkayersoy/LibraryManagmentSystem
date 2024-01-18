using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    public Book(string isbn, string title, string author, int addedcopy = 1)
    {
        _isbn = isbn;
        _title = title;
        _author = author;
        _totalCopies = addedcopy;
        _borrowedCopies = 0;
    }
}
