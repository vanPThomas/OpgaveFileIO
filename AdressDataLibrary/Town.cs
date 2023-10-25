namespace AdressDataLibrary
{
    public class Town : ILocation
    {
        public Town(int townId, string townName)
        {
            Id = townId;
            Name = townName;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        private List<ILocation> streets = new List<ILocation>();

        public void addStreet(Street street)
        {
            streets.Add(street);
        }

        public List<ILocation> GetStreets() 
        {
            return streets;
        }
    }
}
