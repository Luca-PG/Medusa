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
    public partial class XNAPanel : UserControl
    {
        public XNAPanel()
        {
            InitializeComponent();
        }

        public IntPtr PanelHandle
        {
            get
            {
                return this.IsHandleCreated ? this.Handle : IntPtr.Zero;
            }
        }
    }
}
