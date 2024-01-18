using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button _showAllBooksButton;
    [SerializeField] private LibraryManager libraryManager;
    [SerializeField] private Transform _viewContent;
    [SerializeField] private SearchRowUI _rowTemplate;
    [SerializeField] private TMP_InputField _searchInput;
    [SerializeField] private TextMeshProUGUI _feedBackText;
    [SerializeField] private FeedBackEventChannelSO _feedBackTextChannel;
    private Coroutine _feedBackCoroutine;
    private void Awake()
    {
        _showAllBooksButton.onClick.AddListener(ShowAllBooks);
        _searchInput.onValueChanged.AddListener(SearchBooks);
        _feedBackTextChannel.OnEventRaised += DisplayFeedBack;
    }

    private void DisplayFeedBack(FeedBack feedback)
    {
        _feedBackText.text = feedback.Content;

        if (_feedBackCoroutine != null)
        {
            StopCoroutine(_feedBackCoroutine);
        }
        _feedBackCoroutine = StartCoroutine(CleanFeedBackText());
    }
    private IEnumerator CleanFeedBackText()
    {
        yield return new WaitForSeconds(2f);
        _feedBackText.text = "";
        _feedBackCoroutine = null;
    }

    private void SearchBooks(string input)
    {
        CleanRows();

        if (string.IsNullOrEmpty(input)) return;

        foreach (var item in libraryManager.Library.SearchBook(input))
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
    private void ShowAllBooks()
    {
        CleanRows();

        foreach (var item in libraryManager.Library.GetAllBooks())
        {
            SearchRowUI row = Instantiate(_rowTemplate, _viewContent);
            row.SetData(item.Value);
        }
    }
}
