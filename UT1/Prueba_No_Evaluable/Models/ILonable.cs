using System.Reflection.Metadata.Ecma335;

namespace Prueba_No_Evaluable;

public class LoanDays
{
    public int TotalDays { get; set; }
}

public class CalculateFine
{
    public static bool IsLate(int daysLate)
    {
        LoanDays loan = new LoanDays();

        if (loan.TotalDays < daysLate)
        {
            return true;
        }

        return false;
    }
}