using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LibraryManagmentSystem
{
    [CreateAssetMenu(fileName = "BookListEventChannelSO", menuName = "Events/Library/BookList Event Channel")]
    public class BookListEventChannelSO : GenericEventChannelSO<List<Book>>
    {
    }
}