using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField _searchInput;
    [SerializeField] private Transform _viewContent;
    [SerializeField] private SearchRowUI _rowTemplate;

    [Header("Listening Events")]
    [SerializeField] private AddExistBookDataEventChannelSO _addExistBookChannel;
    [SerializeField] private BookEventChannelSO _bookBorrowed;
    private void Awake()
    {
        _addExistBookChannel.OnEventRaised += Refresh;
        _bookBorrowed.OnEventRaised += Refresh;
    }

    private void Refresh(Book book)
    {
        DisplayBooks("");
    }

    private void Refresh(ExistBookAddData data)
    {
        DisplayBooks("");
    }

    private void OnEnable()
    {
        _searchInput.onValueChanged.AddListener(DisplayBooks);
        DisplayBooks("");
    }

    private void OnDisable()
    {
        _searchInput.onValueChanged.RemoveListener(DisplayBooks);
    }
    private void DisplayBooks(string input)
    {
        CleanRows();

        List<Book> displayingBooks;
        if (string.IsNullOrEmpty(input))
        {
            displayingBooks = LibraryManager.GetBooks();
        }
        else
        {
            displayingBooks = LibraryManager.GetBooks(input);
        }

        foreach (var item in displayingBooks)
        {
            SearchRowUI row = Instantiate(_rowTemplate, _viewContent);
            row.SetData(item);
        }
    }

    private void CleanRows()
    {
        foreach (Transform child in _viewContent.transform)
        {
            if (child == _viewContent.transform || child == _rowTemplate.transform) continue;

            Destroy(child.gameObject);
        }
    }
    private void OnDestroy()
    {
        _addExistBookChannel.OnEventRaised -= Refresh;
        _bookBorrowed.OnEventRaised -= Refresh;
    }
}
