﻿using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace AutoSearchDirectory
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    string directoryPath = folderBrowserDialog.SelectedPath;
                    string searchWord = Seach_Word.Text;

                    string[] files = Directory.GetFiles(directoryPath, "*.txt", SearchOption.AllDirectories);
                    resultListBox.Items.Clear();

                    if (files.Length > 0)
                    {
                        foreach (string filePath in files)
                        {
                            string[] lines = File.ReadAllLines(filePath);
                            for (int i = 0; i < lines.Length; i++)
                            {
                                string line = lines[i];
                                if (line.Contains(searchWord))
                                {
                                    resultListBox.Items.Add($"{filePath}:{line}");
                                }
                            }
                        }
                    }
                    else
                    {
                        resultListBox.Items.Add("No files found.");
                    }
                }
            }
        }

        private void DIR_BUTTON_Click(object sender, EventArgs e)
        {
            if (resultListBox.SelectedItem != null)
            {
                string selectedLine = resultListBox.SelectedItem.ToString();
                int lastColonIndex = selectedLine.LastIndexOf(':');
                if (lastColonIndex != -1)
                {
                    string selectedValue = selectedLine.Substring(0, lastColonIndex);
                    string selectedFolderPath = Path.GetDirectoryName(selectedValue);

                    if (Directory.Exists(selectedFolderPath))
                    {
                        Process.Start("explorer.exe", selectedFolderPath);
                    }
                }
            }
        }

        private void searchWordTextBox_TextChanged(object sender, EventArgs e)
        {
            // This event handler is empty, no changes needed here.
        }

        private void FileOpenButton_Click(object sender, EventArgs e)
        {
            if (resultListBox.SelectedItem != null)
            {
                string selectedLine = resultListBox.SelectedItem.ToString();
                int lastColonIndex = selectedLine.LastIndexOf(':');
                if (lastColonIndex != -1)
                {
                    string selectedValue = selectedLine.Substring(0, lastColonIndex);
                    if (File.Exists(selectedValue))
                    {
                        Process.Start(selectedValue);
                    }
                }
            }
        }

        private void SavefileButton_Click(object sender, EventArgs e)
        {
            if (resultListBox.Items.Count > 0)
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt";
                    saveFileDialog.FileName = "ListContents.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string destinationFilePath = saveFileDialog.FileName;

                        using (StreamWriter writer = new StreamWriter(destinationFilePath))
                        {
                            foreach (var item in resultListBox.Items)
                            {
                                string line = item.ToString();
                                int lastColonIndex = line.LastIndexOf(':');
                                if (lastColonIndex != -1)
                                {
                                    string value = line.Substring(0, lastColonIndex);
                                    writer.WriteLine(value);
                                }
                            }
                        }

                        MessageBox.Show("List contents saved successfully!");
                    }
                }
            }
            else
            {
                MessageBox.Show("No items in the list to save.");
            }
        }
    }
}