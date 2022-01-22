using System;
using System.Windows;
using Microsoft.Win32;
using System.ComponentModel;
using System.Collections.Generic;
using AssemblyBrowserView.Commands;
using System.Runtime.CompilerServices;
using AssemblyBrowserLibrary;
using AssemblyBrowserLibrary.Model;

namespace AssemblyBrowserView.ViewModel
{
    /// <summary>
    ///     Proccessing changes
    /// </summary>
    class AssemblyViewModel : INotifyPropertyChanged
    {
        /// <summary>
        ///     Result info
        /// </summary>
        public List<NamespaceWrapper> NamespaceWrapper { get; private set; }

        /// <summary>
        ///     Results container
        /// </summary>
        public AssemblyBrowser AssemblyBrowser { get; }

        /// <summary>
        ///     Open file dialog command
        /// </summary>
        private AssemblyCommands PrivateOpenAssemblyCommand;

        /// <summary>
        ///     Open file dialog command (realization)
        /// </summary>
        public AssemblyCommands OpenAssemblyCommand
        {
            get
            {
                return PrivateOpenAssemblyCommand ??
                    (PrivateOpenAssemblyCommand = new AssemblyCommands(obj =>
                    {
                        try
                        {
                            OpenFileDialog openFileDialog = new OpenFileDialog();

                            // Get assembly info
                            if (openFileDialog.ShowDialog() == true)
                            {
                                NamespaceWrapper = AssemblyBrowser.GetAssemblyData(openFileDialog.FileName);
                                OnPropertyChanged(nameof(NamespaceWrapper));
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }));
            }
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public AssemblyViewModel()
        {
            AssemblyBrowser = new AssemblyBrowser();
            NamespaceWrapper = new List<NamespaceWrapper>();
        }

        /// <summary>
        ///     Call when property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Action on property changes
        /// </summary>
        /// <param name="property">Property info</param>
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
