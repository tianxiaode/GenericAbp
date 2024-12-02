using System.Collections.Generic;
using Generic.Abp.Extensions.Extensions;

namespace Generic.Abp.FileManagement.Settings;

public static class FileManagementDefaultSettings
{
    public const string DefaultStoragePath = "/default/path";
    public const int DefaultFolderCopyMaxNodeCount = 100;
    public const int DefaultExternalShareExpiration = 7; // 7 days

    public const string PublicFolder = "PublicFolder";
    public const string SharedFolder = "SharedFolder";
    public const string UserFolder = "UserFolder";
    public const string ParticipantIsolationFolder = "ParticipantIsolationFolder";
    public const string DefaultFileUpdate = "DefaultFile.Update";
    public const string DefaultFileCleanup = "DefaultFile.Cleanup";
    public const string TemporaryFileUpdate = "TemporaryFile.Update";
    public const string TemporaryFileCleanup = "TemporaryFile.Cleanup";

    public static readonly Dictionary<string, FolderSetting> FolderSettings = new()
    {
        {
            PublicFolder,
            new FolderSetting("10GB".ParseToBytes(), "10MB".ParseToBytes(),
                "jpg,jpeg,png,gif,svg,pdf,txt,doc,docx,xls,xlsx,ppt,pptx,zip,rar,mp4,avi,flv,wmv,mov,mkv,webm,ogg,mp3,wav,aac,m4a,flac,amr,wma,aac")
        },
        {
            SharedFolder,
            new FolderSetting("10GB".ParseToBytes(), "5MB".ParseToBytes(),
                "jpg,jpeg,png,gif,svg,pdf,txt,doc,docx,xls,xlsx,ppt,pptx,zip,rar,mp4,avi,flv,wmv,mov,mkv,webm,ogg,mp3,wav,aac,m4a,flac,amr,wma,aac")
        },
        {
            UserFolder,
            new FolderSetting("500MB".ParseToBytes(), "2MB".ParseToBytes(),
                "jpg,jpeg,png,gif,svg,pdf,txt,doc,docx,xls,xlsx,ppt,pptx,zip,rar,mp4,avi,flv,wmv,mov,mkv,webm,ogg,mp3,wav,aac,m4a,flac,amr,wma,aac")
        },
        {
            ParticipantIsolationFolder,
            new FolderSetting("1GB".ParseToBytes(), "1MB".ParseToBytes(),
                "jpg,jpeg,png,gif,svg,pdf,txt,doc,docx,xls,xlsx,ppt,pptx")
        }
    };

    public static readonly Dictionary<string, CleanupOrUpdateSetting> CleanupOrUpdateSettings = new()
    {
        {
            DefaultFileUpdate,
            new CleanupOrUpdateSetting
            {
                Enable = true,
                RetentionPeriod = 30,
                Frequency = 3600 * 24 * 3, // 72 hours
                BatchSize = 100
            }
        },
        {
            DefaultFileCleanup,
            new CleanupOrUpdateSetting
            {
                Enable = false,
                Frequency = 3600 * 24 * 15, // 15 days
                BatchSize = 50
            }
        },
        {
            TemporaryFileUpdate,
            new CleanupOrUpdateSetting
            {
                Enable = true,
                RetentionPeriod = 7,
                Frequency = 3600 * 24, // 24 hours
                BatchSize = 10
            }
        },
        {
            TemporaryFileCleanup,
            new CleanupOrUpdateSetting
            {
                Enable = true,
                Frequency = 3600 * 24 * 7, // 7 days
                BatchSize = 20
            }
        }
    };
}