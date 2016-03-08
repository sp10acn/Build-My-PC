using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Build_My_PC
{
    public class SearchResults
    {
        private string googleAddress = @"https://www.google.co.uk";

        public struct SearchResult{
            public string Title;
            public float Price;
            public string StoreInfo;
            public string Link;
            public SearchResult(string title, float price, string storeInfo, string link) {
                Title = title;
                Price = price;
                StoreInfo = storeInfo;
                Link = link;
            }
        }

        public string SearchIdentifier {
            get {
                return searchIdentifier;
            }
        }

        public List<SearchResult> searchResults {
            get {
                return sr;
            }
        }

        private List<SearchResult> sr;
        private string searchIdentifier;

        public SearchResults(string identifier) {
            sr = new List<SearchResult>();
            searchIdentifier = identifier;
        }

        public void ClearResults() {
            sr.Clear();
        }

        public void AddGoogleResults2(HtmlDocument doc)
        {
            HtmlNode rn = doc.DocumentNode;
            foreach (HtmlNode n in rn.Descendants("div"))
            {
                foreach (HtmlAttribute att in n.Attributes)
                {
                    if (att.Value.Equals("psli"))
                    {
                        string title = "failed";
                        string price = "failed";
                        string storeCount = "failed";
                        string link = "failed";

                        foreach (HtmlNode n2 in n.Descendants("a"))
                        {
                            foreach (HtmlAttribute att2 in n2.Attributes)
                            {
                                if (att2.Value.Equals("pstl"))
                                {
                                    // Title and Link
                                    link = googleAddress + n2.Attributes["href"].Value;
                                    title = n2.InnerText;
                                }
                            }
                        }
                        
                        foreach (HtmlNode n2 in n.Descendants("div"))
                        {
                            foreach (HtmlAttribute att2 in n2.Attributes)
                            {
                                if (att2.Value.Equals("_tyb shop__secondary"))
                                {
                                    // Price
                                    price = n2.FirstChild.FirstChild.InnerText;
                                    storeCount = n2.InnerText;
                                }
                            }
                        }

                        price = price.Substring(1);
                        float priceFloat;
                        float.TryParse(price, out priceFloat);
                        sr.Add(new SearchResult(title,
                            priceFloat,
                            storeCount,
                            link));
                    }
                }
            }
        }

        public void AddGoogleResults(HtmlDocument doc) {
            HtmlNode rn = doc.DocumentNode;
            foreach (HtmlNode n in rn.Descendants("div"))
            {
                foreach (HtmlAttribute att in n.Attributes)
                {
                    if (att.Value.Equals("pslires"))
                    {
                        string title = "failed";
                        string price = "failed";
                        string storeCount = "failed";
                        string link = "failed";

                        foreach (HtmlNode n2 in n.Descendants("h3"))
                        {
                            foreach (HtmlAttribute att2 in n2.Attributes)
                            {
                                if (att2.Value.Equals("r"))
                                {
                                    // Title and Link
                                    link = googleAddress + n2.SelectSingleNode("a").Attributes["href"].Value;
                                    title = n2.SelectSingleNode("a").InnerText;
                                }
                            }
                        }

                        foreach (HtmlNode n2 in n.Descendants("div"))
                        {
                            foreach (HtmlAttribute att2 in n2.Attributes)
                            {
                                if (att2.Value.Equals("_OA"))
                                {
                                    // Price
                                    HtmlNodeCollection priceNodes = n2.SelectNodes("div");
                                    price = priceNodes[0].FirstChild.InnerText;
                                    storeCount = priceNodes[1].InnerText;
                                }
                            }
                        }

                        price = price.Substring(1);
                        float priceFloat;
                        float.TryParse(price, out priceFloat);
                        sr.Add(new SearchResult(title,
                            priceFloat,
                            storeCount,
                            link));
                    }
                }
            }
        }
    }
}
