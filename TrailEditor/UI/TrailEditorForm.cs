/*
 * Copyright © 2011 Luca Pieracci Galante
 * 
 * This program is licensed under the Microsoft Public License (Ms-PL). You should
 * have received a copy of the license along with the source code. If not, an online copy
 * of the license can be found at http://medusa.codeplex.com/license.
 */

using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace TrailEditor
{
    public partial class TrailEditorForm : Form
    {
        XNAPanel xnaPanel;
        EditPanel editPanel;

        public TrailEditorForm()
        {
            InitializeComponent();

            xnaPanel = new XNAPanel();
            panel1.Controls.Add(xnaPanel);
            xnaPanel.Dock = DockStyle.Fill;
            xnaPanel.Show();

            editPanel = new EditPanel();
            splitContainer1.Panel2.Controls.Add(editPanel);
            editPanel.Dock = DockStyle.Fill;
            editPanel.Show();
        }

        public Panel XNAPanel
        {
            get { return panel1; }
        }

        public int PanelWidth
        {
            get { return panel1.Width; }
        }

        public int PanelHeight
        {
            get { return panel1.Height; }
        }

        public IntPtr PanelHandle
        {
            get { return xnaPanel.PanelHandle; }
        }


        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                // Default to the directory which contains our content files.
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;

                fileDialog.InitialDirectory = assemblyLocation;

                fileDialog.Title = "Save current settings";
                fileDialog.Filter = "XML Files (*.xml)|*.xml;";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    XmlWriterSettings hSettings = new XmlWriterSettings();
                    hSettings.Indent = true;

                    using (XmlWriter xmlWriter = XmlWriter.Create(fileDialog.FileName, hSettings))
                    {
                        IntermediateSerializer.Serialize(xmlWriter, TrailSettings.Instance.ToDataContent(), null);
                    }
                }
            }
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                // Default to the directory which contains our content files.
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;

                fileDialog.InitialDirectory = assemblyLocation;

                fileDialog.Title = "Load trail settings";
                fileDialog.Filter = "XML Files (*.xml)|*.xml;";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {

                    using (XmlReader xmlReader = XmlReader.Create(fileDialog.FileName))
                    {
                        TrailSettingsDataContent data =
                        IntermediateSerializer.Deserialize<TrailSettingsDataContent>(xmlReader, null);

                        TrailSettings.Instance.FromDataContent(data);

                        editPanel.PropertyGrid.Refresh();
                    }
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}