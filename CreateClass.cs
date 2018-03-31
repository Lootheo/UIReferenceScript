
//Put this in Assets/Editor Folder
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Reflection;

public class CreateClass
{

    //Since I can't find a way to edit configuration options, Ill put them here:
    public static bool PublicVariables = true;
    public static bool AddUpdateUI = true;
    public static bool IncludeCanvasScaler = false;
    public static bool CreateUpdateUIFunction = true;


    [MenuItem("Assets/Create/Create C# Child References")]
    static void Create()
    {
        var selected = Selection.activeGameObject;
        if (selected == null)
        {
            Debug.Log("Selected object not Valid");
            return;
        }
        Debug.Log("Creating Class: " + selected.name);
        CreateFileWithChildUIReferences(selected.name, selected);
    }

    static void CreateFileWithChildUIReferences(string activeObjectName, GameObject selectedGO)
    {
        // remove whitespace and minus
        string name = activeObjectName.Replace(" ", "_");
        name = name.Replace("-", "_");
        string copyPath = "Assets/" + name + ".cs";
        Debug.Log("Creating Classfile: " + copyPath);
        if (File.Exists(copyPath) == false)
        { // do not overwrite
            using (StreamWriter outfile =
                new StreamWriter(copyPath))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("using UnityEngine.UI;");
                outfile.WriteLine("using System.Collections;");
                outfile.WriteLine("");
                outfile.WriteLine("public class " + name + " : MonoBehaviour {");
                outfile.WriteLine(" ");
                outfile.WriteLine(" ");
                outfile.WriteLine(ComponentListToString(GetChildUIElements(selectedGO)));
                outfile.WriteLine(" ");
                outfile.WriteLine(" ");
                outfile.WriteLine(" // Use this for initialization");
                outfile.WriteLine(" void Start () {");
                outfile.WriteLine(" ");
                outfile.WriteLine(" }");
                outfile.WriteLine(" ");
                outfile.WriteLine(" ");
                outfile.WriteLine(" // Update is called once per frame");
                outfile.WriteLine(" void Update () {");
                outfile.WriteLine(" ");
                outfile.WriteLine(" }");
                outfile.WriteLine("}");
            }//File written
        }
        else
        {
            Debug.LogError("File already exists, please remove it or delete it to avoid problems");
        }
        AssetDatabase.Refresh();
        Debug.Log("Class: " + name + " created");
    }

    static string ComponentListToString(List<Component> Components)
    {
        string componentVariableText = "";
        foreach (Component component in Components)
        {
            string componentNameReference = "";
            if (PublicVariables) componentNameReference = "public ";
            componentNameReference += (component.GetType().ToString().Replace("UnityEngine.UI.", "") + " " + component.GetType().ToString().Replace("UnityEngine.UI.", "").ToLower() + FirstCharToUpper(component.gameObject.name));
            componentNameReference += ";";
            componentVariableText += componentNameReference + "\n";
        }
        return componentVariableText;
    }


    public static string CreateUpdateUIString(List<Component> Components)
    {
        string componentVariableText = "public void UpdateUI(){";
        foreach (Component component in Components)
        {
            string componentNameReference = "";
            componentNameReference += (component.GetType().ToString().Replace("UnityEngine.UI.", "").ToLower() + FirstCharToUpper(component.gameObject.name));
            componentNameReference += ";";
            componentVariableText += componentNameReference + "\n";
        }
        componentVariableText += "\n }";
        return componentVariableText;
    }

     static List<Component> GetChildUIElements(GameObject selectedGO)
    {
        List<Component> ChildUIComponents = new List<Component>();
        foreach (Transform child in selectedGO.GetComponentsInChildren<Transform>())
        {
            if (GetUIComponentType(child.gameObject) != null)
            {
                ChildUIComponents.Add(GetUIComponentType(child.gameObject));
            }
        }
        return ChildUIComponents;
    }

    //For example: Text, Image, etc
    static Component GetUIComponentType(GameObject currentGameObject)
    {
        foreach (Component component in currentGameObject.GetComponents(typeof(Component)))
        {
            if (component.GetType().ToString().Contains("UnityEngine.UI"))
            {
                if (!IncludeCanvasScaler && component.GetType() == typeof(UnityEngine.UI.CanvasScaler)) { return null; }
                return component;
            }
        }
        Debug.Log("No UI Element Found in " + currentGameObject.name);
        return null;
    }

    public static string FirstCharToUpper(string input)
    {
        if (String.IsNullOrEmpty(input))
            throw new ArgumentException("string is null or empty!");
        return input.ToString().ToUpper().Substring(0, 1) + input.Substring(1);
    }

}