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
using NotepadPlus.Notes;
using Microsoft.Phone.Shell;

namespace NotepadPlus
{
    public partial class ViewNotes : PhoneApplicationPage
    {
        private NoteManager _noteManager;

        private NoteManager NoteManager
        {
            get
            {
                if (_noteManager == null)
                {
                    _noteManager = new NoteManager();
                    _noteManager.GetNotes();
                }

                return _noteManager;
            }
        }

        public ViewNotes()
        {
            InitializeComponent();

            SystemTray.SetIsVisible(this, true);
            SystemTray.SetOpacity(this, 0);

            this.DataContext = this.NoteManager;
            this.Loaded += new RoutedEventHandler(ViewNotes_Loaded);
        }

        void GotoAddOrEditNote(string noteId)
        {
            string uriString = "/AddOrEditNote.xaml" + (string.IsNullOrEmpty(noteId) ? string.Empty : "?noteId=" + noteId);
            this.NavigationService.Navigate(new Uri(uriString, UriKind.Relative));
        }

        void ViewNotes_Loaded(object sender, RoutedEventArgs e)
        {
            tbHeader.Text = string.Format("Notepad+ ({0})", this.NoteManager.Notes.Count);
        }

        private void lstNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Note note = ((ListBox)sender).SelectedItem as Note;
            if (note == null) return;

            GotoAddOrEditNote(note.Id);
        }

        private void AddNote_Click(object sender, EventArgs e)
        {
            GotoAddOrEditNote(string.Empty);
        }

        private void Settings_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Settings.xaml", UriKind.Relative));
        }        

        protected void RateApp_Click(object sender, EventArgs e)
        {
            Common.RateMyApp();
        }

        private void MoreApps_Click(object sender, EventArgs e)
        {
            Common.MoreAppsFromMe();
        }

        private void About_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }        

    }
}