using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace NotepadPlus.Notes
{
    public class NoteManager
    {
        IsolatedStorageSettings settings { get; set; }
        public List<Note> Notes { get; private set; }        

        public NoteManager()
        {
            try
            {
                settings = IsolatedStorageSettings.ApplicationSettings;
                Notes = new List<Note>();                
            }
            catch { }
        }

        public bool AddOrUpdateNote(Note note)
        {
            bool success = false;

            if (string.IsNullOrEmpty(note.Id)) return success;

            if (settings.Contains(note.Id))
            {
                if (!settings[note.Id].Equals(note))
                {
                    settings[note.Id] = note;
                    settings.Save();
                    success = true;
                }
            }
            else
            {
                settings.Add(note.Id, note);
                settings.Save();
                success = true;
            }

            return success;
        }

        public bool DeleteNote(string noteId)
        {
            bool success = false;

            if (settings.Contains(noteId))
            {
                settings.Remove(noteId);
                success = true;
            }

            return success;
        }

        public List<Note> GetNotes()
        {
            List<Note> savedNotes = settings.Values.Cast<Note>().ToList();
            this.Notes = savedNotes;
            return savedNotes;
        }
    }

    public class Note : INotifyPropertyChanged
    {
        private string _id = string.Empty;
        private string _title = string.Empty;
        private string _content = string.Empty;
        private DateTime _created;
        private DateTime _modified;        

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                if (value != _content)
                {
                    _content = value;
                    NotifyPropertyChanged("Content");
                }
            }
        }

        public DateTime Created
        {
            get
            {
                return _created;
            }
            set
            {
                if (value != _created)
                {
                    _created = value;
                    NotifyPropertyChanged("Created");
                }
            }
        }

        public DateTime Modified
        {
            get
            {
                return _modified;
            }
            set
            {
                if (value != _modified)
                {
                    _modified = value;
                    NotifyPropertyChanged("Modified");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
       

}
