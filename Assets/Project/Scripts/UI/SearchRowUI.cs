using UnityEngine;
using TMPro;

public class SearchRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _isbnText;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _authorText;
    [SerializeField] private TextMeshProUGUI _totalCopiesText;
    [SerializeField] private TextMeshProUGUI _borrowedCopiesText;
    public void SetData(Book book)
    {
        _isbnText.text = book.Isbn;
        _titleText.text = book.Title;
        _authorText.text = book.Author;
        _totalCopiesText.text = book.TotalCopies.ToString();
        _borrowedCopiesText.text = book.BorrowedCopies.ToString();
    }
}
