namespace FakeViewModels.Core
{
    public enum FakeImageType
    {
        Cats, // default
        City,
        People
    }

    public class FakeImageAttribute : FakeDataAttribute
    {
        public int? Height { get; private set; }

        public FakeImageType ImageType { get; private set; }

        public int? Width { get; private set; }

        public FakeImageAttribute()
        {
        }

        public FakeImageAttribute(int width, int height, FakeImageType imageType)
        {
            this.Width = width;
            this.Height = height;
            this.ImageType = imageType;
        }
    }
}
