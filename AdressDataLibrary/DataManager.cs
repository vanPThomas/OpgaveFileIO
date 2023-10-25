using System.IO.Compression;

namespace AdressDataLibrary
{
    public class DataManager
    {
        List<Province> wholeStructure = new List<Province>();

        private string _extractedFilePath;
        private string _rootFolder;

        private static DataManager DatamanagerInst;

        private DataManager()
        {
        }

        public static DataManager GetInstance()
        {
            if (DatamanagerInst != null)
                return DatamanagerInst;
            else
            {
                DatamanagerInst = new DataManager();
                return DatamanagerInst;
            }
        }

        public void CreateDataFromFiles(string fileExtractLocation)
        {
            Console.WriteLine("***Commencing Data Extraction ***");
            _extractedFilePath = fileExtractLocation;
            UnzipToPath();
            FillWholeStructList();
            FillCompositeStructure();
            Console.WriteLine("***Data Extracted***");
        }
        private void UnzipToPath ()
        {
            string zipPath = @"straatnamenInfo.zip";
            string extractPath = $"{_extractedFilePath}";
            if(!Directory.Exists(extractPath))
            {
                ZipFile.ExtractToDirectory(zipPath, extractPath);
            }
            else
            {
                Directory.Delete(extractPath, true);
                ZipFile.ExtractToDirectory (zipPath, extractPath);
            }
        }
        private void FillWholeStructList()
        {
            int[] provinceIdArray;
            using(StreamReader sr = File.OpenText($"{_extractedFilePath}\\ProvincieIDsVlaanderen.csv"))
            {
                string[] provinceIdListAsText = sr.ReadLine().Split(',');
                provinceIdArray = new int[provinceIdListAsText.Length];
                for(int i = 0; i < provinceIdListAsText.Length; i++)
                {
                    provinceIdArray[i] = int.Parse(provinceIdListAsText[i]);
                }
            }
            using (StreamReader sr = File.OpenText($"{_extractedFilePath}\\ProvincieInfo.csv"))
            {
                string input = null;
                string[] inputArray;
                while ((input = sr.ReadLine()) != null)
                {
                    inputArray = input.Split(";");
                    int provinceId;
                    if (Int32.TryParse(inputArray[1], out provinceId) && inputArray[2] == "nl")
                    {
                        for(int i = 0; i < provinceIdArray.Length;i++)
                        {
                            if (provinceIdArray[i] == provinceId)
                            {
                                Province province = new Province(provinceId, inputArray[3]);
                                wholeStructure.Add(province);
                                provinceIdArray[i] = 0;

                            }
                        }
                    }
                }
            }
        }

        private void FillCompositeStructure()
        {
            Dictionary<int, Street> streets = new Dictionary<int, Street>();
            List<Town> towns = new List<Town>();
            Dictionary<Town, List<Street>> townsToStreets = new Dictionary<Town, List<Street>>();
            FillStreets(streets);
            FillTowns(towns);
            FillTownsToStreets(townsToStreets, towns, streets);
            FillProvinceToTown(townsToStreets);

        }

