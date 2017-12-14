using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;//for Type class
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;


// Inherits from SaveLoadUtility so we can load and save Levels specifically the way we want
public class LevelEditorLoadSaveUtility : SaveLoadUtility
{
    //use this one for specifying a filename
    public void SaveGame(string path, string saveGameName)
    {
        if (string.IsNullOrEmpty(saveGameName))
        {
            Debug.Log("SaveGameName is null or empty!");
            return;
        }

        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("SaveGame path is null or empty!");
            return;
        }

        //Create a new instance of SaveGame. This will hold all the data that should be saved in our scene.
        SaveGame newSaveGame = new SaveGame();
        newSaveGame.savegameName = saveGameName;
        newSaveGame.saveDate = DateTime.Now.ToString();

        //Clear the SaveLoad.objectIdentifierDict
        SaveLoad.objectIdentifierDict = new Dictionary<string, ObjectIdentifier>();

        //Find all ObjectIdentifier components in the scene.
        //Since we can access the gameObject to which each one belongs with .gameObject, we thereby get all GameObject in the scene which should be saved!
        ObjectIdentifier[] OIsToSerialize = FindObjectsOfType(typeof(ObjectIdentifier)) as ObjectIdentifier[];
        //Go through the "raw" collection of components
        if (true)
        {
            foreach (ObjectIdentifier oi in OIsToSerialize)
            {
                //if the gameObject shouldn't be saved, for whatever reason (maybe it's a temporary ParticleSystem that will be destroyed anyway), ignore it
                if (oi.dontSave == true)
                {
                    if (debugController.oiIsSetToDontSave)
                    {
                        Debug.Log("GameObject " + oi.gameObject.name + " is set to dontSave = true, continuing loop.");
                    }
                    continue;
                }

                //First, we will set the ID of the GameObject if it doesn't already have one.
                if (string.IsNullOrEmpty(oi.id) == true)
                {
                    oi.SetID();
                }
                //then, we will add the OI to the SaveLoad.objectIdentifierDict with the id as key
                //this Dictionary isn't used directly but you may want to access it in your script's OnSave functions so you don't have to find all OIs in the scene again and again.
                if (SaveLoad.objectIdentifierDict.ContainsKey(oi.id) == false)
                {
                    SaveLoad.objectIdentifierDict.Add(oi.id, oi);
                }
            }
        }

        //Go through the OIsToSerialize array and for each GameObject, call the OnSave method on each (MonoBehavior) component (if one exsists).
        //This is a good time to call any functions on the GameObject that should be called before it gets saved and serialized as part of a SaveGame.
        if (true)
        {
            foreach (ObjectIdentifier oi in OIsToSerialize)
            {
                oi.gameObject.SendMessage("OnSave", SendMessageOptions.DontRequireReceiver);
            }
        }

        //Go through the OIsToSerialize array again to pack the GameObjects into serializable form, and add the packed data to the sceneObjects list of the new SaveGame instance.
        if (true)
        {
            foreach (ObjectIdentifier oi in OIsToSerialize)
            {
                //Convert the GameObject's data into a form that can be serialized (an instance of SceneObject),
                //and add it to the SaveGame instance's list of SceneObjects.
                newSaveGame.sceneObjects.Add(PackGameObject(oi.gameObject, oi));
            }
        }

