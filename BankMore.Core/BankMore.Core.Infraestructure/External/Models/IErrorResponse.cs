namespace BankMore.Core.Infraestructure.External.Models;

public interface IErrorResponse
{
    string GetErrorCode();
    string GetErrorDescription();
}
