using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using System;

namespace hamsterbyte.GlobalVariables
{
	public class GlobalVariablesWindow : EditorWindow
	{

		#region VARIABLES
		private GUILayoutOption[] stretch = new GUILayoutOption[]{GUILayout.ExpandWidth (true)};

		private GUIStyle AdjustedLabel { get { return (!EditorGUIUtility.isProSkin) ? EditorStyles.whiteBoldLabel : EditorStyles.boldLabel; } }

		private Color Gray { get { return (!EditorGUIUtility.isProSkin) ? Color.white : Color.gray; } }

		public Color bgColor = Color.white;

		private Vector2 _scrollPos;
		private string _searchString;
		#endregion

		#region SHOW WINDOW
		[MenuItem ("Window/Global Variables")]
		public static void  ShowWindow ()
		{
			EditorWindow.GetWindow (typeof(GlobalVariablesWindow), false, "Globals", true);
		}
		#endregion


		void OnGUI ()
		{
			DrawHeader ();
			DrawVars ();
			this.Repaint ();
		}

		private void DrawHeader ()
		{
			GUI.backgroundColor = Color.gray;
			GUILayout.BeginHorizontal ("button", stretch);
			GUILayout.FlexibleSpace ();
			EditorGUILayout.LabelField ("Global Variables", AdjustedLabel, GUILayout.Width (125));
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
			DrawSearch();
			GUI.backgroundColor = Color.gray;
			GUILayout.BeginHorizontal ("textField", stretch);
			if (Application.isPlaying) {
				EditorGUILayout.LabelField ("Key", GUILayout.Width (position.width * .33f));
				EditorGUILayout.LabelField ("TypeOf", GUILayout.Width (position.width * .3f));
				EditorGUILayout.LabelField ("Value", GUILayout.Width (position.width * .3f));
			} else {
				GUILayout.FlexibleSpace ();
				EditorGUILayout.LabelField ("Variables only visible in play mode!", EditorStyles.wordWrappedLabel, stretch);
				GUILayout.FlexibleSpace ();
			}
			GUILayout.EndHorizontal ();
		}

		private void DrawVars ()
		{
			if (Application.isPlaying) {
				_scrollPos = GUILayout.BeginScrollView(_scrollPos);
				string[] intKeys = GlobalVars.genericVars.Keys.ToArray ();
				Array.Sort(intKeys);
				foreach (string s in intKeys) {
					string typeString;
					if(GlobalVars.genericVars [s] != null)
						typeString = GlobalVars.genericVars [s].GetType ().ToString ().Split('.')[1];
					else 
						typeString = "NULL";

					GUI.backgroundColor = bgColor;
					GUILayout.BeginHorizontal (EditorStyles.toolbar);
					GUILayout.FlexibleSpace();
					EditorGUILayout.LabelField (s, GUILayout.Width (position.width * .33f));
					EditorGUILayout.LabelField (typeString, GUILayout.Width (position.width * .3f));
					DrawValues (s);
					GUILayout.FlexibleSpace();
					GUILayout.EndHorizontal ();
					if (bgColor == Color.white)
						bgColor = Color.grey;
					else 
						bgColor = Color.white;
				}
				GUILayout.EndScrollView();
			}
		}

		private void DrawValues (string name)
		{
			if(GlobalVars.genericVars[name] != null)
				EditorGUILayout.LabelField (GlobalVars.genericVars[name].ToString(), GUILayout.Width (position.width * .3f));
			else
				EditorGUILayout.LabelField ("NULL", GUILayout.Width (position.width * .3f));
		}

		private void DrawSearch(){
			if(Application.isPlaying){
				GUI.backgroundColor = Color.white;
				GUILayout.BeginHorizontal (EditorStyles.toolbar, stretch);
				EditorGUILayout.LabelField ("Search" , GUILayout.Width (60));

				_searchString = EditorGUILayout.TextField(_searchString, EditorStyles.toolbarTextField, GUILayout.Width(position.width * .25f));
				if(_searchString != string.Empty){
					if(GlobalVars.genericVars.ContainsKey(_searchString)){
						GUILayout.BeginHorizontal (EditorStyles.toolbar, GUILayout.Width((position.width * .75f) - 60));
						GUI.SetNextControlName ("Clear");
						if(GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(60))){
							_searchString = string.Empty;
							GUI.FocusControl("Clear");
							return;
						}
						GUI.backgroundColor = Color.green;
						EditorGUILayout.LabelField(" ", EditorStyles.radioButton, GUILayout.Width(20));
						GUI.backgroundColor = Color.gray;
						EditorGUILayout.LabelField ("Value: " + GlobalVars.genericVars[_searchString].ToString() + "     Type: " +  GlobalVars.genericVars [_searchString].GetType ().ToString ().Split('.')[1], stretch);
						GUILayout.EndHorizontal ();
					}else{
						GUILayout.BeginHorizontal (EditorStyles.toolbar, GUILayout.Width((position.width * .75f) - 60));
						GUI.SetNextControlName ("Clear");
						if(GUILayout.Button("Clear", EditorStyles.toolbarButton, GUILayout.Width(60))){
							_searchString = string.Empty;
							GUI.FocusControl("Clear");
							return;
						}
						GUI.backgroundColor = Color.red;
						EditorGUILayout.LabelField(" ", EditorStyles.radioButton, GUILayout.Width(20));
						GUI.backgroundColor = Color.gray;
						EditorGUILayout.LabelField ("Key Not Found!!" , stretch);
						GUILayout.EndHorizontal ();
					}
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal ();
			}
		}
	}
}
