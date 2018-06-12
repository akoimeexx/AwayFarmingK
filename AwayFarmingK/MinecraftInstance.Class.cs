using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace com.akoimeexx.utilities.AwayFarmingK {
    public class MinecraftInstance : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool Set<TRef>(
            ref TRef field, 
            TRef value, 
            [CallerMemberName] string property = null
        ) {
            bool b = default(bool);
            try {
                field = value;
                PropertyChanged?.DynamicInvoke(
                    this, new PropertyChangedEventArgs(property)
                );
            } catch (Exception e) { Console.Error.WriteLineAsync(e.Message); }
            return b;
        }
        public Process Process {
            get { return _process; }
            private set { Set(ref _process, value); }
        } private Process _process = default(Process);
        public bool IsVisible {
            get { return _isVisible; }
            set { Set(ref _isVisible, value); }
        } private bool _isVisible = default(bool);
        public bool IsClicking {
            get { return _isClicking; }
            set { Set(ref _isClicking, value); }
        } private bool _isClicking = default(bool);

        public MinecraftInstance(Process process) {
            Process = process;
            IsVisible = true;
        }
        public override string ToString() {
            string s = base.ToString();
            try {
                s = String.Format(
                    "{0} ({1})",
                    Process.MainWindowTitle,
                    Process.ProcessName
                );
            } catch (Exception e) { Console.Error.WriteLineAsync(e.Message); }
            return s;
        }
    }
}
