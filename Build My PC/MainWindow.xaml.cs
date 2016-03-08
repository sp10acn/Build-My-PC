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
using System.Net;
using System.IO;
using System.Threading;
using System.Xml;
using HtmlAgilityPack;


namespace Build_My_PC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Search search;
        SearchResults[] results;

        public MainWindow()
        {
            InitializeComponent();
            search = new Search();
            search.SearchCompleteEvent += c_OnSearchComplete;
        }

        private void MenuPopupButton_OnClick(object sender, RoutedEventArgs e) { 
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // This is wrong, create a thread the creates a thread[] and loops it checking progress.

            
            search.PerformSearch((int)sldBudgetSlider.Value, (bool)chkAMD.IsChecked, (int)sldIntensity.Value);
        }

        private void sldBudgetSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblBudgetSlider != null && sldBudgetSlider != null) { 
                lblBudgetSlider.Content = "Price: " + sldBudgetSlider.Value.ToString();
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (lblIntensity != null && sldIntensity != null)
            {
                lblIntensity.Content = "Intensity: " + sldIntensity.Value.ToString();
            }
        }

        private void c_OnSearchComplete(object sender, Search.SearchCompleteEventArgs args) {
            Console.WriteLine("DONE...");
            results = args.Results;
            Application.Current.Dispatcher.Invoke(() =>{
                ShowResults(results);
            });
        }

        private void ShowResults(SearchResults[] results) {
            foreach (SearchResults result in results) {
                stackResults.Children.Add(new SearchResult(result));
            }
        }
    }
}
