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

namespace NotepadPlus.Notes
{
    public class NoteManager
    {
        private IsolatedStorageSettings settings { get; set; }
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

        public Note GetNote(string noteId)
        {
            if (settings.Contains(noteId))
            {
                return settings[noteId] as Note;
            }

            return null;
        }

        public List<Note> GetNotes()
        {
            this.Notes = settings.Values.Cast<Note>().ToList();            
            return this.Notes;
        }
    }
}
