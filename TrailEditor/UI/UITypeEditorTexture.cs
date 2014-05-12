/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TrailEditor
{
    public class UITypeEditorTexture : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context,
                                            IServiceProvider provider,
                                            object value)
        {
            IWindowsFormsEditorService editorService = null;

            if (provider != null)
            {
                editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            }

            if (editorService != null)
            {
                using (OpenFileDialog hOpenFile = new OpenFileDialog())
                {
                    hOpenFile.Title = "Load image...";
                    hOpenFile.Multiselect = false;
                    hOpenFile.AddExtension = true;
                    hOpenFile.CheckFileExists = true;
                    hOpenFile.Filter = "Image Formats *.png;*.jpg;|*.png;*.jpg;";
                    hOpenFile.ValidateNames = true;

                    if (hOpenFile.ShowDialog() == DialogResult.OK)
                    {
                        value = hOpenFile.FileName;
                    }
                }
            }

            return value;
        }
    }
}
