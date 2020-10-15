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

      protected void OnPropertyChanged([CallerMemberName] string name = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
      }



      public MainWindow()
        {
            InitializeComponent();
         DownButtonStringImage = "Images/ButtonDownDefault.png";
         DataContext = this;

      }

   

      public void GetKeys()
      {
         
         while (true)
         {
            if (Keyboard.IsKeyDown(Key.S))
            {
               DownButtonStringImage = "Images/ButtonDownUsed.png";
               MessageBox.Show("pressed");
            }
            else
            {
               DownButtonStringImage = "Images/ButtonDownDefault.png";
               MessageBox.Show("not pressed");
            }
            if (Keyboard.IsKeyDown(Key.Up))
            {
               // pink one
            }
            else
            {
               //white one
            }
            if (Keyboard.IsKeyDown(Key.Right))
            {
               // pink one
            }
            else
            {
               //white one
            }
            if (Keyboard.IsKeyDown(Key.Left))
            {
               // pink one
            }
            else
            {
               //white one
            }
            return;
         }
      }

      private void Window_KeyDown(object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Down) // The Arrow-Down key
         {
            DownButtonStringImage = "Images/ButtonDownUsed.png";
            //MessageBox.Show(DownButtonStringImage);
         }
         //else if (e.Key==Key.Up)
         //{
         //   DownButtonStringImage = "Images/ButtonUpUsed.png";
         //   MessageBox.Show("not pressed");
         //}

      }

     
   }
}
