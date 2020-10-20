using Game.Services.Base;
using Units.Enums;
using UnityEngine;

namespace Game.Settings
{
    [System.Serializable]
    public struct ColorByPlayer
    {
        [SerializeField] private PlayerOwner _playerOwner;
        public PlayerOwner PlayerOwner => _playerOwner;
        
        [SerializeField] private Color _color;
        public Color Color => _color;
    }
    
    [CreateAssetMenu(fileName = "PlayerColorSettings", menuName = "Settings/PlayerColorSettings", order = 0)]
    public class PlayerColorSettings : GameServiceScriptableObject
    {
        [SerializeField] private ColorByPlayer[] _colorsCollection;

        public Color GetColorByOwner(PlayerOwner owner)
        {
            for (int i = 0; i < _colorsCollection.Length; i++)
            {
                var colordata = _colorsCollection[i];
                if (colordata.PlayerOwner == owner)
                {
                    return colordata.Color;
                }
            }
            Debug.LogError($"There is no colors for owner {owner}!");
            return Color.black;
        }
    }
}