using System.Reflection;

namespace BankMore.Transfer.API;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}