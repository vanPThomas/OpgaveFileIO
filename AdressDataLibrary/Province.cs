namespace AdressDataLibrary
{
    public class Province : ILocation
    {
        public Province(int provinceId, string provinceName)
        {
            Id = provinceId;
            Name = provinceName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        private List<ILocation> towns = new List<ILocation>();

        public void addTown(Town town)
        {
            towns.Add(town);
        }

        public List<ILocation> GetTowns()
        {
            return towns;
        }
    }
}
