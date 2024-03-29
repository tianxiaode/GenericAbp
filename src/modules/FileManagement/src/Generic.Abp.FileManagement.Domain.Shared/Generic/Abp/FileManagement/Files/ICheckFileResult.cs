﻿using System.Collections.Generic;

namespace Generic.Abp.FileManagement.Files;

public interface ICheckFileResult: IHasHash
{
    bool IsExits {get;}
    IFile File { get; set; }
    Dictionary<int,bool> Uploaded { get; }

}