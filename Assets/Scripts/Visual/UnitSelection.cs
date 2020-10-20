using Game.Services;
using Game.Settings;
using Units.Enums;
using UnityEngine;

namespace Game
{
    public class UnitSelection : MonoBehaviour
    {
        [SerializeField] private bool _isEnabledAtStart;
        [SerializeField] private SpriteRenderer _spriteRend;
        public PlayerOwner CurrentOwner { get; private set; }
        private PlayerColorSettings _colorSettings;
        
        private void Awake()
        {
            if (GameServices.Instance)
            {
                _colorSettings = GameServices.Instance.GetService<PlayerColorSettings>();
            }

            if (_spriteRend != null)
            {
                _spriteRend.gameObject.SetActive(_isEnabledAtStart);
            }
        }

        //TEST
        [ContextMenu("Select and SetOwner to Player1")]
        public void SetOwnerToPlayerOne()
        {
            SetOwner(PlayerOwner.PLAYER_1);
            SetActiveSelect(true);
        }

        public void SetOwner(PlayerOwner owner)
        {
            CurrentOwner = owner;
        }
        public void SetActiveSelect(bool isSelected)
        {
            if (_spriteRend == null)
                return;

            _spriteRend.gameObject.SetActive(isSelected);
            if (isSelected)
            {
                if (_colorSettings != null)
                {
                    _spriteRend.color = _colorSettings.GetColorByOwner(CurrentOwner);
                }
            }
            else
            {
                
            }
        }
    } 
}

