using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReturnRowUI : MonoBehaviour
{
    [Header("UI ELEMENTS")]
    [SerializeField] private TextMeshProUGUI _isbnText;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _authorText;
    [SerializeField] private Button _returnButton;

    [Header("Broadcast Event")]
    [SerializeField] private BookEventChannelSO _returnBookChannel;

    private Book _rowBook;
    private void Awake()
    {
        _returnButton.onClick.AddListener(() => _returnBookChannel.RaiseEvent(_rowBook));
    }
    public void SetData(Book book)
    {
        _rowBook = book;
        _isbnText.text = book.Isbn;
        _titleText.text = book.Title;
        _authorText.text = book.Author;
    }

    private void OnDestroy()
    {
        _returnButton.onClick.RemoveListener(() => _returnBookChannel.RaiseEvent(_rowBook));
    }
}
