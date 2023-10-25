using AdressDataLibrary;

namespace AdressUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataManager dataManager = DataManager.GetInstance();

            Console.WriteLine("Where to extractfiles?");
            Console.Write("Enter extraction folder name: ");
            string extractFolder = Console.ReadLine();
            dataManager.CreateDataFromFiles(extractFolder);
            Console.WriteLine();
            Console.WriteLine("Where to create files with output?");
            Console.Write("Enter data folder name: ");
            string dataFolder = Console.ReadLine();
            dataManager.CreatingFoldersAndFilesFromData(dataFolder);
            Console.WriteLine();
            Console.WriteLine("Do you want to delete all created folders? y/n");
            Console.Write("Delete: ");
            string deleteAnswer = Console.ReadLine();
            if (deleteAnswer.ToLower() == "y")
                dataManager.DeleteAll();
        }
    }
}