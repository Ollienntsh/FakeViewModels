namespace FakeViewModels.Core
{
    public class FakeCollectionAttribute : FakeDataAttribute
    {
        public int ItemCount { get; private set; }

        public FakeCollectionAttribute(int itemCount)
        {
            this.ItemCount = itemCount;
        }
    }
}
