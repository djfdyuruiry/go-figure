namespace GoFigure.App.ViewModels.Interfaces
{
    public interface IStatusViewModel
    {
        string Score { get; set; }
        
        string Target { get; set; }

        string Time { get; }
    }
}