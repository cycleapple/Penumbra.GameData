using Dalamud.Game;
using Dalamud.Plugin.Services;
using Lumina.Excel;

namespace Penumbra.GameData;

/// <summary>Compatibility helpers for game clients whose configured language has no matching Excel pages.</summary>
public static class ExcelSheetExtensions
{
    /// <summary>Return a language backed by pages in the TW client's Excel data.</summary>
    public static ClientLanguage GetSafeLanguage(this IDataManager dataManager)
        => NormalizeLanguage(dataManager.Language);

    /// <summary>Load a sheet using English when the TW client reports its unsupported legacy language slot.</summary>
    public static ExcelSheet<T> GetSafeExcelSheet<T>(
        this IDataManager dataManager,
        ClientLanguage? language = null,
        string? name = null)
        where T : struct, IExcelRow<T>
    {
        var actualLanguage = language ?? dataManager.GetSafeLanguage();
        actualLanguage = NormalizeLanguage(actualLanguage);

        return dataManager.GetExcelSheet<T>(actualLanguage, name);
    }

    private static ClientLanguage NormalizeLanguage(ClientLanguage language)
        => language switch
        {
            ClientLanguage.ChineseTraditional => ClientLanguage.TraditionalChinese,
            ClientLanguage.Japanese or
            ClientLanguage.English or
            ClientLanguage.German or
            ClientLanguage.French or
            ClientLanguage.ChineseSimplified or
            ClientLanguage.Korean or
            ClientLanguage.TraditionalChinese => language,
            _                                 => ClientLanguage.TraditionalChinese,
        };
}
