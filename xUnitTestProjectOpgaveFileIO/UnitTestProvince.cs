using AdressDataLibrary;

namespace xUnitTestProjectOpgaveFileIO
{
    public class UnitTestProvince
    {
        [Theory]
        [InlineData(1, "Oost-Vlaanderen")]
        [InlineData(2, "Antwerpen")]
        [InlineData(3, "Limburg")]

        public void ProvinceConstructor_test(int provinceId, string provinceName)
        {
            Province p = new Province(provinceId, provinceName);

            Assert.Equal(provinceId, p.Id);
            Assert.Equal(provinceName, p.Name);
        }

        [Theory]
        [InlineData("gent", 1,"gent", 1)]
        [InlineData("antwerpen", 2, "antwerpen", 2)]
        [InlineData("luik", 3, "luik", 3)]
        public void AddTown_test(string name,int id,string correctName, int correctId)
        {
            Province p = new Province(1, "Antwerpen");
            Town town = new Town(id, name);
            p.addTown(town);

            Assert.Equal(correctName, p.GetTowns()[0].Name);
            Assert.Equal(correctId, p.GetTowns()[0].Id);
        }

        [Fact]

        public void GetTowns_test()
        {
            Province p = new Province(1, "antwerpen");
            Town town = new Town(1, "gent");
            Town town2 = new Town(2, "Luik");
            p.addTown(town);
            p.addTown(town2);

            List<ILocation> towns = p.GetTowns();

            Assert.Equal(2, towns.Count);
            Assert.Equal(1, towns[0].Id);
            Assert.Equal("Luik", towns[1].Name);
        }
    }
}
