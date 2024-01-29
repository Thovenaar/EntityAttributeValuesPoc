namespace EntityAttributeValuesTests.Utils
{
    /// <summary>
    /// This class contains all collections of tests.
    /// Please create a new collection for each new test case that needs their own seed data.
    /// </summary>
    public class SharedTestCollections
    {
        public const string CommonTestsCollection = "CommonTestsCollection";
    }

    [CollectionDefinition(SharedTestCollections.CommonTestsCollection)]
    public class GenericTestsCollection : ICollectionFixture<CommonTestsFactory>
    {
    }
}
