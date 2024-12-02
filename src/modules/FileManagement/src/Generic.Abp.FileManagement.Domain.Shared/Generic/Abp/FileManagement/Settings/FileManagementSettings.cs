using Generic.Abp.Extensions.Settings;
using System;
using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Settings;

public static class FileManagementSettings
{
    public const string GroupName = "GenericAbp.FileManagement";

    // 提取的常量
    public const string StoragePath = "StoragePath";
    public const string FolderCopyMaxNodeCount = "FolderCopyMaxNodeCount";
    public const string EnablePersonalFolderForRoles = "EnablePersonalFolderForRoles";
    public const string ExpirationDateOfExternalShared = "ExpirationDateOfExternalShared";
    public const string PublicFolderStorageQuota = "PublicFolder.StorageQuota";
    public const string PublicFolderMaxFileSize = "PublicFolder.MaxFileSize";
    public const string PublicFolderAllowedFileTypes = "PublicFolder.AllowedFileTypes";
    public const string SharedFolderStorageQuota = "SharedFolder.StorageQuota";
    public const string SharedFolderMaxFileSize = "SharedFolder.MaxFileSize";
    public const string SharedFolderAllowedFileTypes = "SharedFolder.AllowedFileTypes";
    public const string UserFolderStorageQuota = "UserFolder.StorageQuota";
    public const string UserFolderMaxFileSize = "UserFolder.MaxFileSize";
    public const string UserFolderAllowedFileTypes = "UserFolder.AllowedFileTypes";
    public const string ParticipantIsolationFolderStorageQuota = "ParticipantIsolationFolder.StorageQuota";
    public const string ParticipantIsolationFolderMaxFileSize = "ParticipantIsolationFolder.MaxFileSize";
    public const string ParticipantIsolationFolderAllowedFileTypes = "ParticipantIsolationFolder.AllowedFileTypes";
    public const string DefaultFileCleanupEnable = "DefaultFile.Cleanup.Enable";
    public const string DefaultFileCleanupFrequency = "DefaultFile.Cleanup.Frequency";
    public const string DefaultFileCleanupBatchSize = "DefaultFile.Cleanup.BatchSize";
    public const string DefaultFileUpdateEnable = "DefaultFile.Update.Enable";
    public const string DefaultFileUpdateRetentionPeriod = "DefaultFile.Update.RetentionPeriod";
    public const string DefaultFileUpdateFrequency = "DefaultFile.Update.Frequency";
    public const string DefaultFileUpdateBatchSize = "DefaultFile.Update.BatchSize";
    public const string TemporaryFileCleanupEnable = "TemporaryFile.Cleanup.Enable";
    public const string TemporaryFileCleanupFrequency = "TemporaryFile.Cleanup.Frequency";
    public const string TemporaryFileCleanupBatchSize = "TemporaryFile.Cleanup.BatchSize";
    public const string TemporaryFileUpdateEnable = "TemporaryFile.Update.Enable";
    public const string TemporaryFileUpdateRetentionPeriod = "TemporaryFile.Update.RetentionPeriod";
    public const string TemporaryFileUpdateFrequency = "TemporaryFile.Update.Frequency";
    public const string TemporaryFileUpdateBatchSize = "TemporaryFile.Update.BatchSize";
    public static readonly string[] CleanupOrUpdateSettingPrefixes = ["DefaultFile", "TemporaryFile"];
    public static readonly string[] CleanupOrUpdateSettingTypes = ["Cleanup", "Update"];

    public static readonly string[] CleanupOrUpdateSettingValueNames =
        ["Enable", "RetentionPeriod", "Frequency", "BatchSize"];

