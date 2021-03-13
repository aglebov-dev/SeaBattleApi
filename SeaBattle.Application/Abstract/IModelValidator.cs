namespace SeaBattle.Application.Abstract
{
    public interface IModelValidator
    {
        bool CanValiate<T>(T model);
        ValidationData Validate<T>(T model);
    }
}
