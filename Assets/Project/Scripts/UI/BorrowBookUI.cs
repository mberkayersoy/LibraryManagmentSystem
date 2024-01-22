using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BorrowBookUI : MonoBehaviour
{
    [SerializeField] private Button _borrowButton;
    [SerializeField] private Button _closeWindowButton;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _authorText;
    [SerializeField] private TextMeshProUGUI _overdueDateText;
    [Header("Broadcast Events")]
    [SerializeField] private BookEventChannelSO _userBorrowedBook;
    private Book _rowBook;
    private DateTime _overdueDate;
    [SerializeField] private double _overdueMinute = 2;

    private void Awake()
    {
        _borrowButton.onClick.AddListener(Borrow);
        _closeWindowButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void Borrow()
    {
        _userBorrowedBook.RaiseEvent(_rowBook);
    }

    public void SetData(Book book)
    {
        _rowBook = book;
        _titleText.text = "Title: " + book.Title;
        _authorText.text = "Author: " + book.Author;
        _overdueDate = DateTime.Now.AddMinutes(_overdueMinute);
        _overdueDateText.text = "Overdue Date: " + _overdueDate.ToString();
    }

    private void OnDestroy()
    {
        _borrowButton.onClick.RemoveListener(Borrow);
        _closeWindowButton.onClick.RemoveListener(() => gameObject.SetActive(false));
    }
}
