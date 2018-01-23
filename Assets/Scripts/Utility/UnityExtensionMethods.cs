using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class UnityExtensionMethods
{
    // Get the names of all the children of a Transform
    public static string[] GetChildNames(this Transform trans)
    {
        string[] names = new string[trans.childCount];
        for (int i = 0; i < trans.childCount; i++)
        {
            names[i] = trans.GetChild(i).name;
        }

        return names;
    }

    // Get the amount of active children from a Transform
    public static int GetActiveChildCount(this Transform trans)
    {
        int count = 0;
        foreach(Transform child in trans)
        {
            count += (child.gameObject.activeSelf) ? 1 : 0;
        }

        return count;
    }

    // Gets all children and sub-children
    public static List<Transform> GetAllChildren(this Transform trans)
    {
        List<Transform> children = new List<Transform>();
        
        foreach(Transform child in trans)
        {
            children.Add(child);
            children.AddRange(child.GetAllChildren());
        }

        return children;
    }
}
