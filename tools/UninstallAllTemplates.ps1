# 设置模板根目录和目标路径
$templateRoot = Join-Path -Path $PSScriptRoot -ChildPath "..\src\templates"

# 获取所有模板目录
$templateDirectories = Get-ChildItem -Path $templateRoot -Directory

# 删除每个模板
foreach ($template in $templateDirectories) {
    $templatePath = $templateDir.FullName
    Write-Host "Uninstalling template: $templatePath"
    dotnet new uninstall $templatePath
}

Write-Host "All templates uninstalled successfully from path: $templatePath."
