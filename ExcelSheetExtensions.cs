using Dalamud.Game;
using Dalamud.Plugin.Services;
using Lumina.Excel;

namespace Penumbra.GameData;

/// <summary>Compatibility helpers for game clients whose configured language has no matching Excel pages.</summary>
public static class ExcelSheetExtensions
{
    /// <summary>Return a defined language token for caches and non-Excel Dalamud APIs.</summary>
    public static ClientLanguage GetSafeLanguage(this IDataManager dataManager)
        => Enum.IsDefined(dataManager.Language)
            ? dataManager.Language
            : ClientLanguage.ChineseSimplified;

    /// <summary>Load TC Excel pages when API 13 receives the TW client's unsupported language slot.</summary>
    public static ExcelSheet<T> GetSafeExcelSheet<T>(
        this IDataManager dataManager,
        ClientLanguage? language = null,
        string? name = null)
        where T : struct, IExcelRow<T>
    {
        if (!Enum.IsDefined(dataManager.Language))
            return dataManager.GameData.GetExcelSheet<T>((Lumina.Data.Language)8, name)!;

        return dataManager.GetExcelSheet<T>(language ?? dataManager.Language, name);
    }
}
