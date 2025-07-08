namespace BankMore.Core.Domain.Resources;

public static class LogTemplate
{
    public const string StartHandler = "[Iniciando] handler: {handler}.";
    public const string EndHandler = "[Finalizando] handler: {handler}. | {message} ";
    public const string ErrorHandler = "[Finalizando] handler: {handler}. | [Error] : {message} ";
    public const string WarningHandler = "[Finalizando] handler: {handler}. | [Warning] : {message} ";
}
