namespace Model.EnumerationClasses;

public static class DocumentNotification
{
    public const string Deleted = "DocumentDeleted";
    public const string Notify = "NotifyChanges";
    public const string Added = "DocumentAdded";
    public const string StatutChanged = "StatutChanged";
    public const string ScanResult = "GetScanResult";
}