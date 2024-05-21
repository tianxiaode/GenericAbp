# 设置模板根目录和目标路径
$templateRoot = Join-Path -Path $PSScriptRoot -ChildPath "..\src\templates"

# 获取所有模板目录
$templateDirectories = Get-ChildItem -Path $templateRoot -Directory

# 遍历每个模板目录并注册模板
foreach ($templateDir in $templateDirectories) {
    $templatePath = $templateDir.FullName
    Write-Host "Registering template from $templatePath..."
    dotnet new install --force $templatePath 

    # 检查注册是否成功并输出相应消息
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Template from $templatePath registered successfully."
    }
    else {
        Write-Host "Template registration from $templatePath failed."
    }
}