    public static Dictionary<string, ISettingDefinitionExtensions> Settings => new()
    {
        { StoragePath, new SettingDefinitionExtensions("/files") },
        { FolderCopyMaxNodeCount, new IntSettingDefinitionExtensions(500) },
        { EnablePersonalFolderForRoles, new SettingDefinitionExtensions("") },
        { ExpirationDateOfExternalShared, new IntSettingDefinitionExtensions(7) },
        { PublicFolderStorageQuota, new SettingDefinitionExtensions("10GB", "StorageQuota") },
        { PublicFolderMaxFileSize, new SettingDefinitionExtensions("10MB", "MaxFileSize") },
        {
            PublicFolderAllowedFileTypes, new SettingDefinitionExtensions(
                "jpg,jpeg,png,gif,svg,pdf,txt,doc,docx,xls,xlsx,ppt,pptx,zip,rar,mp4,avi,flv,wmv,mov,mkv,webm,ogg,mp3,wav,aac,m4a,flac,amr,wma,aac",
                "AllowedFileTypes")
        },
        { SharedFolderStorageQuota, new SettingDefinitionExtensions("10GB", "StorageQuota") },
        { SharedFolderMaxFileSize, new SettingDefinitionExtensions("100MB", "MaxFileSize") },
        {
            SharedFolderAllowedFileTypes, new SettingDefinitionExtensions(
                "jpg,jpeg,png,gif,svg,pdf,txt,doc,docx,xls,xlsx,ppt,pptx,zip,rar,mp4,avi,flv,wmv,mov,mkv,webm,ogg,mp3,wav,aac,m4a,flac,amr,wma,aac",
                "AllowedFileTypes")
        },
        { UserFolderStorageQuota, new SettingDefinitionExtensions("1GB", "StorageQuota") },
        { UserFolderMaxFileSize, new SettingDefinitionExtensions("2MB", "MaxFileSize") },
        {
            UserFolderAllowedFileTypes, new SettingDefinitionExtensions(
                "jpg,jpeg,png,gif,svg,pdf,txt,doc,docx,xls,xlsx,ppt,pptx",
                "AllowedFileTypes")
        },
        { ParticipantIsolationFolderStorageQuota, new SettingDefinitionExtensions("1GB", "StorageQuota") },
        { ParticipantIsolationFolderMaxFileSize, new SettingDefinitionExtensions("1MB", "MaxFileSize") },
        {
            ParticipantIsolationFolderAllowedFileTypes, new SettingDefinitionExtensions(
                "jpg,jpeg,png,gif,svg,pdf,txt,doc,docx,xls,xlsx,ppt,pptx",
                "AllowedFileTypes")
        },
        { DefaultFileCleanupEnable, new BooleanSettingDefinitionExtensions(true, "Enable") },
        { DefaultFileCleanupFrequency, new LongSettingDefinitionExtensions(3600 * 24 * 15, "Frequency") },
        { DefaultFileCleanupBatchSize, new IntSettingDefinitionExtensions(500, "BatchSize") },
        { DefaultFileUpdateEnable, new BooleanSettingDefinitionExtensions(true, "Enable") },
        { DefaultFileUpdateRetentionPeriod, new IntSettingDefinitionExtensions(7, "RetentionPeriod") },
        { DefaultFileUpdateFrequency, new LongSettingDefinitionExtensions(3600 * 24 * 15, "Frequency") },
        { DefaultFileUpdateBatchSize, new IntSettingDefinitionExtensions(500, "BatchSize") },
        { TemporaryFileCleanupEnable, new BooleanSettingDefinitionExtensions(true, "Enable") },
        { TemporaryFileCleanupFrequency, new LongSettingDefinitionExtensions(3600 * 24 * 7, "Frequency") },
        { TemporaryFileCleanupBatchSize, new IntSettingDefinitionExtensions(500, "BatchSize") },
        { TemporaryFileUpdateEnable, new BooleanSettingDefinitionExtensions(true, "Enable") },
        { TemporaryFileUpdateRetentionPeriod, new IntSettingDefinitionExtensions(1, "RetentionPeriod") },
        { TemporaryFileUpdateFrequency, new LongSettingDefinitionExtensions(3600 * 24 * 7, "Frequency") },
        { TemporaryFileUpdateBatchSize, new IntSettingDefinitionExtensions(500, "BatchSize") },
    };
}