namespace AdressDataLibrary
{
    public class TownStreetLink
    {
        public TownStreetLink(int townId, int streetId)
        {
            TownId = townId;
            StreetId = streetId;
        }

        public int TownId { get; private set; }
        public int StreetId { get; private set; }
    }
}
