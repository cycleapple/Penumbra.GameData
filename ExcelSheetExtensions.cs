using Dalamud.Game;
using Dalamud.Plugin.Services;
using Lumina.Excel;

namespace Penumbra.GameData;

/// <summary>Compatibility helpers for game clients whose configured language has no matching Excel pages.</summary>
public static class ExcelSheetExtensions
{
    /// <summary>Preserve the launcher's language token for caches and non-Excel game APIs.</summary>
    public static ClientLanguage GetSafeLanguage(this IDataManager dataManager)
        => dataManager.Language;

    /// <summary>Load TC Excel pages when API 13 can not map the launcher's language to Lumina.</summary>
    public static ExcelSheet<T> GetSafeExcelSheet<T>(
        this IDataManager dataManager,
        ClientLanguage? language = null,
        string? name = null)
        where T : struct, IExcelRow<T>
    {
        try
        {
            return dataManager.GetExcelSheet<T>(language ?? dataManager.Language, name);
        }
        catch (Lumina.Excel.Exceptions.UnsupportedLanguageException)
        {
            return dataManager.GameData.Excel.GetSheet<T>((Lumina.Data.Language)8, name);
        }
    }
}
