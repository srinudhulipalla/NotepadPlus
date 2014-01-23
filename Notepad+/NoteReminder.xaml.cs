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
using System.Windows.Controls.Primitives;

namespace NotepadPlus
{
    public partial class NoteReminder : UserControl
    {

        public bool IsComplete = false;

        public NoteReminder()
        {
            InitializeComponent();
            //http://www.geekchamp.com/tips/how-to-get-user-input-from-a-popup-in-windows-phone

            this.Loaded += new RoutedEventHandler(NoteReminder_Loaded);

            
        }

        void NoteReminder_Loaded(object sender, RoutedEventArgs e)
        {
            
            
        }

        
    }
}
