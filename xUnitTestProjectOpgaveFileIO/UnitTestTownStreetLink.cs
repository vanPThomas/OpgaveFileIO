using AdressDataLibrary;

namespace xUnitTestProjectOpgaveFileIO
{
    public class UnitTestTownStreetLink
    {
        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 80)]
        [InlineData(3, 9)]

        public void TSLConstructorTest(int townId, int streetId)
        {
            TownStreetLink tsl = new TownStreetLink(townId, streetId);

            Assert.Equal(townId , tsl.TownId);
            Assert.Equal(streetId, tsl.StreetId);
        }
    }
}