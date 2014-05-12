/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using System;
using System.Windows.Forms;

namespace TrailEditor
{
    public partial class EditPanel : UserControl
    {
        public PropertyGrid PropertyGrid
        {
            get
            {
                return propertyGrid1;
            }
        }

        public EditPanel()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = TrailSettings.Instance;
        }

        private void EditPanel_Load(object sender, EventArgs e)
        {

        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            TrailSettings.Instance.Changed();
        }
    }
}