namespace FakeViewModels.Core
{
    #region types

    public enum FakeDateType
    {
        Birthday,
        Future,
        Past
    } 

    #endregion

    public class FakeDateAttribute : FakeDataAttribute
    {
        #region properties

        public FakeDateType DateType { get; private set; }

        #endregion

        #region constructor

        public FakeDateAttribute(FakeDateType dateType)
        {
            this.DateType = dateType;
        } 

        #endregion
    }
}
