namespace FakeViewModels.Core
{
    #region types

    public enum FakeImageType
    {
        Cats, // default
        City,
        People
    } 

    #endregion

    public class FakeImageAttribute : FakeDataAttribute
    {
        #region properties

        public int? Height { get; private set; }

        public FakeImageType ImageType { get; private set; }

        public int? Width { get; private set; }

        #endregion

        #region constructors

        public FakeImageAttribute()
        {
        }

        public FakeImageAttribute(int width, int height, FakeImageType imageType)
        {
            this.Width = width;
            this.Height = height;
            this.ImageType = imageType;
        } 

        #endregion
    }
}
