namespace EmployeeStructure.BusinessLayer.Extensions;

internal static class DateOnlyExtensions
{
    /// <summary>
    /// Calculates difference in whole years between two dates
    /// </summary>
    /// <param name="thisDate">Base of calculation</param>
    /// <param name="otherDate">Date to get difference from the base date</param>
    public static int DifferenceInYears(this DateOnly thisDate, DateOnly otherDate)
    {
        var years = otherDate.Year - thisDate.Year;
        if (years > 0 && thisDate.AddYears(years) > otherDate)
            years--;
        else if (years < 0 && thisDate.AddYears(years) < otherDate)
            years++;

        return years;
    }
}
