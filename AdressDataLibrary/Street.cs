namespace AdressDataLibrary
{
    public class Street : ILocation
    {
        public Street(int streetId, string streetName)
        {
            Id = streetId;
            Name = streetName;
        }

        public int Id { get; set; }
        public string Name { get; set; }

    }
}