using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LibraryManagmentSystem
{
    [CreateAssetMenu(fileName = "FeedBackEventChannelSO", menuName = "Events/Library/FeedBack Event Channel")]
    public class FeedBackEventChannelSO : GenericEventChannelSO<FeedBack>
    {

    }
    public class FeedBack
    {
        public string Content;
    }

    public interface IFeedBack
    {
        public FeedBack FeedBack { get; }
    }
}

