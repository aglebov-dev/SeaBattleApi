namespace SeaBattle.Application.Abstract
{
    public interface IValidationService
    {
        ValidationData Validate<T>(T model);
    }
}
