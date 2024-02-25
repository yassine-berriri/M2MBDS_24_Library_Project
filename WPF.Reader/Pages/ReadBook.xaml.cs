using System;
using System.Collections.Generic;
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
using ViewModel = WPF.Reader.ViewModel;

namespace WPF.Reader.Pages
{
    /// <summary>
    /// Interaction logic for ReadBook.xaml
    /// </summary>
    public partial class ReadBook : Page
    {
        ViewModel.ReadBook readbookinstance = new ViewModel.ReadBook();

        public ReadBook()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ContextMenuOP(object sender, ContextMenuEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && textBox.SelectionLength > 0)
            {
                ContextMenu contextMenu = new ContextMenu();

                MenuItem readFromHereItem = new MenuItem();
                readFromHereItem.Header = "Lire la partie selectionner";
                readFromHereItem.Click += (s, args) =>
                {
                    // Commencer la lecture à partir de la sélection
                    readbookinstance.ReadFromSelection(textBox.SelectedText);
                  //  ViewModel.ReadBook.ReadFromSelection(textBox.SelectedText);
                };

                contextMenu.Items.Add(readFromHereItem);
                textBox.ContextMenu = contextMenu;
            }
            else
            {
                e.Handled = true; // Empêche le menu contextuel de s'ouvrir s'il n'y a pas de sélection
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
