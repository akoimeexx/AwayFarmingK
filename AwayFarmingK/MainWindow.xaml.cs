using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace com.akoimeexx.utilities.AwayFarmingK
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public ObservableCollection<MinecraftInstance> MinecraftInstances {
            get; private set;
        } = new ObservableCollection<MinecraftInstance>();
        public MainWindow() {
            InitializeComponent();
            DataContext = this;

            AppDomain.CurrentDomain.ProcessExit +=
                Cleanup_MinecraftInstances;
            AppDomain.CurrentDomain.DomainUnload +=
                Cleanup_MinecraftInstances;
            AppDomain.CurrentDomain.UnhandledException +=
                Cleanup_MinecraftInstances;
            ReloadInstances_Click(this, null);
        }

        private void ReloadInstances_Click(object sender, RoutedEventArgs e) {
            List<MinecraftInstance> hiddenInstances = new List<MinecraftInstance>();
            foreach (MinecraftInstance i in MinecraftInstances.Where(
                _ => {
                    return _.IsVisible == false;
                }
            )) {
                hiddenInstances.Add(i);
            }
            MinecraftInstances.Clear();
            foreach (MinecraftInstance i in hiddenInstances) {
                MinecraftInstances.Add(i);
            }
            foreach (Process p in MinecraftHook.GetMinecraftInstances()) {
                MinecraftInstances.Add(new MinecraftInstance(p));
            }
        }

        private void ToggleClicking_Click(object sender, RoutedEventArgs args) {
            try {
                if (Instances.SelectedItem != null) {
                    if (((MinecraftInstance)Instances.SelectedItem).IsClicking) {
                        MinecraftHook.SendMouseEvent(
                            ((MinecraftInstance)Instances.SelectedItem).Process.MainWindowHandle, 
                            MinecraftHook.MouseActions.RightButton_Up
                        );
                        ((MinecraftInstance)Instances.SelectedItem).IsClicking = false;
                        if (!((MinecraftInstance)Instances.SelectedItem).IsVisible) {
                            MinecraftHook.SendWindowMessage(
                                ((MinecraftInstance)Instances.SelectedItem).Process.MainWindowHandle,
                                MinecraftHook.WindowMessage.ShowNA
                            );
                            ((MinecraftInstance)Instances.SelectedItem).IsVisible = true;
                        }
                    } else {
                        MinecraftHook.SendMouseEvent(
                            ((MinecraftInstance)Instances.SelectedItem).Process.MainWindowHandle,
                            MinecraftHook.MouseActions.RightButton_Down
                        );
                        ((MinecraftInstance)Instances.SelectedItem).IsClicking = true;
                        if (HideInstance.IsChecked == true) {
                            MinecraftHook.SendWindowMessage(
                                ((MinecraftInstance)Instances.SelectedItem).Process.MainWindowHandle,
                                MinecraftHook.WindowMessage.Hide
                            );
                            ((MinecraftInstance)Instances.SelectedItem).IsVisible = false;
                        }
                    }
                }
            } catch (Exception e) { Console.Error.WriteLineAsync(e.Message); }
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show(
                String.Join(
                    Environment.NewLine, 
                    "Written by Akoi Meexx", 
                    "", 
                    "This project is a one-off from my main afk program, intended to work",
                    "around changes in Minecraft 1.13 that prevent users from auto-clicking",
                    "the right mouse button while the window is inactive during afk sessions."
                ), 
                "About Away Farming, K?", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information
            );
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        private void Cleanup_MinecraftInstances(object sender, EventArgs args) {
            // Resume showing all windows that are hidden
            foreach (
                MinecraftInstance i in MinecraftInstances.Where(
                    _ => { return !_.IsVisible || _.IsClicking; }
                )
            ) try {
                MinecraftHook.SendMouseEvent(
                    i.Process.MainWindowHandle,
                    MinecraftHook.MouseActions.RightButton_Up
                );
                MinecraftHook.SendWindowMessage(
                    i.Process.MainWindowHandle, 
                    MinecraftHook.WindowMessage.ShowNA
                );
            } catch (Exception e) {
                Console.Error.WriteLineAsync(String.Format(
                    String.Join(
                        Environment.NewLine, 
                        "An error occured while trying to clean up minecraft instance \"{0}\" ({1})", 
                        "Exception message: {2} ({3})"
                    ), 
                    i.ToString(), 
                    i.Process.MainWindowHandle.ToString(), 
                    e.Message, 
                    e.HResult
                ));
            }
        }
    }
}
