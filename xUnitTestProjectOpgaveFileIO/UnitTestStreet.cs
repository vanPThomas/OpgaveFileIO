using AdressDataLibrary;

namespace xUnitTestProjectOpgaveFileIO
{
    public class UnitTestStreet
    {
        [Theory]
        [InlineData(3, "blablaStraat")]
        [InlineData(700, "TralalaDreef")]
        public void StreetConstructorTest(int streetId, string streetName)
        {
            Street s = new Street(streetId, streetName);

            Assert.Equal(streetId, s.Id);
            Assert.Equal(streetName, s.Name);
        }

        

    }
}
