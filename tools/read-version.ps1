[xml]$xml = Get-Content ".\src\common.props"
Write-Host $xml.SelectSingleNode('//Version')."#text"