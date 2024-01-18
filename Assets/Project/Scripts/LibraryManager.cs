using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryManager : MonoBehaviour, IFeedBack
{
    [Header("Listening Events")]
    [SerializeField] private BookEventChannelSO _bookAdded;
    [Header("Broadcast Events")]
    [SerializeField] private FeedBackEventChannelSO _feedBackChannel;
    private Library _library;
    public Library Library { get => _library; private set => _library = value; }

    private FeedBack _feedBack = new FeedBack();
    public FeedBack FeedBack { get => _feedBack; private set => _feedBack = value; }

    private void Awake()
    {
        _bookAdded.OnEventRaised += HandleBookAdded;
        _library = new Library();

    }

    private void HandleBookAdded(Book book)
    {
        if (book != null)
        {
            _feedBackChannel.RaiseEvent(_library.TryAddNewBook(book));
        }
    }
}
