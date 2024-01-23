using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnBookPanelUI : MonoBehaviour
{
    [SerializeField] private ReturnRowUI _returnRow;
    [SerializeField] private Transform _viewContent;
    [SerializeField] private BookEventChannelSO _bookReturned;

    private void Awake()
    {
        _bookReturned.OnEventRaised += Refresh;
    }

    private void Refresh(Book book)
    {
        DisplayBorrowedBooks();
    }

    private void OnEnable()
    {
        DisplayBorrowedBooks();
    }

    private void DisplayBorrowedBooks()
    {
        CleanRows();

        foreach (var item in UserManager.GetUserBorrowedBooks())    
        {
            ReturnRowUI row = Instantiate(_returnRow, _viewContent);
            row.SetData(item.Value.Book);
        }
    }

    private void CleanRows()
    {
        foreach (Transform child in _viewContent.transform)
        {
            if (child == _viewContent.transform) continue;

            Destroy(child.gameObject);
        }
    }

    private void OnDestroy()
    {
        _bookReturned.OnEventRaised -= Refresh;
    }
}
