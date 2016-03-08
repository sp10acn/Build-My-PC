using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using HtmlAgilityPack;
using System.Net;
using System.IO;

namespace Build_My_PC
{
    class Search
    {

        public struct MainSearchThreadParams {
            public int searchBudget;
            public bool isAMD;
            public int searchIntensity;
            public MainSearchThreadParams(int budget, bool amd, int intensity)
            {
                searchBudget = budget;
                isAMD = amd;
                searchIntensity = intensity;
            }
        }

        public struct SingleSearchThreadParams
        {
            public string Keywords;
            public string Identifier;
            public int SearchIntensity;
            public SingleSearchThreadParams(string keywords, string identifier, int searchIntensity)
            {
                Keywords = keywords;
                Identifier = identifier;
                SearchIntensity = searchIntensity;
            }
        }

        public bool IsSearchComplete {
            get {
                return isSearchComplete;
            }
        }

        public SearchResults[] Results {
            get{
                return searchResults.ToArray();
            }   
        }

        private List<SearchResults> searchResults;
        private bool isSearchComplete;

        public Search() {
            searchResults = new List<SearchResults>();
            isSearchComplete = false;
        }

        public void PerformSearch(int budget, bool amd, int intensity) {
            isSearchComplete = false;
            MainSearchThreadParams mainSearchThreadParams = new MainSearchThreadParams(budget, amd, intensity);

            Thread threadSearch = new Thread(GoogleSearchHandler);
            threadSearch.Start(mainSearchThreadParams);
        }

        public void GoogleSearch(object data)
        {
            string website = @"https://www.google.co.uk";
            string searchTermStart = @"/search?q=";
            string searchTermEnd = @"&tbm=shop";
            string pageSelect = @"&start=";

            SingleSearchThreadParams sstp = (SingleSearchThreadParams)data;
            SearchResults sr = new SearchResults(sstp.Identifier);
            HtmlDocument tempDoc = new HtmlDocument();

            for (int i = 0; i < sstp.SearchIntensity; i++)
            {
                string html = string.Empty;
                string keywords = sstp.Keywords;
                Console.WriteLine(website + searchTermStart + keywords + searchTermEnd);

                HttpWebRequest request;
                if (i == 0)
                {
                    request = (HttpWebRequest)WebRequest.Create(website + searchTermStart + keywords + searchTermEnd);
                }
                else {
                    request = (HttpWebRequest)WebRequest.Create(website + searchTermStart + keywords + searchTermEnd + pageSelect + (i * 20).ToString());

                }
                request.AutomaticDecompression = DecompressionMethods.GZip;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                tempDoc.LoadHtml(html);
                sr.AddGoogleResults(tempDoc);
                Thread.Sleep(200 * i);
            }

            searchResults.Add(sr);
        }

