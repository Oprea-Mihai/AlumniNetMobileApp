using AlumniNetMobile.ViewModels;

namespace AlumniNetMobile.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            ProfileViewModel viewModel = new ProfileViewModel();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}