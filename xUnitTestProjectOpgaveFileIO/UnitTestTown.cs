using AdressDataLibrary;

namespace xUnitTestProjectOpgaveFileIO
{
    public  class UnitTestTown
    {
        [Theory]
        [InlineData(1, "gent")]
        [InlineData(2, "luik")]
        [InlineData(3, "genk")]

        public void ProvinceConstructor_test(int Id, string Name)
        {
            Province p = new Province(Id, Name);

            Assert.Equal(Id, p.Id);
            Assert.Equal(Name, p.Name);
        }

        [Theory]
        [InlineData("dreef", 1, "dreef", 1)]
        [InlineData("straat", 2, "straat", 2)]
        [InlineData("weg", 3, "weg", 3)]
        public void AddStreet_test(string name, int id, string correctName, int correctId)
        {
            Town t = new Town(1, "Antwerpen");
            Street street = new Street(id, name);
            t.addStreet(street);

            Assert.Equal(correctName, t.GetStreets()[0].Name);
            Assert.Equal(correctId, t.GetStreets()[0].Id);
        }

        [Fact]

        public void GetStreets_test()
        {
            Town t = new Town(1, "Antwerpen");
            Street s = new Street(1, "dreef");
            Street s2 = new Street(2, "weg");
            t.addStreet(s);
            t.addStreet(s2);

            List<ILocation> streets = t.GetStreets();

            Assert.Equal(2, streets.Count);
            Assert.Equal(1, streets[0].Id);
            Assert.Equal("weg", streets[1].Name);
        }
    }
}
