using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

namespace DesktopUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
      

      private string downButtonStringImage;

      public event PropertyChangedEventHandler PropertyChanged;

      public string DownButtonStringImage
      {
         get { return downButtonStringImage; }
         set {
            downButtonStringImage = value;
            OnPropertyChanged();

         } 
      }
      private string upButtonStringImage;

      public string UpButtonStringImage
      {
         get { return upButtonStringImage; }
         set { upButtonStringImage = value; OnPropertyChanged(); }
      }

      private string leftButtonStringImage;

      public string LeftButtonStringImage
      {
         get { return leftButtonStringImage; }
         set { leftButtonStringImage = value; OnPropertyChanged(); }
      }

      protected void OnPropertyChanged([CallerMemberName] string name = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
      }


      public MainWindow()
        {
            InitializeComponent();
         DownButtonStringImage = "Images/ButtonDownDefault.png";
         UpButtonStringImage = "Images/ButtonUpDefault.png";
         LeftButtonStringImage = "Images/ButtonLeftDefault.png";
         DataContext = this;

      }


      private void Window_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Down) // The Arrow-Down key
         {
            DownButtonStringImage = "Images/ButtonDownUsed.png";
            
         }
         else if (e.Key == Key.Up)
         {
            UpButtonStringImage = "Images/ButtonUpUsed.png";
            
         }
         else if (e.Key == Key.Left)
         {
           LeftButtonStringImage = "Images/ButtonLeftUsed.png";

         }

      }

      private void Window_KeyUp(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Down)
         {
            DownButtonStringImage = "Images/ButtonDownDefault.png";
         }
         else if (e.Key == Key.Up)
         {
            UpButtonStringImage = "Images/ButtonUpDefault.png";

         }
         else if (e.Key == Key.Left)
         {
            LeftButtonStringImage = "Images/ButtonLeftDefault.png";

         }
      }
   }
}
