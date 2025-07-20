
$lines = Get-Content "NHibernateVersions.txt"
foreach ($line in $lines) {
    Write-Host "Testing version: $line" -ForegroundColor Cyan
    dotnet test OpenProfiler.NHibernate.Test\OpenProfiler.NHibernate.Test.csproj --property:NHibernateVersion=$line

    if ($LastExitCode -ne 0) {
        Throw "Test failed at: $line"
    }
}