using UnityEngine;
using UnityEditor;

public class ShowSize : EditorWindow
{
    private Vector3 m_SelectionSize;
    private Vector3 m_SelectionScale;
    private bool m_HasSelection = false;

    [MenuItem("Window/ShowSize")]
	static void Init()
    {
	    ShowSize window = new ShowSize();
        window.autoRepaintOnSceneChange = true;
        window.Show();
    }

    void Update()
    {
        bool hasSelection = false;
        GameObject selection = Selection.activeGameObject as GameObject;
        if (selection)
        {
            MeshFilter meshFilter = selection.GetComponent<MeshFilter>();
            if (meshFilter)
            {
                Mesh mesh = meshFilter.sharedMesh;
                if (mesh)
                {
                    hasSelection = true;

                    Vector3 size = mesh.bounds.size;
                    Vector3 scale = selection.transform.localScale;
                    if ((m_SelectionSize != size) || (m_SelectionScale != scale))
                    {
                        m_SelectionSize = size;
                        m_SelectionScale = scale;
                        Repaint();
                    }
                }
            }
        }
        if (hasSelection != m_HasSelection)
        {
            m_HasSelection = hasSelection;
            Repaint();
        }
    }

	void OnGUI()
	{
	    if (m_HasSelection)
	    {
            GUILayout.Label("Size" +
                "\nX: " + m_SelectionSize.x + m_SelectionScale.x +
                "\nY: " + m_SelectionSize.y * m_SelectionScale.y +
                "\nZ: " + m_SelectionSize.z * m_SelectionScale.z);
        }
	    else
	    {
            GUILayout.Label("");
        }
	}
}
