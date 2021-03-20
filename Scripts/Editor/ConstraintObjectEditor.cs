using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace IxC.TransformConstraints
{
    [CustomEditor(typeof(ConstraintObject))]
    public class ConstraintObjectEditor : Editor
    {
        public ConstraintObject behaviour;

        private void OnEnable()
        {
            behaviour = target as ConstraintObject;
        }

        public override void OnInspectorGUI()
        {
            if (behaviour.constraints == null) behaviour.constraints = new List<ConstraintSolver>(2);

            if (GUILayout.Button(new GUIContent(
                "Force setup update in all constraints",
                "If the constraints are behaving funky when disabling them or changing their influence,\n" +
                "that's a known but not yet fully understood issue with the way I'm keeping track of the\n" +
                "transform's original values, soon enough I'll update the package to fix this, likely gonna\n" +
                "do some changes to the architecture of the ConstraintSolver class, but for now if there's\n" +
                "any issue with this, you can press this button when your transform matches the desired original\n" +
                "values when all constraints are hidden, any help with this issue will be greatly appreciated.")))
                    behaviour.Awake();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(behaviour.constraints.Count.ToString(), new GUIStyle(
                GUI.skin.label) { alignment = TextAnchor.MiddleRight });
            if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
            {
                behaviour.constraints[behaviour.constraints.Count - 1].Cancel(behaviour.transform);
                behaviour.constraints.RemoveAt(behaviour.constraints.Count - 1);
            }
            if(GUILayout.Button("+", GUILayout.ExpandWidth(false)))
            {
                var types = Assembly.GetAssembly(typeof(ConstraintSolver)).
                    GetTypes().Where(
                    myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ConstraintSolver))).ToArray();
                GenericMenu menu = new GenericMenu();
                foreach (var type in types)
                {
                    var item = (ConstraintSolver)
                      Activator.CreateInstance(type, behaviour.transform);
                    menu.AddItem(new GUIContent(item.MenuPath()), false, () =>
                    {
                        behaviour.constraints.Add(item);
                        if (behaviour.constraints.Count > 1)
                            item.Setup(
                                behaviour.constraints[behaviour.constraints.Count - 2].pos,
                                behaviour.constraints[behaviour.constraints.Count - 2].scale,
                                behaviour.constraints[behaviour.constraints.Count - 2].rot);
                    });
                }
                menu.ShowAsContext();

            }

            EditorGUILayout.EndHorizontal();


            EditorGUI.indentLevel++;
            foreach(var constraint in behaviour.constraints)
            {
                constraint.OnInspectorGUI();
            }
        }
    }
}
