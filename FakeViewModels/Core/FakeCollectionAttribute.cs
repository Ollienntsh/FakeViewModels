namespace FakeViewModels.Core
{
    public class FakeCollectionAttribute : FakeDataAttribute
    {
        #region properties

        public int ItemCount { get; private set; }

        #endregion

        #region constructor

        public FakeCollectionAttribute(int itemCount)
        {
            this.ItemCount = itemCount;
        } 

        #endregion
    }
}