        //Call the static method that serializes our SaveGame instance and writes the data to a file.
        SaveLoad.SaveScene(newSaveGame, path);
        if (debugController.gameSaved)
        {
            Debug.Log("Game Saved: " + newSaveGame.savegameName + " (" + newSaveGame.saveDate + ").");
        }
    }


    public override void LoadGame(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            Debug.Log("[LoadGame] " + "SaveGameName is null or empty!");
            return;
        }

        //Clear the refDict, which we will need later to reconnect GameObject and Component variables with their referenced objects
        refDict = new Dictionary<RefReconnecter, string>();

        //Call the static method that will attempt to load the specified file and deserialize it's data into a form that we can use
        SaveGame loadedGame = SaveLoad.LoadLevel(path);
        if (loadedGame == null)
        {
            Debug.Log("[LoadGame] " + "Level at " + path + " couldn't be found!");
            return;
        }

        //clear the SaveLoad.objectIdentifierDict which will hold all the ObjectIdentifiers whose GameObjects will be created anew from the deserialized data, with their IDs as the keys.
        //we need this dictionary later to reconnect parents with their children (or vice versa), 
        //and you might want to access it in your scripts' OnLoad function so you don't ahve to load all Ois again and again.
        SaveLoad.objectIdentifierDict = new Dictionary<string, ObjectIdentifier>();

        //iterate through the loaded SaveGame's sceneObjects list to access each stored object's data and reconstruct/unpack it with all it's components
        //Simultaneously, add the loaded GameObject's ObjectIdentifier to the SaveLoad.objectIdentifierDict.
        foreach (SceneObject loadedObject in loadedGame.sceneObjects)
        {
            GameObject go = UnpackGameObject(loadedObject, null);

            if (go != null)
            {
                //Add the reconstructed GameObject to the list we created earlier.
                ObjectIdentifier oi = go.GetComponent<ObjectIdentifier>();
                SaveLoad.objectIdentifierDict.Add(oi.id, oi);
            }
        }

        //Go through the dictionary of reconstructed GameObjects and try to reassign any missing children, and reset the localPosition
        if (true)
        {
            foreach (KeyValuePair<string, ObjectIdentifier> pair in SaveLoad.objectIdentifierDict)
            {
                ObjectIdentifier oi = pair.Value;
                if (string.IsNullOrEmpty(oi.idParent) == false)
                {
                    if (SaveLoad.objectIdentifierDict.ContainsKey(oi.idParent))
                    {
                        Vector3 pos = oi.transform.position;
                        oi.transform.parent = SaveLoad.objectIdentifierDict[oi.idParent].transform;
                        oi.transform.localPosition = pos;
                    }
                }
            }
        }

        //Now comes a quite ugly part. It will only come into play if you have loaded any members (field or properties) that held references to GameObjects or Components.
        //Basically, when we went through each member (field/property) and it was a GameObject or Component reference, 
        //this field along with the referenced object's id (and some other information) is added to refDict.
        //We now go through each key of refDict and try to find to referenced object (GameObject or Component) so we can add that value to the field or property.
        if (true)
        {
            foreach (KeyValuePair<RefReconnecter, string> pair in refDict)
            {
                RefReconnecter refRec = pair.Key;
                object valueToSet = new object();
                Type fieldType = refRec.field.FieldType;

                if (debugController.currentRefRec)
                {
                    Debug.Log("[LoadGame] " + "Current RefReconnecter: " + refRec.baseInstance.GetType().Name + "/" + refRec.field.Name + " (" + fieldType + ")");
                }

                if (refRec.collectionType == CollectionType.None)
                {
                    ObjectIdentifier oi = SaveLoad.objectIdentifierDict[pair.Value];

                    if (fieldType == typeof(GameObject))
                    {
                        valueToSet = oi.gameObject;
                    }
                    else
                    {
                        Component component = oi.GetComponent(fieldType.Name.ToString());
                        if (component != null)
                        {
                            valueToSet = component;
                        }
                    }
                }
                else
                {
                    Type elementType = TypeSystem.GetElementType(fieldType);
                    Dictionary<string, object> fieldValueDict = refRec.loadedValue as Dictionary<string, object>;

                    if (refRec.collectionType == CollectionType.Array)
                    {
                        Array array = Array.CreateInstance(elementType, fieldValueDict.Count);
                        foreach (KeyValuePair<string, object> pair_fvd in fieldValueDict)
                        {
                            if (pair_fvd.Value == null)
                            {
                                continue;
                            }
                            if (SaveLoad.objectIdentifierDict.ContainsKey(pair_fvd.Value.ToString()))
                            {
                                ObjectIdentifier oi = SaveLoad.objectIdentifierDict[pair_fvd.Value.ToString()];
                                if (elementType == typeof(GameObject))
                                {
                                    array.SetValue(oi.gameObject, Int32.Parse(pair_fvd.Key));
                                }
                                else
                                {
                                    array.SetValue(oi.GetComponent(elementType), Int32.Parse(pair_fvd.Key));
                                }
                            }
                        }
                        valueToSet = array;
                    }
                    else if (refRec.collectionType == CollectionType.List)
                    {
                        object list = Activator.CreateInstance(fieldType);
                        MethodInfo listAddMethod = fieldType.GetMethod("Add");

                        foreach (KeyValuePair<string, object> pair_fvd in fieldValueDict)
                        {
                            if (pair_fvd.Value == null)
                            {
                                listAddMethod.Invoke(list, new object[] { null });
                                continue;
                            }
                            if (SaveLoad.objectIdentifierDict.ContainsKey(pair_fvd.Value.ToString()))
                            {
                                ObjectIdentifier oi = SaveLoad.objectIdentifierDict[pair_fvd.Value.ToString()];
                                if (elementType == typeof(GameObject))
                                {
                                    listAddMethod.Invoke(list, new object[] { oi.gameObject });
                                }
                                else
                                {
                                    listAddMethod.Invoke(list, new object[] { oi.GetComponent(elementType) });
                                }
                            }
                        }

                        valueToSet = list;
                    }
                    else if (refRec.collectionType == CollectionType.Dictionary)
                    {

                        Type keyType = fieldType.GetGenericArguments()[0];
                        Type valueType = fieldType.GetGenericArguments()[1];

                        bool inheritsFromComponent_keyType = SaveLoad.InheritsFrom(keyType, typeof(Component));
                        bool inheritsFromComponent_valueType = SaveLoad.InheritsFrom(valueType, typeof(Component));

                        object dictionary = Activator.CreateInstance(fieldType);
                        MethodInfo dictionaryAddMethod = fieldType.GetMethod("Add", new[] { keyType, valueType });

                        ConvertedDictionary convDict = refRec.loadedValue as ConvertedDictionary;

                        for (int i = 0; i < convDict.keys.Count; i++)
                        {

                            //var newKey = Activator.CreateInstance(keyType);//Can't be used since String has no such initializer
                            object newKey = new object();

                            if (keyType.Namespace == "System" || surrogateTypes.Contains(keyType.Name))
                            {
                                newKey = convDict.keys[i.ToString()];
                            }
                            else if (keyType == typeof(GameObject) || inheritsFromComponent_keyType == true)
                            {
                                string refID = convDict.keys[i.ToString()].ToString();
                                if (SaveLoad.objectIdentifierDict.ContainsKey(refID))
                                {
                                    ObjectIdentifier oi = SaveLoad.objectIdentifierDict[refID];
                                    if (keyType == typeof(GameObject))
                                    {
                                        newKey = oi.gameObject;
                                    }
                                    else
                                    {
                                        if (oi.GetComponent(keyType) != null)
                                        {
                                            newKey = oi.GetComponent(keyType.Name);
                                        }
                                        else
                                        {
                                            Debug.Log("[LoadGame] " + "oi.GetComponent(" + keyType + ") == null");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Dictionary<string, object> keyDict = convDict.keys[i.ToString()] as Dictionary<string, object>;
                                SetValues(ref newKey, keyDict);
                            }

                            //var newValue = Activator.CreateInstance(valueType);
                            object newValue = new object();

                            if (valueType.Namespace == "System" || surrogateTypes.Contains(valueType.Name))
                            {
                                newValue = convDict.values[i.ToString()];
                            }
                            else if (valueType == typeof(GameObject) || inheritsFromComponent_valueType == true)
                            {
                                string refID = convDict.values[i.ToString()].ToString();
                                if (SaveLoad.objectIdentifierDict.ContainsKey(refID))
                                {
                                    ObjectIdentifier oi = SaveLoad.objectIdentifierDict[refID];
                                    if (valueType == typeof(GameObject))
                                    {
                                        newValue = oi.gameObject;
                                    }
                                    else
                                    {
                                        if (oi.GetComponent(valueType.Name) != null)
                                        {
                                            newValue = oi.GetComponent(valueType);
                                        }
                                        else
                                        {
                                            Debug.Log("[LoadGame] " + "oi.GetComponent(" + valueType + ") == null");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Dictionary<string, object> valueDict = convDict.values[i.ToString()] as Dictionary<string, object>;
                                SetValues(ref newValue, valueDict);
                            }

                            dictionaryAddMethod.Invoke(dictionary, new object[] { newKey, newValue });
                        }
                        valueToSet = dictionary;
                    }
                }
                if (refRec.field != null)
                {
                    refRec.field.SetValue(refRec.baseInstance, valueToSet);
                }
                else if (refRec.property != null)
                {
                    refRec.property.SetValue(refRec.baseInstance, valueToSet, null);
                }
            }
        }

        //This is when you might want to call any functions that should be called when a gameobject is loaded.
        //Remember that you can access the static SaveLoad.objectIdentifierDict from anywhere to access all the ObjectIdentifiers that were reconstructed after loading the game.
        if (true)
        {
            foreach (KeyValuePair<string, ObjectIdentifier> pair in SaveLoad.objectIdentifierDict)
            {
                pair.Value.gameObject.SendMessage("OnLoad", SendMessageOptions.DontRequireReceiver);
            }
        }

        if (debugController.gameLoaded)
        {
            Debug.Log("Game Loaded: " + loadedGame.savegameName + " (" + loadedGame.saveDate + ").");
        }
    }
}
