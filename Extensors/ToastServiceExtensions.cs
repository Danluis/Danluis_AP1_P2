using BlazorBootstrap;

namespace Danluis_AP1_P2.Extensors;

public static class ToastServiceExtensions
{
    public static void ShowSuccess(this ToastService toastService, string message)
    {
        toastService.Notify(new ToastMessage(ToastType.Success, message));
    }

    public static void ShowError(this ToastService toastService, string message)
    {
        toastService.Notify(new ToastMessage(ToastType.Danger, message));
    }

    public static void ShowWarning(this ToastService toastService, string message)
    {
        toastService.Notify(new ToastMessage(ToastType.Warning, message));
    }
}