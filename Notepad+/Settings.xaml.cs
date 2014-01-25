using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using NotepadPlus.Notes;

namespace NotepadPlus
{
    public partial class Settings : PhoneApplicationPage
    {
        const string noteSortBy = "note_SortBy";

        private IsolatedStorageSettings settings { get; set; }

        public Settings()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Settings_Loaded);

            try
            {
                settings = IsolatedStorageSettings.ApplicationSettings;                
            }
            catch { }
        }

        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            if (settings.Contains(noteSortBy))
            {
                NoteManager.SortType sortType = (NoteManager.SortType)settings[noteSortBy];
                lpSortTypes.SelectedIndex = (int)sortType;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {    
            settings[noteSortBy] = lpSortTypes.SelectedIndex;
            settings.Save();

            GotoNotesView();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            GotoNotesView();
        }

        void GotoNotesView()
        {
            this.NavigationService.Navigate(new Uri("/ViewNotes.xaml", UriKind.Relative));
        }

    }
}