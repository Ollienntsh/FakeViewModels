namespace FakeViewModels.Core
{
    public enum FakeDateType
    {
        Birthday,
        Future,
        Past
    }

    public class FakeDateAttribute : FakeDataAttribute
    {
        public FakeDateType DateType { get; private set; }

        public FakeDateAttribute(FakeDateType dateType)
        {
            this.DateType = dateType;
        }
    }
}
