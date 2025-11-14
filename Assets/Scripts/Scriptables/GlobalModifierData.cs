using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GlobalModifierData", menuName = "ScriptableObjects/GlobalModifierData", order = 5)]
public class GlobalModifierData : ScriptableObject
{
    public string Name => _name;
    public string Description => _description;
    public Sprite Icon => _icon;
    public float Duration => _duration;
    public bool IsPerModule => _isPerModule;
    public List<ModuleTypes> AffectedModules => _affectedModules;

    public Modifiers Modifiers => _modifiers;
    [SerializeField] private string _name;
    [SerializeField] private string _description;
    [SerializeField] private Sprite _icon;
    [SerializeField] private Modifiers _modifiers;
    [SerializeField] private bool _isOneTime;
    [SerializeField] private float _duration;
    [SerializeField] private bool _isPerModule;
    [SerializeField] private List<ModuleTypes> _affectedModules;

    public void SetDuration(float duration)
    {
        _duration = duration;
    }
}
