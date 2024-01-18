using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FeedBackEventChannelSO", menuName = "Events/FeedBack Event Channel")]
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

