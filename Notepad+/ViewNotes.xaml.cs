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

            this.DataContext = this.NoteManager;
            this.Loaded += new RoutedEventHandler(ViewNotes_Loaded);
        }

        void ViewNotes_Loaded(object sender, RoutedEventArgs e)
        {
            tbHeader.Text += string.Format(" ({0})", this.NoteManager.Notes.Count);
        }

        private void AddNote_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AddNote.xaml", UriKind.Relative));
        }
    }
}