using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    public DifficultySetting[] stages;

    public int SlideCount => stages.Sum(stage => stage.slideCount);

    [Serializable]
    public struct DifficultySetting
    {
        public int characterCount;
        public float slideTime;
        public int slideCount;
    }
}