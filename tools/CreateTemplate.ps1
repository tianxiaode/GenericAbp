param (
    [string]$templateType,
    [string]$outputPath,
    [string[]]$templateArgs
)

# 定义模板类型和对应的模板名称
$templates = @{
    "module" = "module";
    "pages"     = "pages";
    # 添加更多模板类型和对应的模板名称
}

# 检查模板类型是否有效
if (-not $templates.ContainsKey($templateType)) {
    Write-Host "Invalid template type specified."
    exit
}

# 创建模板
$templateName = $templates[$templateType]
$cmdArgs = "new $templateName -o $outputPath"

if ($templateArgs) {
    $cmdArgs += " $($templateArgs -join ' ')"
}

Write-Host "Creating template '$templateType' in '$outputPath'..."

# 执行命令
Invoke-Expression "dotnet $cmdArgs"

# 检查是否成功并输出相应消息
if ($LASTEXITCODE -eq 0) {
    Write-Host "Template '$templateType' created successfully in '$outputPath'."
}
else {
    Write-Host "Failed to create template '$templateType' in '$outputPath'."
}
