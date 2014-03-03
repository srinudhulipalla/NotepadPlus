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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Windows.Threading;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace NotepadPlus
{
    public partial class RecordNote : UserControl, IApplicationService
    {
        DispatcherTimer frameworkDispatcherTimer;
        MemoryStream _audioStream = new MemoryStream();

        Microphone microphone = Microphone.Default;
        SoundEffect sound;
        byte[] buffer;

        public MemoryStream AudioStream
        {
            get
            {
                if (_audioStream == null)
                    _audioStream = new MemoryStream();

                return _audioStream;
            }
            set
            {                
                _audioStream = value;
            }
        }

        public RecordNote()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(RecordNote_Loaded);

            this.frameworkDispatcherTimer = new DispatcherTimer();
            this.frameworkDispatcherTimer.Interval = TimeSpan.FromTicks(333333);
            this.frameworkDispatcherTimer.Tick += frameworkDispatcherTimer_Tick;
            this.frameworkDispatcherTimer.Start();
            FrameworkDispatcher.Update();

            microphone.BufferReady += new EventHandler<EventArgs>(microphone_BufferReady);
        }

        void RecordNote_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //AudioStream = new MemoryStream();

            microphone.BufferDuration = TimeSpan.FromMilliseconds(1000);
            buffer = new byte[microphone.GetSampleSizeInBytes(microphone.BufferDuration)];
            microphone.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (microphone.State == MicrophoneState.Started)
            {
                microphone.Stop();
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            sound = new SoundEffect(AudioStream.ToArray(), microphone.SampleRate, AudioChannels.Mono);
            sound.Play();
        }

        void microphone_BufferReady(object sender, EventArgs e)
        {
            microphone.GetData(buffer);
            AudioStream.Write(buffer, 0, buffer.Length);
        }

        void frameworkDispatcherTimer_Tick(object sender, EventArgs e) { FrameworkDispatcher.Update(); }
        void IApplicationService.StartService(ApplicationServiceContext context) { this.frameworkDispatcherTimer.Start(); }
        void IApplicationService.StopService() { this.frameworkDispatcherTimer.Stop(); }        

    }
}
