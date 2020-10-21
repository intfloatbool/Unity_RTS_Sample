using System.Collections.Generic;
using Units;
using Units.Controllers;
using Units.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class PlayerBattleControl : MonoBehaviour
    {
        [SerializeField] private List<UnitAiController> _playerUnits;

        [Space]
        [SerializeField] private PlayerOwner _playerOwner;
        public PlayerOwner PlayerOwner
        {
            get => _playerOwner;
            set => _playerOwner = value;
        }
        
        [SerializeField] private RectTransform _selectRect;
        [SerializeField] private Camera _camera;
        [SerializeField] private CanvasScaler _canvasScaler;
        
        private Dictionary<GameUnit, UnitSelection> _selectionDict = new Dictionary<GameUnit, UnitSelection>();
        
        private HashSet<UnitAiController> _selectedUnitsHashSet = new HashSet<UnitAiController>();
        
        private Vector2 _guiStartDragPoint;
        private float _multiplerWidth = 1f;
        private float _multiplerHeight = 1f;
        private void Start()
        {
            if (_canvasScaler != null)
            {
                float perfectWidth = _canvasScaler.referenceResolution.x;
                float perfectHeight = _canvasScaler.referenceResolution.y;

                float currentWidth = Screen.width;
                float currentHeight = Screen.height;

                _multiplerWidth = perfectWidth / currentWidth;
                _multiplerHeight = perfectHeight / currentHeight;
            }
        }

        private void OnValidate()
        {
            if (_camera == null)
            {
                _camera = Camera.main;
                if (_camera == null)
                {
                    _camera = FindObjectOfType<Camera>();
                }
            }
        }
        

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DeselectAll();
                _guiStartDragPoint = GetMousePos();
            }
            
            if (Input.GetMouseButton(0))
            {
                ShowRectGui();
            }

            if (Input.GetMouseButtonUp(0))
            {
                ClearRectGui();
            }

            if (Input.GetMouseButtonDown(1))
            {
                RaycastHit hit;
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        
                if (Physics.Raycast(ray, out hit))
                {
                    var raycastedUnit = hit.collider.GetComponent<GameUnit>();
                    
                    foreach (var selectedUnit in _selectedUnitsHashSet)
                    {
                        selectedUnit.SetTarget(null);
                        selectedUnit.IsAutoDetectTarget = false;
                        selectedUnit.SetTarget(hit.point);
                        
                        if (raycastedUnit != null && raycastedUnit.Owner != _playerOwner)
                        {
                            selectedUnit.IsAutoDetectTarget = true;
                            selectedUnit.SetTarget(raycastedUnit);
                        }
                    }
                }
            }
        }

        private void TryOrderToSelected()
        {
            
        }

        private Vector2 GetMousePos()
        {
            Vector2 mousePos = Input.mousePosition;
            return new Vector2(mousePos.x * _multiplerWidth, mousePos.y * _multiplerHeight);
        }

        private void ClearRectGui()
        {
            if (_selectRect == null)
                return;
            
            if (_selectRect.gameObject.activeInHierarchy)
            {
                _selectRect.gameObject.SetActive(false);
            }

            Vector2 min = _selectRect.anchoredPosition - (_selectRect.sizeDelta / 2);
            Vector2 max = _selectRect.anchoredPosition + (_selectRect.sizeDelta / 2);
            
            TrySelectUnitsInside(min, max);
        }

        private void ShowRectGui()
        {
            if (_selectRect == null)
                return;

            if (!_selectRect.gameObject.activeInHierarchy)
            {
                _selectRect.gameObject.SetActive(true);
            }


            Vector3 mousePos = GetMousePos();

            float width = mousePos.x - _guiStartDragPoint.x;
            float height = mousePos.y - _guiStartDragPoint.y;
            
            
            _selectRect.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
            _selectRect.anchoredPosition = _guiStartDragPoint + new Vector2(width / 2, height / 2);
        }

        private void TrySelectUnitsInside(Vector2 min, Vector2 max)
        {
            foreach (var unit in _playerUnits)
            {
                if(unit == null || !unit.gameObject.activeInHierarchy)
                    continue;
                
                Vector3 screenUnitPos = _camera.WorldToScreenPoint(unit.transform.position);

                screenUnitPos.y *= _multiplerHeight;
                screenUnitPos.x *= _multiplerWidth;
                
                bool isUnitInsideRect = screenUnitPos.x > min.x && screenUnitPos.x < max.x && screenUnitPos.y >
                    min.y && screenUnitPos.y < max.y;
                if (isUnitInsideRect)
                {
                    Debug.Log("Select unit: " + unit.name);
                    SelectUnit(unit);
                }
            }
        }

        private void DeselectAll()
        {
            _playerUnits.RemoveAll(pu => pu == null || !pu.gameObject.activeInHierarchy);
            
            _selectedUnitsHashSet.Clear();
            foreach (var aiUnit in _playerUnits)
            {
                if (aiUnit != null)
                {
                    if (_selectionDict.TryGetValue(aiUnit.GameUnit, out var selection))
                    {
                        if (selection != null)
                        {
                            selection.SetActiveSelect(false);
                        }
                    }
                }
            }
        }

        private void SelectUnit(UnitAiController unitAi)
        {
            UnitSelection selection;
            _selectionDict.TryGetValue(unitAi.GameUnit, out selection);
            if (selection == null)
            {
                selection = unitAi.GetComponentInChildren<UnitSelection>();
                if (selection != null)
                {
                    _selectionDict.Add(unitAi.GameUnit, selection);
                }
                
            }
            if (selection != null)
            {
                selection.SetOwner(this._playerOwner);
                selection.SetActiveSelect(true);
                _selectedUnitsHashSet.Add(unitAi);
            }
            
        }
    }
}

