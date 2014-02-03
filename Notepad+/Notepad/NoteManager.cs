﻿using System;
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
        private const string noteSortByKey = "note_SortBy";

        private IsolatedStorageSettings settings { get; set; }
        public List<Note> Notes { get; private set; }

        public enum SortType
        {
            Modified = 0,
            Name = 1
        }

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

            try
            {
                if (string.IsNullOrEmpty(note.Id)) return success;

                settings[note.Id] = note;
                settings.Save();
                success = true;
            }
            catch { success = false; }

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
            this.Notes = settings.Values.OfType<Note>().ToList();

            if (settings.Contains(noteSortByKey))
            {
                SortNotes((SortType)settings[noteSortByKey]);
            }
            else
            {
                SortNotes(SortType.Modified);
            }

            return this.Notes;
        }

        private void SortNotes(SortType sortType)
        {
            if (this.Notes == null) return;

            switch (sortType)
            {
                case SortType.Modified:
                    this.Notes = this.Notes.OrderByDescending(i => i.Modified).ToList();
                    break;
                case SortType.Name:
                    this.Notes = this.Notes.OrderBy(i => i.Title).ToList();
                    break;
            }
        }

    }
}
