
# 获取 main 分支最近一次修改 common.props 文件的提交的版本号

# 获取 main 分支最近一次修改 common.props 文件的提交的版本号
git checkout -b main origin/main
$commitHash = git log -n 1 main --pretty=format:"%H" --diff-filter=M -- ../src/common.props

$latestContent = git show $commitHash -- ../src/common.props

# 从文件内容中提取版本号
$versionPattern = '<Version>([\d]+\.[\d]+\.[\d]+(-[a-zA-Z0-9]+)?)</Version>'
try {
    # 尝试切换到 dev 分支
    git checkout dev
}
catch {
    # 如果切换失败，则创建一个新的 dev 分支并将其设置为跟踪远程仓库的 origin/dev 分支
    git checkout -b dev origin/dev
}
$cureentContent = git show dev -- ../src/common.props

if ([string]::IsNullOrEmpty($latestContent) -or [string]::IsNullOrEmpty($cureentContent)) {
    Add-Content -Path $env:GITHUB_ENV -Value "change=false"
}
else {
    $mainVersion = [regex]::Match($latestContent, $versionPattern)
    $currentVersion = [regex]::Match($cureentContent, $versionPattern)

    if ($currentVersion -ne $mainVersion) {
        Add-Content -Path $env:GITHUB_ENV -Value "change=true"
    }
    else {
        Add-Content -Path $env:GITHUB_ENV -Value "change=false"

    }

}
