namespace SeaBattle.Common.DataValidation
{
    public interface IValidationService
    {
        ValidationData Validate<T>(T model);
    }
}
