using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "KeybindingProfile", menuName = "Keybinding Profile", order = 1)]
public class Keybindings : ScriptableObject {
    public List<Keybinding> keybinding = new List<Keybinding>();

}
[Serializable] public class Keybinding
{
    public enum ActionMapping { left, right, jump, duck, action, switchF, switchB, exit}
    public string actionName;
    public ActionMapping actionMap;
    public KeyCode keyValue;
}
