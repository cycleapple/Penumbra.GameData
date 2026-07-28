using Dalamud.Game;
using Dalamud.Plugin.Services;
using Lumina.Excel;

namespace Penumbra.GameData;

/// <summary>Compatibility helpers for game clients whose configured language has no matching Excel pages.</summary>
public static class ExcelSheetExtensions
{
    /// <summary>Return a language backed by pages in the TW client's Excel data.</summary>
    public static ClientLanguage GetSafeLanguage(this IDataManager dataManager)
        => dataManager.Language switch
        {
            ClientLanguage.Japanese or
            ClientLanguage.English or
            ClientLanguage.German or
            ClientLanguage.French => dataManager.Language,
            _                     => ClientLanguage.English,
        };

    /// <summary>Load a sheet using English when the TW client reports its unsupported legacy language slot.</summary>
    public static ExcelSheet<T> GetSafeExcelSheet<T>(
        this IDataManager dataManager,
        ClientLanguage? language = null,
        string? name = null)
        where T : struct, IExcelRow<T>
    {
        var actualLanguage = language ?? dataManager.GetSafeLanguage();
        actualLanguage = actualLanguage switch
        {
            ClientLanguage.Japanese or
            ClientLanguage.English or
            ClientLanguage.German or
            ClientLanguage.French => actualLanguage,
            _                     => ClientLanguage.English,
        };

        return dataManager.GetExcelSheet<T>(actualLanguage, name);
    }
}