        private void FillStreets(Dictionary<int, Street> streets)
        {
            using(StreamReader sr = File.OpenText($"{_extractedFilePath}\\straatnamen.csv"))
            {
                string input = null;
                string[] inputArray;
                while((input = sr.ReadLine()) != null)
                {
                    inputArray = input.Split(";");
                    int idAsNum;
                    if (Int32.TryParse(inputArray[0], out idAsNum))
                        if(idAsNum > 0)
                        {
                            Street street = new Street(idAsNum, inputArray[1]);
                            streets.Add(street.Id, street);
                        }
                }
            }
        }
        private void FillTowns(List<Town> towns)
        {
            using(StreamReader sr = File.OpenText($"{_extractedFilePath}\\GemeenteNaam.csv"))
            {
                string input = null;
                string[] inputArray;
                while((input = sr.ReadLine()) != null)
                {
                    inputArray = input.Split(";");
                    int idAsNum;
                    if (inputArray[2] == "nl" && Int32.TryParse(inputArray[1], out idAsNum))
                        if (idAsNum > 0)
                        {
                            Town town = new Town(idAsNum, inputArray[3]);
                            towns.Add(town);
                        }
                }
            }
        }
        private void FillTownsToStreets(Dictionary<Town, List<Street>> townsToStreets, List<Town> towns, Dictionary<int, Street> streets)
        {
            List<TownStreetLink> townStreetLinks = new List<TownStreetLink>();
            FillTownStreetLink(townStreetLinks);

            for(int i = 0; i < towns.Count; i++)
            {
                List<Street> streetListForTown = new List<Street>();
                Town curTown = towns[i];
                townsToStreets.Add(curTown, streetListForTown);
                for(int j = 0; j < townStreetLinks.Count; j++) 
                {
                    if (townStreetLinks[j].TownId == curTown.Id)
                    {
                        TownStreetLink curTownStreetLink = townStreetLinks[j];
                        if (streets.ContainsKey(curTownStreetLink.StreetId))
                            townsToStreets[curTown].Add(streets[curTownStreetLink.StreetId]);
                        //try
                        //{
                        //    if (streets.ContainsKey(curTownStreetLink.StreetId))
                        //        townsToStreets[curTown].Add(streets[curTownStreetLink.StreetId]);
                        //    else
                        //        throw new Exception("There is no street with that ID!");
                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine();
                        //    Console.WriteLine("Street Exception!");
                        //    StreetException.ExceptionDetails(ex);
                        //}
                    }
                }
            }
        }
        private void FillTownStreetLink(List<TownStreetLink> townStreetLinks)
        {
            using(StreamReader sr = File.OpenText($"{_extractedFilePath}\\StraatnaamID_gemeenteID.csv"))
            {
                string input = null;
                string[] inputArray;
                while((input = sr.ReadLine()) != null)
                {
                    inputArray = input.Split(";");
                    int townIdAsNum;
                    int streetIdAsNum;
                    if (Int32.TryParse(inputArray[1], out townIdAsNum) && Int32.TryParse(inputArray[0], out streetIdAsNum)) 
                    {
                        TownStreetLink townStreetLink = new TownStreetLink(townIdAsNum, streetIdAsNum);
                        townStreetLinks.Add(townStreetLink);
                    }
                }
            }
        }
        private void FillProvinceToTown(Dictionary<Town, List<Street>> townsToStreets)
        {
            using(StreamReader sr = File.OpenText($"{_extractedFilePath}\\ProvincieInfo.csv"))
            {
                string input = null;
                string[] inputArray;

                while((input = sr.ReadLine()) != null)
                {
                    inputArray = input.Split(";");
                    int townIdAsNum;
                    int provinceIdAsNum;
                    if (Int32.TryParse(inputArray[0], out townIdAsNum) && Int32.TryParse(inputArray[1], out provinceIdAsNum) && inputArray[2] == "nl")
                        foreach(Province province in wholeStructure)
                            if(province.Id == provinceIdAsNum)
                            {
                                foreach(Town town in townsToStreets.Keys)
                                    if (town.Id == townIdAsNum)
                                    {
                                        foreach(Street street in townsToStreets[town])
                                            town.addStreet(street);
                                        province.addTown(town);
                                        break;
                                    }
                                break;
                            }
                }
            }
        }

        public void CreatingFoldersAndFilesFromData(string rootFolder)
        {
            _rootFolder = rootFolder;
            FolderCreator(_rootFolder);
            CreateSmallFiles();
            CreateMegaFile();

        }
        private void CreateSmallFiles()
        {
            foreach (Province province in wholeStructure)
            {
                string provinceDir = $"{_rootFolder}\\{province.Name}";
                List<ILocation> towns = province.GetTowns();
                FolderCreator(provinceDir);
                foreach (Town town in towns)
                {
                    List<ILocation> streetList = town.GetStreets();
                    List<ILocation> sorted = streetList.OrderBy(x => x.Name).ToList();
                    using StreamWriter file = new($"{provinceDir}\\{town.Name}.txt");
                    foreach (Street street in sorted)
                        file.WriteLine(street.Name);
                }
            }
        }
        private void CreateMegaFile()
        {
            using StreamWriter file = new($"{_rootFolder}\\BigList.txt");
            foreach (Province province in wholeStructure)
            {
                List<ILocation> towns = province.GetTowns();
                foreach (Town town in towns)
                {
                    List<ILocation> streetList = town.GetStreets();
                    List<ILocation> sorted = streetList.OrderBy(x => x.Name).ToList();
                    foreach (Street street in sorted)
                        file.WriteLine($"{province.Name},{town.Name},{street.Name}");
                }
            }
        }
        private void FolderCreator(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            else
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
        }
        public void DeleteAll()
        {
            Directory.Delete(_extractedFilePath, true);
            Directory.Delete(_rootFolder, true);
        }
    }
}