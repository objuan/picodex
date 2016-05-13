using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using UnityEngine;
using UnityEditor;

namespace AppWorms
{
    public partial class ProjectUserControl : UserControl
    {
        public event EventHandler OnActiveGameObjectChanged;

        public ProjectUserControl()
        {
            InitializeComponent();
        }

        public void UpdateUnity()
        {
           // TreeNode root = treeView.Nodes.Add("Scene");

            foreach (GameObject go in UnityEngine.SceneManagement.Scene.current.GetRootGameObjects())
            {
                Add(go, null);
            }
            treeView.ExpandAll();
        }

        private void Add(GameObject go, TreeNode parent)
        {
            TreeNode node;
            if (parent == null)
                node = treeView.Nodes.Add("Scene");
            else
                node = parent.Nodes.Add(go.name);

            node.Tag = go;

            int c = go.transform.childCount;
            for (int i = 0; i < c; i++)
            {
                Add(go.transform.GetChild(i).gameObject, node);
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Selection.activeGameObject = e.Node.Tag as GameObject;
            if (OnActiveGameObjectChanged!=null) OnActiveGameObjectChanged(this,EventArgs.Empty);

        }
    }
}
