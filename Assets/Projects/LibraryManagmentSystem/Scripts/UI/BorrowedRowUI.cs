using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LibraryManagmentSystem
{
    public class BorrowedRowUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _borrowerText;
        [SerializeField] private TextMeshProUGUI _isbnText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _authorText;
        [SerializeField] private TextMeshProUGUI _borrowDateText;
        [SerializeField] private TextMeshProUGUI _overdueDateText;

        public void SetData(BorrowedBook borrowedBook)
        {
            _borrowerText.text = borrowedBook.BorrowerName;
            _isbnText.text = borrowedBook.Book.Isbn;
            _authorText.text = borrowedBook.Book.Author;
            _titleText.text = borrowedBook.Book.Title;
            _borrowDateText.text = borrowedBook.BorrowDate.ToString();
            _overdueDateText.text = borrowedBook.ReturnDueDate.ToString();
        }
    }
}
