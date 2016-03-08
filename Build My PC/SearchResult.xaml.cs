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

namespace Build_My_PC
{
    /// <summary>
    /// Interaction logic for SearchResult.xaml
    /// </summary>
    public partial class SearchResult : UserControl
    {
        public SearchResult(SearchResults results)
        {
            InitializeComponent();
            lblTitle.Text = results.SearchIdentifier;
            foreach (SearchResults.SearchResult sr in results.searchResults)
            {
                stackItems.Children.Add(new SearchResultItem(sr.Title, sr.Price.ToString(), sr.StoreInfo, sr.Link));
            }
        }
    }
}
