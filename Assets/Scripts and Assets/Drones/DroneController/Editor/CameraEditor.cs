using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using DroneController.CameraMovement;

[CustomEditor(typeof(DroneCamera))]
[ExecuteInEditMode]
public class CameraEditor : Editor
{
    public override void OnInspectorGUI()
    {

        var myScript = target as CameraScript;

        UpdateLayerInputs();

EditorGUILayout.BeginVertical("Box");
        myScript.inputEditorFPS = GUILayout.Toolbar(myScript.inputEditorFPS, new string[] { "Third Person View", "First Person View" });

        switch (myScript.inputEditorFPS)
        {
            case 0:
                myScript.FPS = false;
                break;

            case 1:
                myScript.FPS = true;
                break;
        }

        if (myScript.FPS)
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("First person view properties to position and rotate camera inside the drone.", MessageType.None);
           // EditorGUILayout.Space();
         //   myScript.FPS = EditorGUILayout.Toggle(new GUIContent("FPS (ON)"), myScript.FPS);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            myScript.positionInsideDrone = EditorGUILayout.Vector3Field(new GUIContent("Position inside drone", "This will move cameras local position inside drone."), myScript.positionInsideDrone);
            myScript.rotationInsideDrone = EditorGUILayout.Vector3Field(new GUIContent("Rotation inside drone", "This will give the camera extra X (up/down) tilt when in POV mode, so when you move forward your camera view is straight."), myScript.rotationInsideDrone);
            myScript.fpsFieldOfView = EditorGUILayout.FloatField(new GUIContent("Field Of View", "Camera field of view"), myScript.fpsFieldOfView);
        }
        else
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Third person view properties to position and rotate camera behind the drone.", MessageType.None);
           // EditorGUILayout.Space();
          //  myScript.FPS = EditorGUILayout.Toggle(new GUIContent("FPS (OFF)"), myScript.FPS);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            myScript.positionBehindDrone = EditorGUILayout.Vector3Field(new GUIContent("Position behind drone", "Position of the camera behind the drone."), myScript.positionBehindDrone);
            myScript.cameraFollowPositionTime = EditorGUILayout.Slider(new GUIContent("Camera follow position time", "How fast the camera will follow drone position. (The lower the value the faster it will follow)"), myScript.cameraFollowPositionTime, 0.0f, 0.1f);
            myScript.extraTilt = EditorGUILayout.FloatField(new GUIContent("Extra camera tilt when moving", "Value where if the camera/drone is moving upwards will raise the camera view upward to get a better look at what is above, same goes when going downwards."), myScript.extraTilt);
            myScript.tpsFieldOfView = EditorGUILayout.FloatField(new GUIContent("Field Of View", "Camera field of view"), myScript.tpsFieldOfView);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            myScript.freeMouseMovement = EditorGUILayout.Toggle(new GUIContent("Free Mouse Look", "Allows to freely view around the drone with your mouse and not depending on drone look rotation."), myScript.freeMouseMovement);
            EditorGUILayout.Space();
            if (myScript.freeMouseMovement)
            {
                myScript.mouseSensitvity = EditorGUILayout.FloatField(new GUIContent("Mouse Sensitivity", "Value that will determine how fast your free look mouse will behave."), myScript.mouseSensitvity);
                myScript.mouseFollowTime = EditorGUILayout.FloatField(new GUIContent("Mouse Follow Time", "Value that will follow the camera view behind the mouse movement.(The lower the value, the faster it will follow mouse movement)"), myScript.mouseFollowTime);
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        EditorGUILayout.EndVertical(); 

        DrawDefaultInspector();
    }

    string[] optionsFPS = null;
    string[] optionsTPS = null;
    private void UpdateLayerInputs()//adds layer properties to our custom insepctor, cool isnt it?
    {
        optionsFPS = new string[32];
        optionsTPS = new string[32];

        for (int i = 0; i <= 31; i++)
        {
            string layerName = LayerMask.LayerToName(i);
            optionsFPS[i] = layerName;
            optionsTPS[i] = layerName;
        }
    }

}