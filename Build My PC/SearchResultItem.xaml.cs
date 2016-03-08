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
using System.Diagnostics;

namespace Build_My_PC
{
    /// <summary>
    /// Interaction logic for SearchResultItem.xaml
    /// </summary>
    public partial class SearchResultItem : UserControl
    {
        public SearchResultItem(string title, string price, string stores, string link)
        {
            InitializeComponent();
            lblTitle.Content = title;
            lblPrice.Content = "£" + price;
            lblStores.Content = stores;
            hlLink.NavigateUri = new Uri(link);
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

    }
}