        private void GoogleSearchHandler(object data)
        {
            MainSearchThreadParams tp = (MainSearchThreadParams)data;

            SearchParameters sp = new SearchParameters(tp.searchBudget, tp.isAMD);

            Thread[] threads = new Thread[8];

            SingleSearchThreadParams sstpTemp;

            sstpTemp = new SingleSearchThreadParams(sp.keywords.Cpu, "CPU", tp.searchIntensity);
            threads[0] = new Thread(GoogleSearch);
            threads[0].Start(sstpTemp);

            sstpTemp = new SingleSearchThreadParams(sp.keywords.Motherboard, "Motherboard", tp.searchIntensity);
            threads[1] = new Thread(GoogleSearch);
            threads[1].Start(sstpTemp);

            sstpTemp = new SingleSearchThreadParams(sp.keywords.Memory, "Memory", tp.searchIntensity);
            threads[2] = new Thread(GoogleSearch);
            threads[2].Start(sstpTemp);

            sstpTemp = new SingleSearchThreadParams(sp.keywords.StorageHDD, "Storage HDD", tp.searchIntensity);
            threads[3] = new Thread(GoogleSearch);
            threads[3].Start(sstpTemp);

            sstpTemp = new SingleSearchThreadParams(sp.keywords.StorageSSD, "Storage SSD", tp.searchIntensity);
            threads[4] = new Thread(GoogleSearch);
            threads[4].Start(sstpTemp);

            sstpTemp = new SingleSearchThreadParams(sp.keywords.Case, "Case", tp.searchIntensity);
            threads[5] = new Thread(GoogleSearch);
            threads[5].Start(sstpTemp);

            sstpTemp = new SingleSearchThreadParams(sp.keywords.GraphicCard, "Graphics Card", tp.searchIntensity);
            threads[6] = new Thread(GoogleSearch);
            threads[6].Start(sstpTemp);

            sstpTemp = new SingleSearchThreadParams(sp.keywords.PowerSupply, "Power Supply", tp.searchIntensity);
            threads[7] = new Thread(GoogleSearch);
            threads[7].Start(sstpTemp);

            bool isRunning = true;
            while (isRunning)
            {
                int threadCounter = 0;
                for (int i = 0; i < threads.Length; i++)
                {
                    if (threads[0].IsAlive)
                    {
                        threadCounter++;
                    }
                }
                if (threadCounter == 0) isRunning = false;

                Application.Current.Dispatcher.Invoke(() =>
                {
                    (Application.Current.MainWindow as MainWindow).pbProgress.Value = 100 - ((100 / threads.Length) * threadCounter);
                });
                Thread.Sleep(200);
            }

            searchResults = FilterResults(sp, searchResults.ToArray());

            isSearchComplete = true;
            SearchCompleteEventArgs args = new SearchCompleteEventArgs(searchResults.ToArray());
            OnSearchComplete(args);
        }

        private List<SearchResults> FilterResults(SearchParameters searchParams, SearchResults[] searchResults) {
            for (int i = 0; i < searchResults.Length; i++) {
                switch (searchResults[i].SearchIdentifier)
                {
                    case "CPU":
                        searchResults[i] = FilterResult(searchParams.priceRanges.Cpu, searchResults[i]);
                        break;
                    case "Motherboard":
                        searchResults[i] = FilterResult(searchParams.priceRanges.Motherboard, searchResults[i]);
                        break;
                    case "Memory":
                        searchResults[i] = FilterResult(searchParams.priceRanges.Memory, searchResults[i]);
                        break;
                    case "Storage HDD":
                        searchResults[i] = FilterResult(searchParams.priceRanges.StorageHDD, searchResults[i]);
                        break;
                    case "Storage SSD":
                        searchResults[i] = FilterResult(searchParams.priceRanges.StorageSSD, searchResults[i]);
                        break;
                    case "Case":
                        searchResults[i] = FilterResult(searchParams.priceRanges.Case, searchResults[i]);
                        break;
                    case "Graphics Card":
                        searchResults[i] = FilterResult(searchParams.priceRanges.GraphicCard, searchResults[i]);
                        break;
                    case "Power Supply":
                        searchResults[i] = FilterResult(searchParams.priceRanges.PowerSupply, searchResults[i]);
                        break;
                }
            }
            return searchResults.ToList();
        }

        private SearchResults FilterResult(SearchParameters.Range range, SearchResults res) {
            List<SearchResults.SearchResult> toRemove = new List<SearchResults.SearchResult>();
            foreach (SearchResults.SearchResult r in res.searchResults)
            {
                if (r.Price > range.Max || r.Price < range.Min)
                    toRemove.Add(r);
            }
            foreach (SearchResults.SearchResult removal in toRemove) {
                res.searchResults.Remove(removal);
            }
            return res;
        }

        // Search Complete Event

        public event EventHandler<SearchCompleteEventArgs> SearchCompleteEvent;
        public delegate void SearchCompleteEventHandler(SearchCompleteEventArgs e);

        public class SearchCompleteEventArgs : EventArgs
        {
            public SearchResults[] Results
            {
                get
                {
                    return res;
                }
            }

            private SearchResults[] res;

            public SearchCompleteEventArgs(SearchResults[] results)
            {
                res = results;
            }
        }

        protected virtual void OnSearchComplete(SearchCompleteEventArgs e)
        {
            EventHandler<SearchCompleteEventArgs> handler = SearchCompleteEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
