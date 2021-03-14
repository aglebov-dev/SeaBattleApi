namespace SeaBattle.Common.DataValidation
{
    public interface IModelValidator
    {
        bool CanValiate<T>(T model);
        ValidationData Validate<T>(T model);
    }
}
