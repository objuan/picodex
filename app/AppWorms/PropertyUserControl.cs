using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppWorms
{
    public partial class PropertyUserControl : UserControl
    {
        UnityEngine.Component component;

        public PropertyUserControl(UnityEngine.Component component)
        {
            InitializeComponent();
            this.component = component;

            propertyGrid.SelectedObject = component;


        }

    }
}
