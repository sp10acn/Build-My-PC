using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Build_My_PC
{
    class SearchParameters
    {
        public struct Keywords {
            public string Cpu;
            public string Motherboard;
            public string Memory;
            public string StorageHDD;
            public string StorageSSD;
            public string Case;
            public string GraphicCard;
            public string PowerSupply;

            public Keywords(string _cpu, string _motherboard, string _memory, string _storageHdd, string _storageSsd, string _case, string _graphicCard, string _powerSupply) {
                Cpu = _cpu;
                Motherboard = _motherboard;
                Memory = _memory;
                StorageHDD = _storageHdd;
                StorageSSD = _storageSsd;
                Case = _case;
                GraphicCard = _graphicCard;
                PowerSupply = _powerSupply;
            }
        }

        public struct PriceRanges {
            public Range Cpu;
            public Range Motherboard;
            public Range Memory;
            public Range StorageHDD;
            public Range StorageSSD;
            public Range Case;
            public Range GraphicCard;
            public Range PowerSupply;

            public PriceRanges(Range _cpu, Range _motherboard, Range _memory, Range _storageHdd, Range _storageSsd, Range _case, Range _graphicCard, Range _powerSupply)
            {
                Cpu = _cpu;
                Motherboard = _motherboard;
                Memory = _memory;
                StorageHDD = _storageHdd;
                StorageSSD = _storageSsd;
                Case = _case;
                GraphicCard = _graphicCard;
                PowerSupply = _powerSupply;
            }
        }

        public struct Range {
            public int Min;
            public int Max;
            public Range (int min, int max) {
                Min = min;
                Max = max;
            }
        }

        public Keywords keywords {
            get {
                return kw;
            }
        }
        public PriceRanges priceRanges
        {
            get {
                return pr;
            }
        }

        private PriceRanges pr;
        private Keywords kw;

        public SearchParameters(int budget, bool amd) {
            string cpuKey;
            string motherboardKey;
            string memoryKey = "RAM";
            string storageHDDKey = "3.5+inch+HDD";
            string storageSSDKey = "2.5+inch+SSD";
            string caseKey = "desktop+ATX+gaming+case";
            string graphicCardKey;
            string powerSupplyKey;

            if (amd)
            {
                cpuKey = "amd+cpu";
                motherboardKey = "amd+motherboard+atx";
                graphicCardKey = "amd+graphics+card";
            }
            else
            {
                cpuKey = "intel+socket+1150+cpu";
                motherboardKey = "socket+1150+motherboard+atx+form+factor";
                graphicCardKey = "nvidia+graphics+card";
            }

            XmlDocument xmlRanges = new XmlDocument();
            xmlRanges.Load("Data/PriceRanges.xml");
            XmlNode node = xmlRanges.FirstChild;

            if (budget < 300)
            {
                node = node.SelectSingleNode("_300");
                powerSupplyKey = "desktop+psu+350w+new";
            }
            else if (budget < 600)
            {
                node = node.SelectSingleNode("_600");
                powerSupplyKey = "desktop+psu+500w+new";
            }
            else if (budget < 900)
            {
                node = node.SelectSingleNode("_900");
                powerSupplyKey = "desktop+psu+650w+new";
            }
            else if (budget < 1200)
            {
                node = node.SelectSingleNode("_1200");
                powerSupplyKey = "desktop+psu+850w+new";
            }
            else
            {
                node = node.SelectSingleNode("_MORE");
                powerSupplyKey = "desktop+psu+1000w+new";
            }

            int cpuMin;
            int cpuMax;
            int motherboardMin;
            int motherboardMax;
            int memoryMin;
            int memoryMax;
            int storageHddMin;
            int storageHddMax;
            int storageSsdMin;
            int storageSsdMax;
            int caseMin;
            int caseMax;
            int graphicCardMin;
            int graphicCardMax;
            int powerSupplyMin;
            int powerSupplyMax;

            int.TryParse(node.SelectSingleNode("Cpu").SelectSingleNode("MIN").InnerText, out cpuMin);
            int.TryParse(node.SelectSingleNode("Cpu").SelectSingleNode("MAX").InnerText, out cpuMax);
            int.TryParse(node.SelectSingleNode("Motherboard").SelectSingleNode("MIN").InnerText, out motherboardMin);
            int.TryParse(node.SelectSingleNode("Motherboard").SelectSingleNode("MAX").InnerText, out motherboardMax);
            int.TryParse(node.SelectSingleNode("Memory").SelectSingleNode("MIN").InnerText, out memoryMin);
            int.TryParse(node.SelectSingleNode("Memory").SelectSingleNode("MAX").InnerText, out memoryMax);
            int.TryParse(node.SelectSingleNode("Storage").SelectSingleNode("HDD").SelectSingleNode("MIN").InnerText, out storageHddMin);
            int.TryParse(node.SelectSingleNode("Storage").SelectSingleNode("HDD").SelectSingleNode("MAX").InnerText, out storageHddMax);
            int.TryParse(node.SelectSingleNode("Storage").SelectSingleNode("SSD").SelectSingleNode("MIN").InnerText, out storageSsdMin);
            int.TryParse(node.SelectSingleNode("Storage").SelectSingleNode("SSD").SelectSingleNode("MAX").InnerText, out storageSsdMax);
            int.TryParse(node.SelectSingleNode("Case").SelectSingleNode("MIN").InnerText, out caseMin);
            int.TryParse(node.SelectSingleNode("Case").SelectSingleNode("MAX").InnerText, out caseMax);
            int.TryParse(node.SelectSingleNode("GraphicCard").SelectSingleNode("MIN").InnerText, out graphicCardMin);
            int.TryParse(node.SelectSingleNode("GraphicCard").SelectSingleNode("MAX").InnerText, out graphicCardMax);
            int.TryParse(node.SelectSingleNode("PowerSupply").SelectSingleNode("MIN").InnerText, out powerSupplyMin);
            int.TryParse(node.SelectSingleNode("PowerSupply").SelectSingleNode("MAX").InnerText, out powerSupplyMax);

            pr = new PriceRanges(
                new Range(cpuMin, cpuMax),
                new Range(motherboardMin, motherboardMax),
                new Range(memoryMin, memoryMax),
                new Range(storageHddMin, storageHddMin),
                new Range(storageSsdMin, storageSsdMax),
                new Range(caseMin, caseMax),
                new Range(graphicCardMin, graphicCardMax),
                new Range(powerSupplyMin, powerSupplyMax));

            kw = new Keywords(
                cpuKey,
                motherboardKey,
                memoryKey,
                storageHDDKey,
                storageSSDKey,
                caseKey,
                graphicCardKey,
                powerSupplyKey);
        }
    }
}
