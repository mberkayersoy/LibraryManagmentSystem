using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverdueBooksPaneUI : MonoBehaviour
{
    [SerializeField] private BorrowedRowUI _borrowedRow;
    [SerializeField] private Transform _viewContent;

    private void OnEnable()
    {
        DisplayBorrowedBooks();
    }
    private void DisplayBorrowedBooks()
    {
        CleanRows();

        foreach (var item in UserManager.GetAllOverdueBooks())
        {
            BorrowedRowUI row = Instantiate(_borrowedRow, _viewContent);
            row.SetData(item);
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
}
